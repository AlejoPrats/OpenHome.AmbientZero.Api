using AmbientZero.Data.Entities;

namespace AmbientZero.Data.Interfaces
{
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user filtering by username and if its active
        /// </summary>
        /// <param name="username">string that represents the user to find</param>
        /// <returns>Returns active user that matches the username</returns>
        User? GetUserByUsername(string username);
    }
}
