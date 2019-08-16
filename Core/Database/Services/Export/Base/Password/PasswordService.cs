// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordService.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Services
{
    using Microsoft.AspNetCore.Identity;

    public class PasswordService : IPasswordService
    {
        private readonly PasswordHasher<string> passwordHasher;

        public PasswordService()
        {
            this.passwordHasher = new PasswordHasher<string>();
        }

        public string HashPassword(string user, string password)
        {
            return this.passwordHasher.HashPassword(user, password);
        }

        public bool VerifyHashedPassword(string user, string hashedPassword, string providedPassword)
        {
            var result = this.passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
            return result != PasswordVerificationResult.Failed;
        }

        public void Dispose()
        {
        }
    }
}
