// <copyright file="AllorsUserStore.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Security
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Threading;
    using System.Threading.Tasks;
    using Allors;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Services;
    using Microsoft.AspNetCore.Identity;
    using Claim = System.Security.Claims.Claim;
    using Task = System.Threading.Tasks.Task;

    public class AllorsUserStore : IUserPasswordStore<IdentityUser>,
                                   IUserLoginStore<IdentityUser>,
                                   IUserClaimStore<IdentityUser>,
                                   IUserSecurityStampStore<IdentityUser>
    // IUserTwoFactorStore<IdentityUser>,
    // IUserEmailStore<IdentityUser>,
    // IUserLockoutStore<IdentityUser>,
    // IUserPhoneNumberStore<IdentityUser>
    {
        private readonly IDatabase database;

        public AllorsUserStore(IDatabaseService databaseService) => this.database = databaseService.Database;

        #region IUserStore
        public void Dispose()
        {
        }

        public async Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.Id;
        }

        public async Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.UserName;
        }

        public async Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.UserName = userName;
        }

        public async Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.NormalizedUserName;
        }

        public async Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.NormalizedUserName = normalizedName;
        }

        public async Task<IdentityResult> CreateAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                try
                {
                    var user = new PersonBuilder(session)
                        .WithUserName(identityUser.UserName)
                        .WithUserPasswordHash(identityUser.PasswordHash)
                        .WithUserEmail(identityUser.Email)
                        .WithUserEmailConfirmed(identityUser.EmailConfirmed)
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

        public async Task<IdentityResult> UpdateAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                try
                {
                    var user = identityUser.User(session);

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

        public async Task<IdentityResult> DeleteAsync(IdentityUser identityUser, CancellationToken cancellationToken)
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

        public async Task<IdentityUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                var user = (User)session.Instantiate(userId);
                return user?.AsIdentityUser();
            }
        }

        public async Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                var user = new Users(session).FindBy(M.User.NormalizedUserName, normalizedUserName);
                return user?.AsIdentityUser();
            }
        }

        #endregion

        #region IUserPasswordStore
        public async Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            user.PasswordHash = passwordHash;
        }

        public async Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return user.PasswordHash;
        }

        public async Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return string.IsNullOrWhiteSpace(user.PasswordHash);
        }
        #endregion

        #region IUserLoginStore
        public async Task AddLoginAsync(IdentityUser identityUser, UserLoginInfo userLoginInfo, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = (User)session.Instantiate(identityUser.Id);

                var login = new LoginBuilder(session)
                    .WithProvider(userLoginInfo.LoginProvider)
                    .WithKey(userLoginInfo.ProviderKey)
                    .WithDisplayName(userLoginInfo.ProviderDisplayName)
                    .Build();

                user.AddLogin(login);

                session.Derive();
                session.Commit();
            }
        }

        public async Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var extent = new Logins(session).Extent();
                extent.Filter.AddEquals(M.Login.Provider, loginProvider);
                extent.Filter.AddEquals(M.Login.Key, providerKey);

                var user = extent.FirstOrDefault()?.UserWhereLogin;
                return user?.AsIdentityUser();
            }
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = (User)session.Instantiate(identityUser.Id);
                return user.Logins.Select(v => v.AsUserLoginInfo()).ToArray();
            }
        }

        public async Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            using (var session = this.database.CreateSession())
            {
                var extent = new Logins(session).Extent();
                extent.Filter.AddEquals(M.Login.Provider, loginProvider);
                extent.Filter.AddEquals(M.Login.Key, providerKey);

                var login = extent.FirstOrDefault();
                login?.Delete();

                session.Derive();
                session.Commit();
            }
        }

        #endregion

        #region IUserClaimStore
        public async Task AddClaimsAsync(IdentityUser identityUser, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = identityUser.User(session);

                foreach (var claim in claims)
                {
                    var identityClaim = new IdentityClaimBuilder(session)
                        .WithType(claim.Type)
                        .WithValueType(claim.ValueType)
                        .WithValue(claim.Value)
                        .WithIssuer(claim.Issuer)
                        .WithOriginalIssuer(claim.OriginalIssuer)
                        .Build();

                    user.AddIdentityClaim(identityClaim);
                }

                session.Derive();
                session.Commit();
            }
        }

        public async Task<IList<Claim>> GetClaimsAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = (User)session.Instantiate(identityUser.Id);
                return user.IdentityClaims.Select(v => v.AsClaim()).ToArray();
            }
        }

        public async Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var extent = new IdentityClaims(session).Extent();
                extent.Filter.AddEquals(M.IdentityClaim.Type, claim.Type);
                extent.Filter.AddEquals(M.IdentityClaim.ValueType, claim.ValueType);
                extent.Filter.AddEquals(M.IdentityClaim.Value, claim.Value);
                extent.Filter.AddEquals(M.IdentityClaim.Issuer, claim.Issuer);

                var users = extent.ToArray().Select(v => v.UserWhereIdentityClaim);
                return users.Select(v => v.AsIdentityUser()).ToArray();
            }
        }

        public async Task RemoveClaimsAsync(IdentityUser identityUser, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            using (var session = this.database.CreateSession())
            {
                var user = identityUser.User(session);
                foreach (var claim in claims)
                {
                    foreach (var identityClaim in user.IdentityClaims.ToArray())
                    {
                        if (identityClaim.Issuer == claim.Issuer &&
                            identityClaim.Type == claim.Type &&
                            identityClaim.ValueType == claim.ValueType &&
                            identityClaim.Value == claim.Value)
                        {
                            identityClaim.Delete();
                        }
                    }
                }

                session.Derive();
                session.Commit();
            }
        }

        public async Task ReplaceClaimAsync(IdentityUser identityUser, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = identityUser.User(session);
                foreach (var identityClaim in user.IdentityClaims.ToArray())
                {
                    if (identityClaim.Issuer == claim.Issuer &&
                        identityClaim.Type == claim.Type &&
                        identityClaim.ValueType == claim.ValueType)
                    {
                        identityClaim.Value = newClaim.Value;
                        break;
                    }
                }

                session.Derive();
                session.Commit();
            }
        }

        #endregion

        #region IUserSecurityStampStore
        public async Task<string> GetSecurityStampAsync(IdentityUser identityUser, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = identityUser.User(session);
                return user.SecurityStamp;
            }
        }

        public async Task SetSecurityStampAsync(IdentityUser identityUser, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            using (var session = this.database.CreateSession())
            {
                var user = identityUser.User(session);
                user.SecurityStamp = stamp;

                session.Derive();
                session.Commit();
            }
        }

        #endregion
    }
}
