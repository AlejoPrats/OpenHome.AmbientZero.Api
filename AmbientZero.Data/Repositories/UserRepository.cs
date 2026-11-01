using AmbientZero.Data.Constants;
using AmbientZero.Data.Entities;
using AmbientZero.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public int UpdatePassword(string username, string password)
        {
            return _ambientZeroContext.Users
                .Where(x => x.Username == username)
                .ExecuteUpdate(x => x.SetProperty(s => s.Password, password));
        }

        public int ActivateUser(string username)
        {
            return _ambientZeroContext.Users
                .Where(x => x.Username == username)
                .ExecuteUpdate(x => x.SetProperty(s => s.IsActive, true));
        }
    }
}
