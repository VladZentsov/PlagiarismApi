using DAL.DBContext;
using DAL.Repositories;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.RepositoryInterfaces;
using PlagiarismTest.Helpers;

namespace PlagiarismTest.DALTests
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserRepository _userRepository;
        private PlagiarismDBContext _plagiarismDBContext;
        [SetUp]
        public void Setup()
        {
            _plagiarismDBContext = DALMockHelper.DBContext;
            var mapper = DALMockHelper.CreateMapper();
            _userRepository = new UserRepository(_plagiarismDBContext, mapper);
        }

        [Test]
        public async Task GetAllUsersAsync()
        {
            //Arrange

            //Act
            IList<User> lstData = await _userRepository.GetAllAsync();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.That(lstData, Is.Not.Null);
                Assert.That(lstData.Count, Is.EqualTo(2));
            });
        }

        [Test]
        public async Task CreateUser()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var user = new User()
            {
                Id = userId,
                Name = "TestName",
                Surname = "TestSurname",
                Email = "TestEmail",
                GroupName = "TestGroupName",
                Role = Plagiarism_BLL.Enums.Role.User,
                UniversityYear = 3,
                PassHash = "$2a$12$2jqPtyTyTNcFu5AGn.Ka5u/OkTRnnfWStZ2MAA9E1Jp4NG9TTyCtq",
            };

            //Act
            await _userRepository.CreateAsync(user);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.Users.Count(), Is.EqualTo(3));
        }

        [Test]
        public async Task DeleteUser()
        {
            //Arrange
            var userToDelete = _plagiarismDBContext.Users.FirstOrDefault();
            //Act
            await _userRepository.DeleteAsync(userToDelete.Id);
            await _plagiarismDBContext.SaveChangesAsync();

            //Assert
            Assert.That(_plagiarismDBContext.Users.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateUser()
        {
            //Arrange
            var userId = _plagiarismDBContext.Users.FirstOrDefault().Id;
            var userToUpdate = new User()
            {
                Id = userId,
                Name = "TestName",
                Surname = "TestSurname",
                Email = "TestEmail",
                GroupName = "TestGroupName",
                Role = Plagiarism_BLL.Enums.Role.User,
                UniversityYear = 3,
                PassHash = "$2a$12$2jqPtyTyTNcFu5AGn.Ka5u/OkTRnnfWStZ2MAA9E1Jp4NG9TTyCtq",
            };

            //Act
            await _userRepository.UpdateAsync(userToUpdate);
            await _plagiarismDBContext.SaveChangesAsync();

            var userAfterUpdate = _plagiarismDBContext.Users.FirstOrDefault(u => u.Id == userId);

            //Assert
            Comparator.PropertyValuesAreEquals(userToUpdate, userAfterUpdate);
        }

        [Test]
        public async Task UserGetById()
        {
            //Arrange
            var expectedUser = ModelHelper.GenerateUsersData()[0];

            //Act
            var actual = await _userRepository.GetByIdAsync(expectedUser.Id);

            //Assert
            Comparator.PropertyValuesAreEquals(actual, expectedUser);
        }

        [Test]
        public async Task UserGetByEmail()
        {
            //Arrange
            var expectedUser = ModelHelper.GenerateUsersData()[0];

            //Act
            var actual = await _userRepository.GetByEmailAsync(expectedUser.Email);

            //Assert
            Comparator.PropertyValuesAreEquals(actual, expectedUser);
        }


    }
}
