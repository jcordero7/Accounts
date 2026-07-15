
using Accounts.Application.Login;
using Accounts.Application.Login.Interfaces;
using Accounts.Application.User;
using Accounts.Application.User.Interfaces;
using aplication.Infrastructure.Common;
using AutoMapper;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Accounts.Application.CaseUses
{
    public class LoginCaseUse : ILoginCaseUse
    {
       
        private readonly ILoginRepository _loginRepository;
        private readonly IMapper _mapper;
      //  private HttpContext _context;


        public LoginCaseUse(
            //  HttpContext context,
            ILoginRepository loginRepository, 
            IMapper mapper
        )
        {
            this._loginRepository = loginRepository;
            this._mapper = mapper;
        }

       

        //public PlatformDto Get(int idProf, string token)
        //{
        //    throw new NotImplementedException();
        //}

        ////IMPLMENTAR ASINCRONO
        //public List<PlatformDto> Gets(string token)
        //{
        //    //throw new NotImplementedException();
        //    List<PlatformDto> platformDtos = new List<PlatformDto>();

        //    var resultPlatform = _platformService.Gets(token);

        //    if (resultPlatform == null)
        //    {
        //        return null;
        //    }

        //    platformDtos = _mapper.Map<List<PlatformDto>>(resultPlatform);

        //    //IMPLEMNTAR ASINCRONO SIPI
        //    //traer plataformas

        //    return platformDtos;
        //}


        public ApiResponse<string> Login(UserLogin userLogin)
        {

            // Accounts.Infrastructure.User.User user = new Infrastructure.User.User();

            //   user = _mapper.Map<Accounts.Infrastructure.User.User>(user);

            //string token = _userService.Create(userLogin);

            var result = _loginRepository.Login(userLogin);

            //if (token == "")
            //{
            //    return null;
            //}

            return result;
        }


        public bool RequestToken(string email)
        {
            var result = _loginRepository.RequestToken(email);

            return result;
        }


        public bool EnabledAccount(UserEnabledAccount userEnabled)
        {

            var result = _loginRepository.EnabledAccount(userEnabled);

            return result;

        }

        public bool ConfirmAccount(UserEnabledAccount userEnabled)
        {

            var result = _loginRepository.ConfirmAccount(userEnabled);

            return result;

        }

        public bool ForgottenPassword(string email)
        {

            var result = _loginRepository.ForgottenPassword(email);

            return result;

        }

        public bool ResetPassword(string token, string email)
        {

            var result = _loginRepository.ResetPassword(token, email);

            return result;

        }

        public bool ReenviarCodigo(string email, string token)
        {

            var result = _loginRepository.ResetPassword(email, token);

            return result;

        }


    }
}
