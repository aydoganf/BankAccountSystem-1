using AccountApp.Models.Operation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace AccountApp.Utility
{
    public class Application
    {
        private static int _APPLICATION_ID
        {
            get
            {
                string value = ConfigurationManager.AppSettings["APPLICATION_ID"];
                int id;

                int.TryParse(value, out id);
                return id;
            }
        }

        public static readonly int APPLICATION_ID = _APPLICATION_ID;
        public static readonly string SESSION_LOGIN_KEY = "login";

        public static void HandleException(Exception exception, OperationResult operationResult)
        {
            if (exception is AydoganFBank.Context.Exception.ServiceException)
            {
                var serviceEx = (AydoganFBank.Context.Exception.ServiceException)exception;
                operationResult.ResultMessage = serviceEx.ExceptionMessage;
                operationResult.ResultCode = serviceEx.ExceptionCode.ToString();
                return;
            }

            operationResult.ResultMessage = "Internal server error.";
            operationResult.ResultCode = "99999";
        }

        public static void HandleOperation(Exception exception, dynamic viewBag)
        {
            var operationResult = new OperationResult();

            if (exception is AydoganFBank.Context.Exception.ServiceException)
            {
                var serviceEx = (AydoganFBank.Context.Exception.ServiceException)exception;
                operationResult.ResultMessage = serviceEx.ExceptionMessage;
                operationResult.ResultCode = serviceEx.ExceptionCode.ToString();

                viewBag.OperationResult = operationResult;

                return;
            }

            operationResult.ResultMessage = "Internal server error.";
            operationResult.ResultCode = "99999";
            viewBag.OperationResult = operationResult;
        }

        public static void HandleWarning(dynamic viewBag, string message)
        {
            var operationResult = new OperationResult()
            {
                IsSuccess = false,
                ResultCode = "99998",
                ResultMessage = message
            };

            viewBag.OperationResult = operationResult;
        }

        public static void HandleOperation(dynamic viewBag, string message = "Operation is successfully done.")
        {
            var operationResult = new OperationResult()
            {
                IsSuccess = true,
                ResultCode = "000000",
                ResultMessage = message
            };

            viewBag.OperationResult = operationResult;
        }
    }
}