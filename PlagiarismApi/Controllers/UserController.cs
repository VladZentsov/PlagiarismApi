using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Plagiarism_BLL.CoreModels;
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

        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetUserInfo()
        //{
        //    var userDto = _userService.GetAccountInfo()
        //}
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _userService.ValidateUser(loginModel.Email, loginModel.Password);

            if (user == null)
            {
                return Unauthorized();
            }

            var token = JwtHelper.GenerateJwtToken(user);
            return Ok(new { Token = token });
        }

        [HttpPost("CreateAccount")]
        public async Task<IActionResult> CreateAccount([FromBody] UserModel userModel)
        {
            var userDto = _mapper.Map<UserDto>(userModel);
            userDto.Role = Role.User;
            userDto = await _userService.CreateAccount(userDto, userModel.Password);

            var token = JwtHelper.GenerateJwtToken(userDto);
            return Ok(new { Token = token });
        }
    }
}
