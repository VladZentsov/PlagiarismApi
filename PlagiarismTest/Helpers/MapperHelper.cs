using AutoMapper;
using PlagiarismApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlagiarismTest.Helpers
{
    public class MapperHelper
    {
        public static IMapper CreateMapper()
        {
            var mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new AutomapperProfile());
                cfg.AddProfile(new Plagiarism_BLL.AutomapperProfile());

            }).CreateMapper();

            return mapper;
        }
    }
}
