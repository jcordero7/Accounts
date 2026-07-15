using System;
using System.Collections.Generic;
using System.Text;
using Accounts.Application.Enumerations;

namespace Accounts.Infrastructure.User
{
    internal class UserResponse
    {

        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Names { get; set; }

        public string SurNames { get; set; }


        //luego se toman en cuenta
        //******************************************
        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }
        //******************************************

        public ProgramName Program { get; set; }

    }
}
