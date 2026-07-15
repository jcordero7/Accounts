using Accounts.Application.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Accounts.Models.ViewModels
{
    public class ChangePasswordViewModel
    {

        //AGREGAR VALIDACIONES CON DATAANOTATIONS U OTRA LIBRERIA
        public int Id { get; set; }
        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }

    }

}
