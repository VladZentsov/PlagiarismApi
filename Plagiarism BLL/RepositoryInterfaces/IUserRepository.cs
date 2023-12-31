﻿using Plagiarism_BLL.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.RepositoryInterfaces
{
    public interface IUserRepository: ICRUD<User>
    {
        public Task<User> GetByEmailAsync(string email);
    }
}
