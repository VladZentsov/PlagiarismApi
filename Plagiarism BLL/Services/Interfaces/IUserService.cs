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
        public Task CreateAccount(UserDto userDto, string password);
        public Task<UserDto> GetAccountInfo(Guid userId);
    }
}
