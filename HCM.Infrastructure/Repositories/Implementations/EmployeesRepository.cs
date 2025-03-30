﻿using HCM.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace HCM.Infrastructure.Repositories.Implementations
{
    public class EmployeesRepository : IEmployeesRepository
    {
        private readonly HCMContext _dbContext;

        public EmployeesRepository(HCMContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = _dbContext.Employees
                .Include(e => e.Manager)
                .AsNoTracking();

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id, bool asNoTracking = true) 
        {
            Employee? employee = null;

            if (asNoTracking)
            {
                employee = await _dbContext.Employees
                    .Include(e => e.Manager)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            else
            {
                employee = await _dbContext.Employees
                    .Include(e => e.Manager)
                    .FirstOrDefaultAsync(e => e.Id == id);
            }

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetByManagerIdAsync(int managerId)
        {
            var employees = _dbContext.Employees
                .AsNoTracking()
                .Where(e => e.ManagerId == managerId);

            return employees;
        }

        public async Task<bool> ExistsByPhoneNumber(string phoneNumber)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.PhoneNumber == phoneNumber);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.Email == email);
        }

        public async Task<bool> ExistsByNationalIdNumber(string nationalIdNumber)
        {
            return await _dbContext.Employees
                .AnyAsync(e => e.NationalIdNumber == nationalIdNumber);
        }

        public async Task SaveTrackingChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
