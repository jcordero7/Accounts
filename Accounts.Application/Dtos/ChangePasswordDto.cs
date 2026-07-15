using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Application.Dtos
{
    public class ChangePasswordDto
    {
        public int Id { get; set; }

        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }
    }
}
