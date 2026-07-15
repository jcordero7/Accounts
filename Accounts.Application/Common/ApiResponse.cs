using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Application.Common;
using Accounts.Application.Constants;


namespace aplication.Infrastructure.Common
{
    public class ApiResponse<T>
    {
        public bool Success { get; set; }
        public string Code { get; set; }  // opcional: "OK", "VALIDATION_ERROR", etc.
        public string Message { get; set; } // opcional, para feedback
        public T Data { get; set; }
        public ErrorResponse Error { get; set; }

        public ErrorType TipoError { get; set; }

        public int StatusCode { get; set; }
    }
}


