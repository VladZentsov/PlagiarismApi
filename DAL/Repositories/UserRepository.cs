using AutoMapper;
using DAL.DBContext;
using Microsoft.EntityFrameworkCore;
using Plagiarism_BLL.CoreModels;
using Plagiarism_BLL.Exceptions;
using Plagiarism_BLL.RepositoryInterfaces;

namespace DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        private readonly IMapper _mapper;
        private readonly DbSet<User> _users;
        public UserRepository(IPlagiarismDBContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _users = dbContext.Set<User>();
        }
        public async Task CreateAsync(User model)
        {
            _users.Add(model);
        }

        public async Task DeleteAsync(Guid id)
        {
            var userToDelete = await _users.FindAsync(id);
            if (userToDelete != null)
            {
                _users.Remove(userToDelete);
            }
            else
                throw new PlagiarismException($"User with identifier {id} not found.");
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            var user = await _users.FindAsync(id);
            if (user != null)
                return user;
            else
                throw new PlagiarismException($"User with identifier {id} not found.");
        }

        public async Task<User> UpdateAsync(User model)
        {
            var userToUpdate = await _users.FindAsync(model.Id);
            if (userToUpdate != null)
            {
                _mapper.Map(model, userToUpdate);
                return userToUpdate;
            }
            else
                throw new PlagiarismException($"User with identifier {model.Id} not found.");
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            var user = await _users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
                return user;
            else
                throw new PlagiarismException($"User with email {email} not found.");
        }
    }
}
