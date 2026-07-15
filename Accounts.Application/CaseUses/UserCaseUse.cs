
using Accounts.Application.Dtos;
using Accounts.Application.User;
using Accounts.Application.User.Interfaces;
using aplication.Infrastructure.Common;
using AutoMapper;
//using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using userEntity = Accounts.Application.User.User;

namespace Accounts.Application.CaseUses
{
    public class UserCaseUse : IUserCaseUse
    {
       
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
      //  private HttpContext _context;


        public UserCaseUse(
          //  HttpContext context,
            IUserRepository userRepository, 
            IMapper mapper
        )
        {
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        public ApiResponse<UserDto> Get(int userId, string token)
        {
            var user = _userRepository.Get(userId, token);
            return _mapper.Map<ApiResponse<UserDto>>(user);
           
        }

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


        public ApiResponse<UserDto> Add(UserDto userDto, string token)
        {
            var user = _mapper.Map<Application.User.User>(userDto);
            var result = _userRepository.Create(user, token);
            var userDtoResult = _mapper.Map<ApiResponse<UserDto>>(result);

            return userDtoResult;
        }

        public ApiResponse<bool> Edit(UserDto userDto, string token) {

            var user = _mapper.Map<Application.User.User>(userDto);
            var result = _userRepository.Edit(user, token);


            return result;

        }

        public bool Delete(int userId, string token)
        {
            throw new NotImplementedException();
        }

        public ApiResponse<bool> ChangePassword(ChangePasswordDto changePasswordDto, string token)
        {
            var result = _userRepository.ChangePassword(changePasswordDto, token);
            return result;
        }
    }
}
