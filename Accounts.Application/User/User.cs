
using Accounts.Application.Enumerations;
using System;
using System.Collections.Generic;
using System.Text;

namespace Accounts.Application.User
{
    public class User
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

        public string Token { get; set; }

        public ProgramName Program { get; set; }

        public UserState State { get; set; }

    }
}


