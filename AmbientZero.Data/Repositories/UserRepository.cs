using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;

namespace AmbientZero.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AmbientZeroContext _ambientZeroContext;

        public UserRepository(AmbientZeroContext ambientZeroContext)
        {
            _ambientZeroContext = ambientZeroContext ?? throw new ArgumentNullException(nameof(ambientZeroContext));
        }

        public User? GetUserByUsername(string username)
        { 
            return _ambientZeroContext.Users.FirstOrDefault(x => x.Username == username && x.IsActive);
        }
    }
}
