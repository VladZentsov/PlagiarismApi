using DAL.DBContext;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.RepositoryInterfaces;
using PlagiarismTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismTest.DALTests
{
    public class WorkInfoInfoRepoitoryTests
    {
        private IWorkInfoRepository _workInfoRepository;
        private PlagiarismDBContext _plagiarismDBContext;
        [SetUp]
        public void Setup()
        {
            _plagiarismDBContext = DALMockHelper.DBContext;
            var mapper = DALMockHelper.CreateMapper();
            _workInfoRepository = new WorkInfoRepository(_plagiarismDBContext, mapper);
        }

        [Test]
        public async Task GetAllWorkInfosAsync()
        {
            //Arrange

            //Act
            IList<WorkInfo> lstData = await _workInfoRepository.GetAllAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(lstData, Is.Not.Null);
                Assert.That(lstData.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task CreateWorkInfo()
        {
            //Arrange
            var workInfoId = Guid.NewGuid();
            var workInfo = new WorkInfo()
            {
                Id = workInfoId,
                UserId = new Guid("8ddfa4dd-d8f3-4f3a-bb78-0ded6d5e2305"),
                WorkId = new Guid("89e202f0-bda2-4713-beee-0c3125266658"),
                WorkName = "TestWorkName",
                WorkType = Plagiarism_BLL.Enums.WorkType.cs
            };

            //Act
            await _workInfoRepository.CreateAsync(workInfo);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.WorkInfos.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteWorkInfo()
        {
            //Arrange
            var workInfoToDelete = _plagiarismDBContext.WorkInfos.FirstOrDefault();
            //Act
            await _workInfoRepository.DeleteAsync(workInfoToDelete.Id);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.WorkInfos.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateWorkInfo()
        {
            //Arrange
            var firstWork = _plagiarismDBContext.WorkInfos.FirstOrDefault(); 
            var workInfoToUpdate = new WorkInfo()
            {
                Id = firstWork.Id,
                UserId = firstWork.UserId,
                WorkId = firstWork.WorkId,
                WorkName = "TestWorkName",
                WorkType = Plagiarism_BLL.Enums.WorkType.cs
            };

            //Act
            await _workInfoRepository.UpdateAsync(workInfoToUpdate);
            await _plagiarismDBContext.SaveChangesAsync();

            var workInfoAfterUpdate = _plagiarismDBContext.WorkInfos.FirstOrDefault(u => u.Id == firstWork.Id);

            //Assert
            Assert.AreEqual(workInfoToUpdate.WorkName, workInfoAfterUpdate.WorkName);
        }

        [Test]
        public async Task WorkInfoGetById()
        {
            //Arrange
            var expectedWorkInfo = _plagiarismDBContext.WorkInfos.FirstOrDefault();

            //Act
            var actual = await _workInfoRepository.GetByIdAsync(expectedWorkInfo.Id);

            //Assert
            Comparator.PropertyValuesAreEquals(actual, expectedWorkInfo);
        }
        [Test]
        public async Task WorkInfoGetByWorkIdWithDetails()
        {
            //Arrange
            var expectedWorkInfo = _plagiarismDBContext.WorkInfos
                .Include(w=>w.Work)
                .Include(w => w.User)
                .FirstOrDefault();

            //Act
            var actual = await _workInfoRepository.GetByWorkIdWithDetailsAsync(expectedWorkInfo.WorkId);

            //Assert
            Comparator.PropertyValuesAreEquals(actual, expectedWorkInfo);
        }

        [Test]
        public async Task WorkInfoGetAllWithDetails()
        {
            //Arrange
            var expectedWorkInfo = await _plagiarismDBContext.WorkInfos
                .Include(w => w.Work)
                .Include(w => w.User)
                .ToListAsync();

            //Act
            var actual = await _workInfoRepository.GetAllWorkWithDetailsAsync();

            //Assert
            Assert.AreEqual(expectedWorkInfo.Count(), actual.Count());
            for (int i = 0; i < expectedWorkInfo.Count(); i++)
            {
                Comparator.PropertyValuesAreEquals(actual[i], expectedWorkInfo[i]);
            }
        }
    }
}
