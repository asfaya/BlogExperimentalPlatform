namespace BlogExperimentalPlatform.Utils
{
    using System;

    public class BlogSystemException : Exception
    {
        public BlogSystemException()
        {
        }

        public BlogSystemException(string message) : base(message)
        {
        }

        public BlogSystemException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
