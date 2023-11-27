using AutoMapper;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.DTOs;
using Plagiarism_BLL.Enums;
using Plagiarism_BLL.Security;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Services
{
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<UserDto> CreateAccount(UserDto userDto, string password)
        {
            userDto.Id = Guid.NewGuid();
            User user = _mapper.Map<User>(userDto);
            user.PassHash = PasswordsUtil.HashPassword(password);

            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.SaveAsync();
            return userDto;
        }

        public async Task<UserDto> GetAccountInfo(Guid userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
        public async Task<UserDto> ValidateUser(string email, string pass)
        {
            var user = await _unitOfWork.UserRepository.GetByEmailAsync(email);
            if (!PasswordsUtil.VerifyPassword(pass, user.PassHash))
                throw new Exception();

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }
    }
}
