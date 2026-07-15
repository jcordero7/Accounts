using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Application.Common
{
    public class ErrorResponse
    {
        public string Code { get; set; }
        public string Message { get; set; }

        public ErrorResponse(string code, string message)
        {
            Code = code;
            Message = message;
        }

    }
}

