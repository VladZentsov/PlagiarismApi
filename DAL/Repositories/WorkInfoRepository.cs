using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class WorkInfoRepository : IWorkInfoRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        public readonly DbSet<WorkInfo> _workInfo;
        public WorkInfoRepository(IPlagiarismDBContext dbContext)
        {
            _dbContext = dbContext;
            _workInfo = dbContext.Set<WorkInfo>();
        }

        public async Task CreateAsync(WorkInfo model)
        {
            _workInfo.Add(model);
        }

        public async Task<WorkInfo> GetByIdAsync(Guid id)
        {
            return await _workInfo.FindAsync(id);
        }

        public async Task<WorkInfo> GetByWorkIdWithDetailsAsync(Guid workId)
        {
            return await _workInfo
                .Include(wi => wi.Work)
                .Include(wi => wi.User)
                .FirstOrDefaultAsync(wi => wi.WorkId == workId);

        }

        public async Task<List<WorkInfo>> GetAllAsync()
        {
            return await _workInfo.ToListAsync();
        }

        public async Task<List<WorkInfo>> GetAllWorkWithDetailsAsync()
        {
            return await _workInfo
                .Include(wi => wi.Work)
                .Include(wi => wi.User)
                .ToListAsync();

        }

        public async Task<WorkInfo> UpdateAsync(WorkInfo model)
        {
            var workInfoToUpdate = await _workInfo.FindAsync(model.Id);
            //if (workInfoToUpdate != null)
            //{
            //    // Обновление свойств информации о работе
            //    workInfoToUpdate.Title = model.Title;
            //    // Другие свойства...

            //    await _dbContext.SaveChangesAsync();
            //    return workInfoToUpdate;
            //}

            return null; // Если информация о работе не найдена
        }

        public async Task DeleteAsync(Guid id)
        {
            var workInfoToDelete = await _workInfo.FindAsync(id);
            if (workInfoToDelete != null)
            {
                _workInfo.Remove(workInfoToDelete);
            }
        }
    }
}