using Autenticador.Domain.Entities;

namespace Autenticador.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetById(int id);
        Task Add(User user);
        Task<bool> UsernameExists(string username);
    }
}
