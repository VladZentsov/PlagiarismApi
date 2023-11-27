using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Enums;
using Plagiarism_BLL.Services.Interfaces;
using Plagiarism_BLL.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plagiarism_BLL.Services
{
    public class WorkService: IWorkService
    {
        private readonly IUnitOfWork _unitOfWork;
        public WorkService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task UploadWork(Guid userId, string workName, WorkType workType, byte[] data)
        {
            Work work = new Work()
            {
                Id = Guid.NewGuid(),
                Data = data,
            };
            await _unitOfWork.WorkRepository.CreateAsync(work);

            WorkInfo workInfo = new WorkInfo()
            {
                WorkName = workName,
                UserId = userId,
                WorkId = work.Id,
                WorkType = workType,
            };

            await _unitOfWork.WorkInfoRepository.CreateAsync(workInfo);

            await _unitOfWork.SaveAsync();
        }
    }
}
