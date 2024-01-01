using AutoMapper;
using DAL.UnitOfWork;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.DTOs;
using Plagiarism_BLL.Services;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.UnitOfWork;
using PlagiarismTest.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismTest.BLLTests
{
    public class UserServiceTests
    {
        private IUnitOfWork _unitOfWork;
        private IUserService _userService;
        private IMapper _mapper;
        [SetUp]
        public void Setup()
        {
            _unitOfWork = UnitOfWorkMock.UnitOfWork;
            _mapper = MapperHelper.CreateMapper();
            _userService = new UserService(_unitOfWork, _mapper);
        }
        public static UserResult GenerateUserResult(User user)
        {
            return new UserResult()
            {
                Id = user.Id,
                Email = user.Email,
                GroupName = user.GroupName,
                JwtToken = null,
                Name = user.Name,
                Role = user.Role,
                Surname = user.Surname,
                UniversityYear = user.UniversityYear,
            };
        }

        [Test]
        public async Task GetAccountInfo()
        {
            //Arrange
            var user = ModelHelper.GenerateUsersData().FirstOrDefault();
            var expectedUserInfo = GenerateUserResult(user);
            //Act
            var actualUserInfo = await _userService.GetAccountInfo(user.Id);

            //Assert
            Comparator.PropertyValuesAreEquals(expectedUserInfo, actualUserInfo);
        }

        [Test]
        public async Task CreateAccount()
        {
            //Arrange
            Guid userId = Guid.NewGuid();
            var password = "12345xx";
            var user = ModelHelper.GenerateTestUser(userId);
            var expectedUserInfo = GenerateUserResult(user);
            var userDto = _mapper.Map<UserDto>(user);
            //Act
            var actualUserInfo = await _userService.CreateAccount(userDto, password);

            //Assert
            var idProperty = expectedUserInfo.GetType().GetProperty("Id");
            PropertyInfo[] expProps = new PropertyInfo[] { idProperty };
            Comparator.PropertyValuesAreEquals(expectedUserInfo, actualUserInfo, expProps);
        }

        [Test]
        public async Task ValidateUser()
        {
            //Arrange
            var user = ModelHelper.GenerateUsersData().FirstOrDefault();
            var expectedUserInfo = GenerateUserResult(user);
            //Act
            var actualUserInfo = await _userService.ValidateUser(user.Email, "7654321z");
            //Assert

            Comparator.PropertyValuesAreEquals(expectedUserInfo, actualUserInfo);
        }
    }
}
