using System;

namespace Logo.Domain.Exceptions 
{
    /// <summary>
    /// Base exception.
    /// </summary>
    [Serializable]
    public class LogoException : Exception
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public LogoException()
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="message"></param>
        public LogoException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public LogoException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
