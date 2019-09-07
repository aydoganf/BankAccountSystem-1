using AydoganFBank.Context.Exception;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace aydoganfbank.web.api2.Utility
{
    public static class ActionHandler
    {
        public static ActionResult<ApiResponse<T>> HandleResult<T>(this ControllerBase controller, Func<T> action)
        {
            try
            {
                T result = (T)action.Invoke();
                if (result == default)
                    return controller.HandleResult((default(T)), HttpStatusCode.NotFound, 0, null);
                return controller.HandleResult(result, HttpStatusCode.OK, 0, "success");
            }
            catch (Exception ex)
            {
                if (ex is ServiceException serviceException)
                {
                    if (serviceException.ExceptionCode == 10000)
                        return controller.HandleResult<T>(default, HttpStatusCode.Unauthorized, 10000, "unauthorized");

                    if (serviceException.ExceptionCode > 10000)
                    {
                        return controller.HandleResult<T>(default, HttpStatusCode.BadRequest,
                            serviceException.ExceptionCode, serviceException.ExceptionMessage);
                    }
                        
                }

                return HandleResult<T>(controller, default, HttpStatusCode.InternalServerError, 1, "failed");
            }
        }

        public static ActionResult<ApiResponse<T>> HandleResult<T>(this ControllerBase controller, T result,
            HttpStatusCode statusCode, int resultCode, string resultMessage)
        {
            InitContextItems(controller);

            controller.HttpContext.Items["resultCode"] = resultCode;
            controller.HttpContext.Items["resultMessage"] = resultMessage;

            switch (statusCode)
            {
                case HttpStatusCode.OK:
                    return controller.Ok(result);
                case HttpStatusCode.InternalServerError:
                case HttpStatusCode.BadRequest:
                    return controller.BadRequest(new { });
                case HttpStatusCode.NotFound:
                    return controller.NotFound();
                default:
                    return controller.StatusCode((int)statusCode, result);
            }
        }

        public static void InitContextItems(this ControllerBase controller)
        {
            if (!controller.HttpContext.Items.ContainsKey("resultMessage"))
                controller.HttpContext.Items.Add("resultMessage", "");
            if (!controller.HttpContext.Items.ContainsKey("resultCode"))
                controller.HttpContext.Items.Add("resultCode", "");
        }
    }
}
