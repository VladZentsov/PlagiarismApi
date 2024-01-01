using AutoMapper;
using DAL.DBContext;
using DAL.Repositories;
using Microsoft.Extensions.Configuration;
using Plagiarism_BLL.RepositoryInterfaces;
using Plagiarism_BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.UnitOfWork
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly IPlagiarismDBContext _plagiarismDBContext;
        private readonly IMapper _automapperProfile;

        private IUserRepository _userRepository;
        private IWorkInfoRepository _workInfoRepository;
        private IWorkRepository _workRepository;

        public UnitOfWork(IPlagiarismDBContext plagiarismDBContext, IMapper automapperProfile)
        {
            _plagiarismDBContext = plagiarismDBContext;
            _automapperProfile = automapperProfile;
        }

        public Task SaveAsync()
        {
            _plagiarismDBContext.SaveChanges();
            return Task.CompletedTask;
        }

        public IUserRepository UserRepository
        {
            get
            {
                if (_userRepository == null)
                {
                    _userRepository = new UserRepository(_plagiarismDBContext, _automapperProfile);
                }
                return _userRepository;
            }
        }

        public IWorkInfoRepository WorkInfoRepository
        {
            get
            {
                if (_workInfoRepository == null)
                {
                    _workInfoRepository = new WorkInfoRepository(_plagiarismDBContext, _automapperProfile);
                }
                return _workInfoRepository;
            }
        }
        public IWorkRepository WorkRepository
        {
            get
            {
                if (_workRepository == null)
                {
                    _workRepository = new WorkRepository(_plagiarismDBContext, _automapperProfile);
                }
                return _workRepository;
            }
        }


    }
}
