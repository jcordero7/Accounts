
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using aplication.Infrastructure.Common;

namespace Accounts.Application.Login.Interfaces
{
    public interface ILoginRepository
    {

        //User Get(int idCountry, string token);

        //List<User> Gets(string nameSearch, string token);

        ApiResponse<string> Login(UserLogin userLogin);

        bool RequestToken(string email);

        bool EnabledAccount(UserEnabledAccount userEnabled);

        bool ConfirmAccount(UserEnabledAccount userEnabled);

        bool ForgottenPassword(string email);

        bool ResetPassword(string token, string email);

    }
}


