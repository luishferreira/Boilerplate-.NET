using Boilerplate.Domain.Entities;

namespace Boilerplate.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<bool> UsernameExistsAsync(string username);
    }
}
