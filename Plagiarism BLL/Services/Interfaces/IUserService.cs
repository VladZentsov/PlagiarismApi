using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Services.Interfaces
{
    public interface IUserService
    {
        public Task<UserResult> CreateAccount(UserDto userDto, string password);
        public Task<UserResult> GetAccountInfo(Guid userId);
        public Task<UserResult> ValidateUser(string email, string pass);
    }
}
