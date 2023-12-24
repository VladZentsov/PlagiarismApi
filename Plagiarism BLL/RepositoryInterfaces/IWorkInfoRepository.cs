using Plagiarism_BLL.CoreModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.RepositoryInterfaces
{
    public interface IWorkInfoRepository:ICRUD<WorkInfo>
    {
        public Task<WorkInfo> GetByWorkIdWithDetailsAsync(Guid workId);
        public Task<List<WorkInfo>> GetAllWorkWithDetailsAsync();
    }
}
