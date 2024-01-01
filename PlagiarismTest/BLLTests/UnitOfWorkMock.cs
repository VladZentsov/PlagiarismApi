using DAL.DBContext;
using DAL.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Moq;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.RepositoryInterfaces;
using Plagiarism_BLL.UnitOfWork;
using PlagiarismTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismTest.BLLTests
{
    public class UnitOfWorkMock
    {
        public static IUnitOfWork UnitOfWork
        {
            get
            {
                var userList = ModelHelper.GenerateUsersData();
                var worksList = ModelHelper.GenerateWorksData();
                var usersInfosWithDetails = ModelHelper.GenetrateWorkInfosWithDetails();
                var userRepo = new Mock<IUserRepository>();
                userRepo.Setup(ur => ur.GetAllAsync()).ReturnsAsync(userList);
                userRepo.Setup(ur => ur.GetByIdAsync(userList[0].Id)).ReturnsAsync(userList[0]);
                userRepo.Setup(ur => ur.CreateAsync(It.IsAny<User>()))
                      .Returns(Task.CompletedTask);
                userRepo.Setup(ur => ur.GetByEmailAsync(userList[0].Email))
                      .ReturnsAsync(userList[0]);

                var workRepo = new Mock<IWorkRepository>();
                workRepo.Setup(ur => ur.CreateAsync(It.IsAny<Work>()))
                      .Returns(Task.CompletedTask);
                workRepo.Setup(ur => ur.GetByIdAsync(worksList[0].Id))
                      .ReturnsAsync(worksList[0]);
                workRepo.Setup(ur => ur.GetByIdAsync(worksList[1].Id))
                      .ReturnsAsync(worksList[1]);

                var workInfoRepo = new Mock<IWorkInfoRepository>();
                workInfoRepo.Setup(ur => ur.CreateAsync(It.IsAny<WorkInfo>()))
                      .Returns(Task.CompletedTask);
                workInfoRepo.Setup(ur => ur.GetAllWorkWithDetailsAsync())
                      .ReturnsAsync(usersInfosWithDetails);

                var unitOfWorkMock = new Mock<IUnitOfWork>();
                unitOfWorkMock.Setup(x => x.UserRepository).Returns(userRepo.Object);
                unitOfWorkMock.Setup(x => x.WorkRepository).Returns(workRepo.Object);
                unitOfWorkMock.Setup(x => x.WorkInfoRepository).Returns(workInfoRepo.Object);

                return unitOfWorkMock.Object;
            }
        }
    }
}
