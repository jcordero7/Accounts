using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using aplication.Infrastructure.Common;

namespace Accounts.Application.Login
{
    public interface ILoginCaseUse
    {

        //  public Task

        //CountryDto Get(int id, string token);

        //List<CountryDto> Gets(string nameSearch, string token);

        ApiResponse<string> Login(UserLogin userLogin);

        bool RequestToken(string email);

        bool EnabledAccount(UserEnabledAccount userEnabled);

        bool ConfirmAccount(UserEnabledAccount userEnabled);

        bool ForgottenPassword(string email);

        bool ResetPassword(string token, string email);

        bool ReenviarCodigo(string token, string email);

    }
}


