using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plagiarism_BLL.DTOs;
using Plagiarism_BLL.Enums;
using Plagiarism_BLL.Services.Interfaces;
using PlagiarismApi.Models;
using PlagiarismApi.Security;

namespace PlagiarismApi.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var userResult = await _userService.ValidateUser(loginModel.Email, loginModel.Password);

            if (userResult == null)
            {
                return Unauthorized();
            }

            userResult.JwtToken = JwtHelper.GenerateJwtToken(userResult);

            return Ok(userResult);
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] UserModel userModel)
        {
            var userDto = _mapper.Map<UserDto>(userModel);
            userDto.Role = Role.User;
            var userResult = await _userService.CreateAccount(userDto, userModel.Password);

            userResult.JwtToken = JwtHelper.GenerateJwtToken(userResult);
            return Ok(userResult);
        }
    }
}
