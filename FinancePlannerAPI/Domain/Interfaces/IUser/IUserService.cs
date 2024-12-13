using FinancePlannerAPI.Domain.Entities;

namespace FinancePlannerAPI.Domain.Interfaces
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(Guid id);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task CreateUserAsync(string login, string passHash, string email);
        Task UpdateUserAsync(Guid id, string login, string passHash, string email);
        Task DeleteUserAsync(Guid id);
    }
}
