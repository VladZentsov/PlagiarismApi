using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBContext
{
    public class PlagiarismDBContext: DbContext, IPlagiarismDBContext
    {
        public PlagiarismDBContext(DbContextOptions<PlagiarismDBContext> options) : base(options)
        { }

        public DbSet<User> Users { get; set; }
        public DbSet<Work> Works { get; set; }
        public DbSet<WorkInfo> WorkInfos { get; set; }


    }
}
