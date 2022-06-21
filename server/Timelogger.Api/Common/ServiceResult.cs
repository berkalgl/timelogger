using System.Collections.Generic;

namespace Timelogger.Api.Common
{
    public class ServiceResult
    {
        public ServiceResultStates State { get; set; }
        public string Message { get; set; }
    }

    public class ServiceResult<T> : ServiceResult where T : class
    {
        public ServiceResult() { }

        public ServiceResult(T result, string message = "", ServiceResultStates state = ServiceResultStates.SUCCESS)
        {
            Result = result;
            State = state;
            Message = message;
        }

        public T Result { get; set; }
    }

    public enum ServiceResultStates
    {
        SUCCESS,
        FAIL,
        WARNING,
        INFO,
        ERROR
    }
    public static class ReturnMessages
    {
        public static string Success = "Success";
        public static string AdditionSuccess = "Inserted Successfully";
        public static string DisabledSuccess = "Disabled Successfully";
    }
}
