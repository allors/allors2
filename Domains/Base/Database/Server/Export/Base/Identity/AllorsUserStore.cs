// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AllorsUserStore.cs" company="Allors bvba">
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

namespace Identity
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Allors.Services;

    using global::Allors;
    using global::Allors.Domain;
    using global::Allors.Meta;

    using Identity.Models;

    using Microsoft.AspNetCore.Identity;

    using Task = System.Threading.Tasks.Task;

    public class AllorsUserStore<TUser> : IUserStore<TUser>,
                                          IUserPasswordStore<TUser>
                                        //IUserLoginStore<TUser>
                                        //IUserClaimStore<TUser>,
                                        //IUserSecurityStampStore<TUser>,
                                        //IUserTwoFactorStore<TUser>,
                                        //IUserEmailStore<TUser>,
                                        //IUserLockoutStore<TUser>,
                                        //IUserPhoneNumberStore<TUser>
                                        where TUser : ApplicationUser
    {
        private readonly IDatabase database;

        public AllorsUserStore(IDatabaseService databaseService)
        {
            this.database = databaseService.Database;
        }

        #region IUserStore
        public void Dispose()
        {
        }

        public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.UserName;
        }

        public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
        }

        public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.NormalizedUserName;
        }

        public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
        }

        public async Task<IdentityResult> CreateAsync(TUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                try
                {
                    var lastname = identityUser.Email?.Split("@")?[0];

                    var user = new PersonBuilder(session)
                        .WithUserName(identityUser.UserName)
                        .WithNormalizedUserName(identityUser.NormalizedUserName)
                        .WithUserPasswordHash(identityUser.PasswordHash)
                        .WithUserEmail(identityUser.Email)
                        .WithUserEmailConfirmed(identityUser.EmailConfirmed)
                        .WithLastName(lastname)
                        .Build();

                    session.Derive();
                    session.Commit();

                    identityUser.Id = user.Id.ToString();

                    return IdentityResult.Success;
                }
                catch (Exception e)
                {
                    return IdentityResult.Failed(new IdentityError { Description = $"Could not create user {identityUser.UserName}." });
                }
            }
        }

        public async Task<IdentityResult> UpdateAsync(TUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                try
                {
                    var user = (User)session.Instantiate(identityUser.Id);

                    user.UserName = identityUser.UserName;
                    user.UserPasswordHash = identityUser.PasswordHash;
                    user.UserEmail = identityUser.Email;
                    user.UserEmailConfirmed = identityUser.EmailConfirmed;

                    session.Derive();
                    session.Commit();

                    return IdentityResult.Success;
                }
                catch
                {
                    return IdentityResult.Failed(new IdentityError { Description = $"Could not update user {identityUser.UserName}." });
                }
            }
        }

        public async Task<IdentityResult> DeleteAsync(TUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                try
                {
                    var user = (User)session.Instantiate(identityUser.Id);

                    if (user is Deletable)
                    {
                        ((Deletable)user).Delete();
                    }

                    session.Derive();
                    session.Commit();

                    return IdentityResult.Success;
                }
                catch
                {
                    return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {identityUser.UserName}." });
                }
            }
        }

        public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                var user = (User)session.Instantiate(userId);

                if (user != null)
                {
                    var identityUser = new ApplicationUser
                    {
                        Id = user.Id.ToString(),
                        UserName = user.UserName,
                        PasswordHash = user.UserPasswordHash,
                        Email = user.UserEmail,
                        EmailConfirmed = user.UserEmailConfirmed ?? false
                    };

                    return (TUser)identityUser;
                }

                return null;
            }
        }

        public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                var user = new Users(session).FindBy(M.User.NormalizedUserName, normalizedUserName);

                if (user != null)
                {
                    var identityUser = new ApplicationUser
                    {
                        Id = user.Id.ToString(),
                        UserName = user.UserName,
                        PasswordHash = user.UserPasswordHash,
                        Email = user.UserEmail,
                        EmailConfirmed = user.UserEmailConfirmed ?? false
                    };

                    return (TUser)identityUser;
                }

                return null;
            }
        }
        #endregion

        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return string.IsNullOrWhiteSpace(user.PasswordHash);
        }
        #endregion
    }
}
