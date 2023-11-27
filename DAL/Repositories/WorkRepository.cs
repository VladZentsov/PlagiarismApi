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
    public class WorkRepository: IWorkRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        public readonly DbSet<Work> _work;
        public WorkRepository(IPlagiarismDBContext dbContext)
        {
            _dbContext = dbContext;
            _work = dbContext.Set<Work>();
        }

        public async Task CreateAsync(Work model)
        {
            _work.Add(model);
        }

        public async Task<Work> GetByIdAsync(Guid id)
        {
            return await _work.FindAsync(id);
        }

        public async Task<List<Work>> GetAllAsync()
        {
            return await _work.ToListAsync();
        }

        public async Task<Work> UpdateAsync(Work model)
        {
            var workToUpdate = await _work.FindAsync(model.Id);
            //if (workToUpdate != null)
            //{
            //    // Обновление свойств работы
            //    workToUpdate.Title = model.Title;
            //    // Другие свойства...

            //    await _dbContext.SaveChangesAsync();
            //    return workToUpdate;
            //}

            return null; // Если работа не найдена
        }

        public async Task DeleteAsync(Guid id)
        {
            var workToDelete = await _work.FindAsync(id);
            if (workToDelete != null)
            {
                _work.Remove(workToDelete);
            }
        }
    }
}
