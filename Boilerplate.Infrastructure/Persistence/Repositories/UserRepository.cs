using Boilerplate.Domain.Entities;
using Boilerplate.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Boilerplate.Infrastructure.Persistence.Repositories
{
    public class UserRepository(AppDbContext context) : IUserRepository
    {
        public async Task<User?> GetById(int id)
        {
            return await context.Usuarios.FindAsync(id);
        }

        public async Task Add(User user)
        {
            await context.Usuarios.AddAsync(user);
        }

        public async Task<bool> UsernameExists(string username)
        {
            return await context.Usuarios.AnyAsync(u => u.Username == username);
        }
    }
}
