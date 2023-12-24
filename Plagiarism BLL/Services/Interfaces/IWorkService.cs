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
        public Task<FullWorkResult> UploadWork(Guid userId, string workName, WorkType workType, string code);
        public Task<CompareTwoWorksResult> CompareWorks(Guid currentWorkId, Guid workToCompareId);
        public Task<CompareToAllWorksResult> CompareToAllWorks(Guid currentWorkId);
        public Task DeleteWork(Guid workId);
    }
}
