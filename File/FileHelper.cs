using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using Jeremy.Tools.Common;
using Jeremy.Tools.Extensions;

namespace Jeremy.Tools.File
{
    public abstract class FileHelper : IDisposable
    {
        private bool _disposed;

        #region 构造

        protected FileHelper()
        {

        }

        #endregion


        #region 释放资源

        ~FileHelper()
        {
            Dispose(false);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                // todo: 填写需要被清理的托管资源
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion



        #region 写入

        /// <summary>
        /// 将指定内容写入到文件中。
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="contents">文件内容</param>
        /// <param name="isAppend">是否追加</param>
        /// <param name="encoding">指定编码格式</param>
        protected static async void WriteContentsToFile(string path, Stream contents, bool isAppend, Encoding encoding)
        {
            CreateDir(Path.GetDirectoryName(path));

            if (!System.IO.File.Exists(path))
            {
                var file = System.IO.File.Create(path);
                file.Close();
            }

            try
            {
                var bytes = new byte[contents.Length];
                await contents.ReadAsync(bytes.AsMemory(0, bytes.Length));
                contents.Seek(0, SeekOrigin.Begin);
                var fs = new FileStream(path, isAppend ? FileMode.Append : FileMode.Create, FileAccess.Write);
                var bw = new BinaryWriter(fs, encoding);
                bw.Write(bytes);
                bw.Flush();
                bw.Close();
                fs.Close();
            }
            catch (Exception e)
            {
                throw new Exception("Write stream to file error.", e);
            }
        }

        /// <summary>
        /// 将指定内容写入到文件中。
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="contents">文件内容</param>
        /// <param name="isAppend">是否追加</param>
        /// <param name="encoding">指定编码格式</param>
        protected static async void WriteContentsToFile(string path, string contents, bool isAppend, Encoding encoding)
        {
            CreateDir(Path.GetDirectoryName(path));

            if (!System.IO.File.Exists(path))
            {
                var file = System.IO.File.Create(path);
                file.Close();
            }

            try
            {
                var fileStream = new StreamWriter(path, isAppend, encoding);
                await fileStream.WriteAsync(contents);
                fileStream.Flush();
                fileStream.Close();
                await fileStream.DisposeAsync();
            }
            catch (Exception e)
            {
                throw new Exception("Write contents to file error.", e);
            }
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="contents">文件内容</param>
        /// <param name="encoding">指定文件编码</param>
        public static void Write(string path, string contents, Encoding encoding = null)
        {
            if (encoding == null) encoding = Encoding.UTF8;
            WriteContentsToFile(path, contents, false, encoding);
        }

        /// <summary>
        /// 将内容写入文件
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="contents">文件内容</param>
        /// <param name="encoding">指定文件编码</param>
        public static void Write(string path, string contents, string encoding)
        {
            WriteContentsToFile(path, contents, false, Encoding.GetEncoding(encoding));
        }

        /// <summary>
        /// 保存 base64 内容为图片，默认格式为 png
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="base64">base64 格式的图片数据</param>
        public static void WriteToImage(string path, string base64)
        {
            WriteToImage(path, base64, ImageFormat.Png);
        }

        /// <summary>
        /// 保存 base64 内容为图片
        /// </summary>
        /// <param name="path">文件路径，含文件名</param>
        /// <param name="base64">base64 格式的图片数据</param>
        /// <param name="suffix">指定保存格式</param>
        public static void WriteToImage(string path, string base64, ImageFormat suffix)
        {
            try
            {
                var arr = Convert.FromBase64String(base64.CheckAndCorrectBase64());
                var ms = new MemoryStream(arr);
                var bitmap = new Bitmap(ms);
                ms.Close();

                bitmap.Save(path, suffix);
                bitmap.Dispose();
            }
            catch (Exception e)
            {
                throw new Exception("Write to image error.", e);
            }
        }

        #endregion



        #region 创建

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDir(string path)
        {
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
        }

        /// <summary>
        /// 生成一个随机文件名
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string CreateRandomFileName(string filename)
        {
            var ext = Path.GetExtension(filename);
            var date = DateTime.Now.ToLongTimeString();
            var hashedName = (date + filename).ComputeHash();
            return $"{hashedName}{ext}";
        }

        #endregion



        #region 读取

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">指定文件编码</param>
        /// <returns></returns>
        public static string Read(string path, Encoding encoding = null)
        {
            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException("read file is not exists.");
            }

            if (encoding == null) encoding = Encoding.UTF8;

            var sr = new StreamReader(path, encoding);
            var res = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();

            return res;
        }

        /// <summary>
        /// 读取文件内容
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <param name="encoding">指定文件编码</param>
        public static string Read(string path, string encoding)
        {
            return Read(path, Encoding.GetEncoding(encoding));
        }

        #endregion



        #region 剪切和复制

        /// <summary>
        /// 移动文件到指定位置
        /// </summary>
        /// <param name="srcPath">文件源路径</param>
        /// <param name="destPath">文件目标路径</param>
        /// <param name="overwrite">是否允许覆盖同名文件</param>
        public static void Move(string srcPath, string destPath, bool overwrite = false)
        {
            var path = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            System.IO.File.Move(srcPath, destPath, overwrite);
        }

        /// <summary>
        /// 拷贝文件到指定位置
        /// </summary>
        /// <param name="srcPath">文件源路径</param>
        /// <param name="destPath">文件目标路径</param>
        /// <param name="overwrite">是否允许覆盖同名文件</param>
        public static void Copy(string srcPath, string destPath, bool overwrite = false)
        {
            var path = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(path)) Directory.CreateDirectory(path);
            System.IO.File.Copy(srcPath, destPath, overwrite);
        }

        /// <summary>
        /// 拷贝指定文件夹及所有内容到指定位置
        /// </summary>
        /// <param name="srcPath">文件夹源路径（根路径）</param>
        /// <param name="destPath">文件夹目标路径</param>
        public static void CopyDir(string srcPath, string destPath)
        {
            try
            {
                // 创建目标路径
                if (!Directory.Exists(destPath))
                {
                    Directory.CreateDirectory(destPath);
                }

                // 获取文件列表并开始复制
                var fileList = Directory.GetFileSystemEntries(srcPath);
                foreach (var file in fileList)
                {
                    var newPath = Path.Combine(destPath, Path.GetFileName(file));
                    if (Directory.Exists(file)) CopyDir(file, newPath);
                    Copy(file, newPath, true);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Copy directory error.", e);
            }
        }

        #endregion



        #region 删除

        /// <summary>
        /// 删除一个文件
        /// </summary>
        /// <param name="path">文件路径</param>
        public static void Delete(string path)
        {
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
        }

        /// <summary>
        /// 删除整个文件夹及其子内容
        /// </summary>
        /// <param name="path">文件夹根路径</param>
        public static void DeleteDir(string path)
        {
            if (!Directory.Exists(path)) return;

            var fileList = Directory.GetFileSystemEntries(path);
            foreach (var file in fileList)
            {
                if (System.IO.File.Exists(file)) System.IO.File.Delete(file);
                DeleteDir(file);
            }

            Directory.Delete(path);
        }

        #endregion



        #region 检查

        /// <summary>
        /// 判断文件是否为图片类型
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <returns></returns>
        public static bool IsImage(string fileName)
        {
            var mime = MimeMapping.GetMimeMapping(fileName);
            return mime.StartsWith("image");
        }

        #endregion
    }
}
