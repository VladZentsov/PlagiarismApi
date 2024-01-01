using AutoMapper;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Exceptions;
using Plagiarism_BLL.RepositoryInterfaces;


namespace DAL.Repositories
{
    public class WorkInfoRepository : IWorkInfoRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        private readonly DbSet<WorkInfo> _workInfo;
        private readonly IMapper _mapper;
        public WorkInfoRepository(IPlagiarismDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _workInfo = dbContext.Set<WorkInfo>();
            _mapper = mapper;
        }

        public async Task CreateAsync(WorkInfo model)
        {
            _workInfo.Add(model);
        }

        public async Task<WorkInfo> GetByIdAsync(Guid id)
        {
            var workInfo = await _workInfo.FindAsync(id);
            if(workInfo != null)
            {
                return workInfo;
            }
            else
                throw new PlagiarismException($"WorkInfo with identifier {id} not found.");
        }

        public async Task<WorkInfo> GetByWorkIdWithDetailsAsync(Guid workId)
        {
            var workInfo = await _workInfo
                .Include(wi => wi.Work)
                .Include(wi => wi.User)
                .FirstOrDefaultAsync(wi => wi.WorkId == workId);

            if( workInfo != null )
            {
                return workInfo;
            }
            else
                throw new PlagiarismException($"WorkInfo with identifier {workId} not found.");

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
            if (workInfoToUpdate != null)
            {
                workInfoToUpdate.WorkType = model.WorkType;
                workInfoToUpdate.WorkName = model.WorkName;
                return workInfoToUpdate;
            }
            throw new PlagiarismException($"WorkInfo with identifier {model.Id} not found.");
        }

        public async Task DeleteAsync(Guid id)
        {
            var workInfoToDelete = await _workInfo.FindAsync(id);
            if (workInfoToDelete != null)
            {
                _workInfo.Remove(workInfoToDelete);
            }
            else
                throw new PlagiarismException($"WorkInfo with identifier {id} not found.");
        }
    }
}