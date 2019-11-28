using AccountApp.Models.Operation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AccountApp.Utility
{
    public static class Application
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

        #region Security codes
        public static string GenerateCode(int length)
        {
            double lowerBound = Math.Pow(10, length - 1);            
            double higherBound = Math.Pow(10, length) - 1;

            if (length == 1)
            {
                lowerBound = 1;
                higherBound = 9;
            }

            Random random = new Random();
            return random.Next((int)lowerBound, (int)higherBound).ToString();
        }
        #endregion

        #region Vievbag handling

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
        #endregion

        #region Html helpers
        public enum ButtonType
        {
            Success,
            Info,
            Warning,
            Danger,
            Primary,
            Default
        }

        public enum ButtonSize
        {
            Normal,
            Large,
            Small,
            XSmall
        }

        public static string ToCss(this ButtonType buttonType)
        {
            switch (buttonType)
            {
                case ButtonType.Success:
                    return "btn btn-success";
                case ButtonType.Info:
                    return "btn btn-info";
                case ButtonType.Warning:
                    return "btn btn-warning";
                case ButtonType.Danger:
                    return "btn btn-danger";
                case ButtonType.Primary:
                    return "btn btn-primary";
                case ButtonType.Default:
                    return "btn btn-default";
                default:
                    return "btn btn-default";
            }
        }

        public static string ToCss(this ButtonSize buttonSize)
        {
            switch (buttonSize)
            {
                case ButtonSize.Normal:
                    return string.Empty;
                case ButtonSize.Large:
                    return "btn-lg";
                case ButtonSize.Small:
                    return "btn-sm";
                case ButtonSize.XSmall:
                    return "btn-xs";
                default:
                    return string.Empty;
            }
        }

        public enum AlertBoxType
        {
            Success,
            Info,
            Warning,
            Danger
        }

        public static string ToCss(this AlertBoxType boxType)
        {
            switch (boxType)
            {
                case AlertBoxType.Success:
                    return "alert alert-success";
                case AlertBoxType.Info:
                    return "alert alert-info";
                case AlertBoxType.Warning:
                    return "alert alert-warning";
                case AlertBoxType.Danger:
                    return "alert alert-danger";
                default:
                    return "alert alert-default";
            }
        }

        public static string ToIcon(this AlertBoxType boxType)
        {
            switch (boxType)
            {
                case AlertBoxType.Success:
                    return "glyphicon glyphicon-ok";
                case AlertBoxType.Info:
                    return "glyphicon glyphicon-info-sign";
                case AlertBoxType.Warning:
                    return "glyphicon glyphicon-warning-sign";
                case AlertBoxType.Danger:
                    return "glyphicon glyphicon-exclamation-sign";
                default:
                    return "glyphicon glyphicon-info-sign";
            }
        }

        public static MvcHtmlString BuildAlertBox(AlertBoxType boxType, string message, bool marginTop = true)
        {
            TagBuilder tagBuilder = new TagBuilder("div");
            TagBuilder iconBuilder = new TagBuilder("i");

            tagBuilder.AddCssClass(boxType.ToCss());
            if (marginTop)
                tagBuilder.AddCssClass("margin-top-10");
            
            iconBuilder.AddCssClass(boxType.ToIcon());
            tagBuilder.InnerHtml = $"{iconBuilder.ToString()} {message}";

            return new MvcHtmlString(tagBuilder.ToString());
        }

        public static MvcHtmlString GetUiAlertBox(this HtmlHelper helper, object viewBag)
        {
            if (((dynamic)viewBag).OperationResult != null)
            {
                OperationResult result = ((dynamic)viewBag).OperationResult as OperationResult;

                if (result.IsSuccess)
                    return BuildAlertBox(AlertBoxType.Success, result.UIMessage);
                else
                    return BuildAlertBox(AlertBoxType.Danger, result.UIMessage);
            }

            return MvcHtmlString.Create(string.Empty);
        }

        public static MvcHtmlString BuildLink(
            this HtmlHelper helper,
            string controller, 
            object modelId, 
            string actionName, 
            string text, 
            ButtonType buttonType = ButtonType.Default, 
            ButtonSize buttonSize = ButtonSize.Normal)
        {
            TagBuilder tagBuilder = new TagBuilder("a");

            tagBuilder.AddCssClass(buttonType.ToCss());
            tagBuilder.AddCssClass(buttonSize.ToCss());

            tagBuilder.Attributes.Add("href", $"/{controller}/{modelId}/{actionName}");
            tagBuilder.SetInnerText(text);

            return new MvcHtmlString(tagBuilder.ToString());
        }
        #endregion
    }
}