using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Accounts.Application.Dtos;
using aplication.Infrastructure.Common;

namespace Accounts.Application.User
{
    public interface IUserCaseUse
    {

        //  public Task

        //CountryDto Get(int id, string token);

        //List<CountryDto> Gets(string nameSearch, string token);

        ApiResponse<UserDto> Get(int userId, string token);

        ApiResponse<UserDto> Add(UserDto userDto, string token);

        ApiResponse<bool> Edit(UserDto userDto, string token);

        bool Delete(int userId, string token);

        ApiResponse<bool> ChangePassword(ChangePasswordDto changePasswordDto, string token);

    }
}


