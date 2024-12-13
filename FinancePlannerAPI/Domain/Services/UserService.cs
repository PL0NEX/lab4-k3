using FinancePlannerAPI.Domain.Entities;
using FinancePlannerAPI.Domain.Interfaces;

namespace FinancePlannerAPI.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<User?> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task CreateUserAsync(string login, string passHash, string email)
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Login = login,
                PassHash = passHash,
                Email = email
            };

            await _userRepository.AddAsync(user);
        }

        public async Task UpdateUserAsync(Guid id, string login, string passHash, string email)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new Exception("Пользователь не найден");
            }

            user.Login = login;
            user.PassHash = passHash;
            user.Email = email;

            await _userRepository.UpdateAsync(user);
        }

        public async Task DeleteUserAsync(Guid id)
        {
            await _userRepository.DeleteAsync(id);
        }
    }
}
