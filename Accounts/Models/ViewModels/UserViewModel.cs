using Accounts.Application.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models.ViewModels
{
    public class UserViewModel
    {

        //AGREGAR VALIDACIONES CON DATAANOTATIONS U OTRA LIBRERIA
        public int Id { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

      //  public string UserName { get; set; }

        public string Names { get; set; }

        public string SurNames { get; set; }

        //luego se toman en cuenta
        //******************************************
        public DateTime BirthDate { get; set; }

        public string Phone { get; set; }
        //******************************************

        public ProgramName Program { get; set; }

       // public UserState State { get; set; }

        public IEnumerable<UserViewModel> GetUsers()
        {
            return new List<UserViewModel>() { new UserViewModel { Id = 101,  Names = "Anet", Email = "anet@test.com", Password = "anet123" } };
        }
    }

}
