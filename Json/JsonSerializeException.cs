using System;

namespace Jeremy.Tools.Json
{
    public class JsonSerializeException : Exception
    {
        public JsonSerializeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}