using AutoMapper;
using DAL.DBContext;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Enums;
using Plagiarism_BLL.RepositoryInterfaces;
using PlagiarismApi;
using PlagiarismTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace PlagiarismTest.DALTests
{
    public class DbContextMock
    {
        public static PlagiarismDBContext DBContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<PlagiarismDBContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;
                var context = new PlagiarismDBContext(options);
                context.Users.AddRange(ModelHelper.GenerateUsersData());
                context.SaveChanges();
                return context;
            }
        }
    }
}
