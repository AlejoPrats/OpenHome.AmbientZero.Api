using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Interfaces
{
    /// <summary>
    /// Cross-cutting security utilities (hashing, token generation, etc).
    /// Stateless and infrastructure-level.
    /// </summary>
    public interface ISecurityService
    {
        /// <summary>
        /// Hashes the password using PBKDF2 through Microsoft's default Identity hasher.
        /// </summary>
        /// <param name="username">
        /// Required only because the underlying Microsoft API enforces a non-null user parameter.
        /// The value does not participate in the hashing process. This exists for historical
        /// reasons and potential future extensibility in custom hashers.
        /// </param>
        /// <param name="password">Plain-text password to hash.</param>
        /// <returns>The hashed password string.</returns>
        string? HashPassword(string username, string password);
    }
}
