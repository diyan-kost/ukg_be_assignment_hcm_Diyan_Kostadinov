using HCM.Core.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HCM.Core.Services
{
    public interface IUsersService
    {
        Task LoginAsync(LoginUserModel loginUserModel);

        Task LogoutAsync();
    }
}
