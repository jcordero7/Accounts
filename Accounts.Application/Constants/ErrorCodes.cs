using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Application.Constants
{
  
    public static class ErrorCodes
    {
        // Errores de negocio (esperados)
        public const string EVENT_ALREADY_PASSED = "EVENT_ALREADY_PASSED";
        public const string DUPLICATE_EVENT = "DUPLICATE_EVENT";
        public const string INVALID_INPUT = "INVALID_INPUT";

        // Errores técnicos controlables
        public const string NETWORK_ERROR = "NETWORK_ERROR";
        public const string TIMEOUT = "TIMEOUT";
        public const string API_ERROR = "API_ERROR";

        // Errores críticos
        public const string INTERNAL_ERROR = "INTERNAL_ERROR";
        public const string UNEXPECTED_ERROR = "UNEXPECTED_ERROR";
    }


    public enum ErrorType
    {
        None,
        Business,
        Technical,
        Critical
    }

}
