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

            CreateMap<UserResult, User>()
                .ReverseMap();

            CreateMap<CompareTwoWorksResult, CompareResult>()
                .ReverseMap();

            CreateMap<User, User>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<Work, Work>();

            CreateMap<WorkInfo, WorkInfo>()
                .ForMember(d => d.Id, opts => opts.Ignore())
                .ForMember(d => d.WorkId, opts => opts.Ignore())
                .ForMember(d => d.UserId, opts => opts.Ignore())
                .ForMember(d => d.User, opts => opts.Ignore())
                .ForMember(d => d.Work, opts => opts.Ignore())
                .ForMember(dest => dest.WorkName, opt => opt.MapFrom(src => src.WorkName))
                .ForMember(dest => dest.WorkType, opt => opt.MapFrom(src => src.WorkType))
                .ReverseMap();
        }
    }
}
