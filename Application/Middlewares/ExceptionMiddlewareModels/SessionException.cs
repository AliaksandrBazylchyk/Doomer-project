﻿namespace Application.Middlewares.ExceptionMiddlewareModels
{
    public class SessionException : Exception
    {
        public SessionException() { }
        public SessionException(string message) : base(message) { }
        public SessionException(string message, Exception innerException) : base(message, innerException) { }
    }
}
