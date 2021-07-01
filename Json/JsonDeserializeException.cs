using System;

namespace Jeremy.Tools.Json
{
    public class JsonDeserializeException : Exception
    {
        public JsonDeserializeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}