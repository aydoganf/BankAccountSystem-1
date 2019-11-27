using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AccountApp.Models.Operation
{
    public class OperationResult
    {
        public string ResultCode { get; set; }
        public string ResultMessage { get; set; }
        public bool IsSuccess { get; set; }

        public string UIMessage => !IsSuccess ? $"Error Code: {ResultCode} - Error: {ResultMessage}" : $"{ResultMessage}";
    }
}