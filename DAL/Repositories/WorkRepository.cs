using AutoMapper;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Exceptions;
using Plagiarism_BLL.RepositoryInterfaces;

namespace DAL.Repositories
{
    public class WorkRepository: IWorkRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        private readonly DbSet<Work> _work;
        private readonly IMapper _mapper;
        public WorkRepository(IPlagiarismDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _work = dbContext.Set<Work>();
            _mapper = mapper;
        }

        public async Task CreateAsync(Work model)
        {
            _work.Add(model);
        }

        public async Task<Work> GetByIdAsync(Guid id)
        {
            var work = await _work.FindAsync(id);
            if (work != null)
            {
                return work;
            }
            else
                throw new PlagiarismException($"Work with identifier {id} not found.");
        }

        public async Task<List<Work>> GetAllAsync()
        {
            return await _work.ToListAsync();
        }

        public async Task<Work> UpdateAsync(Work model)
        {
            var workToUpdate = await _work.FindAsync(model.Id);

            if (workToUpdate != null)
            {
                _mapper.Map(model, workToUpdate);
                return workToUpdate;
            }
            else
                throw new PlagiarismException($"Work with identifier {model.Id} not found.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var workToDelete = await _work.FindAsync(id);
            if (workToDelete != null)
            {
                _work.Remove(workToDelete);
            }
            else
                throw new PlagiarismException($"Work with identifier {id} not found.");
        }
    }
}
