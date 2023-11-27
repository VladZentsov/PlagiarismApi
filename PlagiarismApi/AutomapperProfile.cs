using AutoMapper;
using Plagiarism_BLL.DTOs;
using PlagiarismApi.Models;

namespace PlagiarismApi
{
    public class AutomapperProfile: Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserModel, UserDto>()
                .ReverseMap();
        }
    }
}
