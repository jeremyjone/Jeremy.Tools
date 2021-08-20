namespace Jeremy.Tools.Net
{
    /// <summary>
    /// 三种状态，分别是成功、失败（网络问题）、错误（本地问题）。
    /// </summary>
    public enum Status
    {

        /// <summary>
        /// 成功
        /// </summary>
        Success,

        /// <summary>
        /// 失败，网络问题
        /// </summary>
        Fail,

        /// <summary>
        /// 错误，本地问题
        /// </summary>
        Error
    }
}