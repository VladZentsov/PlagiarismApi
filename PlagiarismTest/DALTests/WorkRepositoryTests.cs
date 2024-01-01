using DAL.DBContext;
using DAL.Repositories;
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
    public class WorkRepositoryTests
    {
        private IWorkRepository _workRepository;
        private PlagiarismDBContext _plagiarismDBContext;
        [SetUp]
        public void Setup()
        {
            _plagiarismDBContext = DALMockHelper.DBContext;
            var mapper = MapperHelper.CreateMapper();
            _workRepository = new WorkRepository(_plagiarismDBContext, mapper);
        }

        [Test]
        public async Task GetAllWorksAsync()
        {
            //Arrange

            //Act
            IList<Work> lstData = await _workRepository.GetAllAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(lstData, Is.Not.Null);
                Assert.That(lstData.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task CreateWork()
        {
            //Arrange
            var workId = Guid.NewGuid();
            var work = new Work()
            {
                Id = workId,
                Code = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Security\r\n{\r\n    public class PasswordsUtil\r\n    {\r\n        public static string HashPassword(string password)\r\n        {\r\n            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);\r\n            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);\r\n            return hashedPassword;\r\n        }\r\n\r\n        public static bool VerifyPassword(string password, string hashedPassword)\r\n        {\r\n            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);\r\n        }\r\n    }\r\n}\r\n"
            };

            //Act
            await _workRepository.CreateAsync(work);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.Works.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteWork()
        {
            //Arrange
            var workToDelete = _plagiarismDBContext.Works.FirstOrDefault();
            //Act
            await _workRepository.DeleteAsync(workToDelete.Id);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.Works.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateWork()
        {
            //Arrange
            var workId = _plagiarismDBContext.Works.FirstOrDefault().Id;
            var workToUpdate = new Work()
            {
                Id = workId,
                Code = "using System;\r\nusing System.Collections.Generic;\r\nusing System.Linq;\r\nusing System.Text;\r\nusing System.Threading.Tasks;\r\n\r\nnamespace Plagiarism_BLL.Security\r\n{\r\n    public class PasswordsUtil\r\n    {\r\n        public static string HashPassword(string password)\r\n        {\r\n            string salt = BCrypt.Net.BCrypt.GenerateSalt(12);\r\n            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);\r\n            return hashedPassword;\r\n        }\r\n\r\n        public static bool VerifyPassword(string password, string hashedPassword)\r\n        {\r\n            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);\r\n        }\r\n    }\r\n}\r\n"
            };

            //Act
            await _workRepository.UpdateAsync(workToUpdate);
            await _plagiarismDBContext.SaveChangesAsync();

            var workAfterUpdate = _plagiarismDBContext.Works.FirstOrDefault(u => u.Id == workId);

            //Assert
            Comparator.PropertyValuesAreEquals(workToUpdate, workAfterUpdate);
        }

        [Test]
        public async Task WorkGetById()
        {
            //Arrange
            var expectedWork = ModelHelper.GenerateWorksData()[0];

            //Act
            var actual = await _workRepository.GetByIdAsync(expectedWork.Id);

            //Assert
            Comparator.PropertyValuesAreEquals(actual, expectedWork);
        }
    }
}
