﻿using HCM.Infrastructure.Entities;

namespace HCM.Infrastructure.Repositories
{
    public interface IUsersRepository
    {
        Task<User?> GetByUsernameAsync(string username);
    }
}
