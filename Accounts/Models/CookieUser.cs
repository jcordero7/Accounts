using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models
{
    public class CookieUser
    {
        #region properties
        public int Id { get; set; }

        public string EmailUser { get; set; }

       // public string UserName { get; set; }

      //  public string Password { get; set; }

       // public string Name { get; set; }

        public string Token { get; set; }


        #endregion properties

        #region constructor

#pragma warning disable CS0169 // The field 'CookieUser.Context' is never used
        private HttpContext Context;
#pragma warning restore CS0169 // The field 'CookieUser.Context' is never used

       // public CookieUser(HttpContext context)
        public CookieUser()
        {
           // Context = context;
        }

        //public CookieUser(string userid, string password)
        //{
        //    Id = 1;
        //    if (string.IsNullOrEmpty(userid)) userid = "kkk";
        //    if (string.IsNullOrEmpty(password)) password = "kkk";


        //    Email = "jairo@dfds.com";
        //    UserName = "jai";
        //    Password = "dfd";
        //    Name = "Jairo Cordero";

        //}

        #endregion constructor

    }
}
