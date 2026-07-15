using Accounts.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Utilities
{
    public static class UGeneral
    {

        public static (string, string) Message(string pCode)
        {
            int codeError = 0;
            bool band;
            string code;
            string message;

            band = int.TryParse(pCode, out codeError);
            code = band ? codeError.ToString() : ErrorCodes.defaultCode;
            message = ErrorCodes.GetMessage(code);

            return (code, message);

        }

    }
}
