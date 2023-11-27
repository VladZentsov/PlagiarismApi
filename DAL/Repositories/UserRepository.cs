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
    public class UserRepository : IUserRepository
    {
        private readonly IPlagiarismDBContext _dbContext;
        public readonly DbSet<User> _users;
        public UserRepository(IPlagiarismDBContext dbContext)
        {
            _dbContext = dbContext;
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
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _users.ToListAsync();
        }

        public async Task<User> GetByIdAsync(Guid id)
        {
            return await _users.FindAsync(id);
        }

        public async Task<User> UpdateAsync(User model)
        {
            var userToUpdate = await _users.FindAsync(model.Id);
            if (userToUpdate != null)
            {
                userToUpdate.Name = model.Name;
                return userToUpdate;
            }
            return null;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
