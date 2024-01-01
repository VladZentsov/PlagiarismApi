using AutoMapper;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using PlagiarismApi;
using PlagiarismTest.Helpers;

namespace PlagiarismTest.DALTests
{
    public class DALMockHelper
    {
        public static PlagiarismDBContext DBContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<PlagiarismDBContext>()
                    .UseInMemoryDatabase(Guid.NewGuid().ToString())
                    .Options;

                var context = new PlagiarismDBContext(options);

                context.Users.AddRange(ModelHelper.GenerateUsersData());
                context.Works.AddRange(ModelHelper.GenerateWorksData());
                context.WorkInfos.AddRange(ModelHelper.GenerateWorkInfosData());

                context.SaveChanges();

                return context;
            }
        }

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
