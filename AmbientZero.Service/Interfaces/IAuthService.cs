namespace AmbientZero.Service.Interfaces
{
    public interface IAuthService
    {
        /// <summary>
        /// Validates username and password and generates token for autorization
        /// </summary>
        /// <param name="username">username to validate</param>
        /// <param name="password">password of the user</param>
        /// <returns>Token encrypted with the secret defined in the appsettings</returns>
        string? AuthUser(string username, string password);
    }
}
