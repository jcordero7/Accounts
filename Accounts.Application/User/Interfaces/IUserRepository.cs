
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Accounts.Application.Dtos;
using aplication.Infrastructure.Common;
using UserEntity = Accounts.Application.User.User;

namespace Accounts.Application.User.Interfaces
{
    public interface IUserRepository
    {

        ApiResponse<UserEntity> Get(int userId, string token);

        List<User> Gets(string nameSearch, string token);

        ApiResponse<User> Create(User user, string token);

        ApiResponse<bool> Edit(User user, string token);

        ApiResponse<bool> ChangePassword(ChangePasswordDto changePasswordDto, string token);

    }
}
