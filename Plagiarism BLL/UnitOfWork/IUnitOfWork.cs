using Plagiarism_BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IUserRepository UserRepository { get;}
        public IWorkInfoRepository WorkInfoRepository { get;}
        public IWorkRepository WorkRepository { get;}
        public Task SaveAsync();
    }
}
