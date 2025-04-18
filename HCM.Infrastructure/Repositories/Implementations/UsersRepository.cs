﻿using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class UsersRepository : IUsersRepository
    {
        private readonly HCMContext _dbContext;

        public UsersRepository(HCMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.Username == username);

            return user;
        }

        public async Task<User?> GetByEmployeeIdAsync(int employeeId)
        {
            var user = await _dbContext.Users
                .Include(u => u.Role)
                .Include(u => u.Employee)
                .FirstOrDefaultAsync(u => u.EmployeeId == employeeId);

            return user;
        }

        public async Task<User> CreateUserAsync(User user)
        {
            await _dbContext.Users.AddAsync(user);

            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task DeleteAsync(User user)
        {
            _dbContext.Remove(user);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SaveTrackingChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
