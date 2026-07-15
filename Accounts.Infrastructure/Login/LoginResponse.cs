using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Application.Enumerations;

namespace Accounts.Infrastructure.Login
{
    internal class LoginResponse
    {

        public string User { get; set; }

        public string Password { get; set; }

        public ProgramName Program { get; set; }

    }
}
