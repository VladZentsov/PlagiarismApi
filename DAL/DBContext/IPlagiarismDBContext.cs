using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DBContext
{
    public interface IPlagiarismDBContext
    {
        DbSet<T> Set<T>() where T : class;
        public DatabaseFacade Database { get; }
        int SaveChanges();
    }
}
