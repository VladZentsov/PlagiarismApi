using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Services.Interfaces
{
    public interface IWorkService
    {
        public Task UploadWork(Guid userId, string workName, WorkType workType, string code);
        public Task<CompareWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId);
    }
}
