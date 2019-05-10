// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PasswordService.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
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