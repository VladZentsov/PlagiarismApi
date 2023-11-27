using AutoMapper;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserDto, User>()
                .ReverseMap();
        }
    }
}
