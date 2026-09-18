using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class SecurityService : ISecurityService
    {
        public SecurityService() { }

        public string? HashPassword(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            PasswordHasher<string> hasher = new PasswordHasher<string>();
            return hasher.HashPassword(username, password);
        }
    }
}
