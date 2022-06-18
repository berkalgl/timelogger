using System;

namespace Timelogger.Domain.Core.Exceptions
{
    public class TimeloggerException : Exception
    {
        public TimeloggerException() { }

        public TimeloggerException(string message)
            : base(message)
        {
            BusinessMessage = message;
        }
        public string BusinessMessage { get; set; }
    }
}
