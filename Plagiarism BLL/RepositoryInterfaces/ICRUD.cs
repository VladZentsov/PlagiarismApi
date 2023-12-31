﻿using Plagiarism_BLL.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.RepositoryInterfaces
{
    public interface ICRUD<T> where T : BaseModel
    {
        public Task CreateAsync(T model);
        public Task<T> GetByIdAsync(Guid id);
        public Task<List<T>> GetAllAsync();
        public Task<T> UpdateAsync(T model);
        public Task DeleteAsync(Guid id);
    }
}
