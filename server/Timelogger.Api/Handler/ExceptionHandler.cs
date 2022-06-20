using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Timelogger.Api.Common;
using Timelogger.Domain.Core.Exceptions;

namespace Timelogger.Api.Handler
{
    public class ExceptionHandler
    {
        public static async Task HandleException(HttpContext filterContext)
        {
            var exceptionHandlerPathFeature =
                filterContext.Features.Get<IExceptionHandlerPathFeature>();

            var exception = exceptionHandlerPathFeature.Error;
            try
            {
                //we can log the exception here to db or somewhere else
            }
            catch (Exception)
            {
                //log exception to a file if something is wrong in logging somewhere else
            }

            ServiceResult result;
            if (exception is TimeloggerException timeloggerException)
            {
                result = new ServiceResult
                {
                    State = ServiceResultStates.ERROR,
                    Message = timeloggerException.BusinessMessage
                };
            }
            else
            {
                // We should not show exception message to the client, it is security risk. But in this case we can.
                result = new ServiceResult
                {
                    State = ServiceResultStates.ERROR,
                    Message = "An error occured during process" + ":" + exception.Message
                };
            }
            filterContext.Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            filterContext.Response.ContentType = new MediaTypeHeaderValue("application/json").ToString();
            await filterContext.Response.WriteAsync(JsonConvert.SerializeObject(result, new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() }));

        }
    }
}
