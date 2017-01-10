// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserStore.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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

namespace Allors.Web.Identity
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Allors.Domain;
    using Allors.Meta;

    using Microsoft.AspNet.Identity;

    using Task = System.Threading.Tasks.Task;

    public partial class UserStore : IUserStore<IdentityUser>,
                             IUserEmailStore<IdentityUser>,/*
                             IUserClaimStore<IdentityUser>,*/
                             IUserLoginStore<IdentityUser>,
                             IUserRoleStore<IdentityUser>,
                             IUserPasswordStore<IdentityUser>,
                             IUserLockoutStore<IdentityUser, string>,
                             IUserTwoFactorStore<IdentityUser, string>,
                             IUserPhoneNumberStore<IdentityUser>
    {
        private IDatabase Database => Config.Default;

        public void Dispose()
        {
        }

        public Task CreateAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (!string.IsNullOrWhiteSpace(identityUser.Id))
            {
                throw new ArgumentException(nameof(identityUser));
            }

            this.CreateUser(identityUser);

            return Task.FromResult<object>(null);
        }

        public Task UpdateAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            this.UpdateUser(identityUser);

            return Task.FromResult<object>(null);
        }

        public Task DeleteAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            using (var session = this.Database.CreateSession())
            {
                var person = (Person)session.Instantiate(identityUser.Id);
                if (person != null)
                {
                    person.Delete();
                    session.Derive();
                    session.Commit();
                }
            }

            return Task.FromResult<object>(null);
        }

        public Task<IdentityUser> FindByIdAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException(nameof(userId));
            }

            try
            {
                using (var session = this.Database.CreateSession())
                {
                    var person = (Person)session.Instantiate(userId);
                    if (person != null)
                    {
                        var result = new IdentityUser().MapFrom(person);
                        return Task.FromResult(result);
                    }
                }
            }
            catch
            {
                // Do not throw an exception, because the
                // database might not be initialized at this point.
                return Task.FromResult<IdentityUser>(null);
            }

            return Task.FromResult<IdentityUser>(null);
        }

        public Task<IdentityUser> FindByNameAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("userName");
            }

            using (var session = this.Database.CreateSession())
            {
                var persons = new People(session).Extent();
                persons.Filter.AddEquals(M.Person.UserName.RoleType, userName);
                switch (persons.Count)
                {
                    case 0:
                        return Task.FromResult<IdentityUser>(null);
                    case 1:
                        var identityUser = new IdentityUser().MapFrom(persons.First);
                        return Task.FromResult(identityUser);
                    default:
                        throw new Exception("Found multiple users with username " + userName);
                }
            }
        }

        public Task<string> GetEmailAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            return Task.FromResult(identityUser.Email);
        }

        public Task SetEmailAsync(IdentityUser identityUser, string email)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            identityUser.Email = email;
            this.UpdateUser(identityUser);

            return Task.FromResult<object>(null);
        }

        public Task<bool> GetEmailConfirmedAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            return Task.FromResult(identityUser.EmailConfirmed);
        }

        public Task SetEmailConfirmedAsync(IdentityUser identityUser, bool confirmed)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            identityUser.EmailConfirmed = confirmed;
            this.UpdateUser(identityUser);

            return Task.FromResult<object>(null);
        }

        public Task<IdentityUser> FindByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(nameof(email));
            }

            using (var session = this.Database.CreateSession())
            {
                var persons = new People(session).Extent();
                persons.Filter.AddEquals(M.Person.UserEmail.RoleType, email);
                switch (persons.Count)
                {
                    case 0:
                        return Task.FromResult<IdentityUser>(null);
                    case 1:
                        return Task.FromResult(new IdentityUser().MapFrom(persons.First));
                    default:
                        throw new Exception("Found multiple users with email " + email);
                }
            }
        }

        public Task<string> GetPasswordHashAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            return Task.FromResult(identityUser.PasswordHash);
        }

        public Task SetPasswordHashAsync(IdentityUser identityUser, string passwordHash)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            identityUser.PasswordHash = passwordHash;
            this.UpdateUser(identityUser);

            return Task.FromResult<object>(null);
        }

        public Task<bool> HasPasswordAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            return Task.FromResult(!string.IsNullOrWhiteSpace(identityUser.PasswordHash));
        }

        public Task AddToRoleAsync(IdentityUser identityUser, string roleName)
        {
            throw new NotSupportedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser identityUser, string roleName)
        {
            throw new NotSupportedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (!string.IsNullOrWhiteSpace(identityUser.Id))
            {
                using (var session = this.Database.CreateSession())
                {
                    var person = (Person)session.Instantiate(identityUser.Id);
                    if (person != null)
                    {
                        var roles = new List<string>();
                        foreach (UserGroup userGroup in person.UserGroupsWhereMember)
                        {
                            roles.Add(userGroup.Name);
                        }

                        return Task.FromResult<IList<string>>(roles);
                    }
                }
            }

            return Task.FromResult<IList<string>>(null);
        }

        public Task<bool> IsInRoleAsync(IdentityUser identityUser, string roleName)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (!string.IsNullOrWhiteSpace(identityUser.Id))
            {
                using (var session = this.Database.CreateSession())
                {
                    var person = (Person)session.Instantiate(identityUser.Id);
                    if (person != null)
                    {
                        var roleNameLower = roleName.ToLowerInvariant();
                        foreach (UserGroup userGroup in person.UserGroupsWhereMember)
                        {
                            if (userGroup.Name.ToLowerInvariant().Equals(roleNameLower))
                            {
                                return Task.FromResult(true);
                            }
                        }
                    }
                }
            }
            
            return Task.FromResult(false);
        }

        public Task AddLoginAsync(IdentityUser identityUser, UserLoginInfo loginInfo)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (loginInfo == null)
            {
                throw new ArgumentNullException(nameof(loginInfo));
            }

            using (var session = this.Database.CreateSession())
            {
                var person = (Person)session.Instantiate(identityUser.Id);
                if (person != null)
                {
                    new LoginBuilder(session).WithUser(person).WithProvider(loginInfo.LoginProvider).WithKey(loginInfo.ProviderKey).Build();
                    session.Derive();
                    session.Commit();
                }
            }

            return Task.FromResult<object>(null);
        }

        public Task RemoveLoginAsync(IdentityUser identityUser, UserLoginInfo loginInfo)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (loginInfo == null)
            {
                throw new ArgumentNullException(nameof(loginInfo));
            }

            var provider = loginInfo.LoginProvider;
            if (!string.IsNullOrWhiteSpace(provider))
            {
                using (var session = this.Database.CreateSession())
                {
                    var person = (Person)session.Instantiate(identityUser.Id);
                    if (person != null)
                    {
                        foreach (Login login in person.LoginsWhereUser)
                        {
                            if (provider.Equals(login.Provider))
                            {
                                login.Delete();
                            }
                        }

                        session.Derive();
                        session.Commit();
                    }
                }
            }

            return Task.FromResult<object>(null);
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser identityUser)
        {
            if (identityUser == null)
            {
                throw new ArgumentNullException(nameof(identityUser));
            }

            if (string.IsNullOrWhiteSpace(identityUser.Id))
            {
                throw new ArgumentException("id");
            }

            using (var session = this.Database.CreateSession())
            {
                var person = (Person)session.Instantiate(identityUser.Id);
                if (person != null)
                {
                    var list = new List<UserLoginInfo>();
                    foreach (Login login in person.LoginsWhereUser)
                    {
                        list.Add(new UserLoginInfo(login.Provider, login.Key));
                    }

                    return Task.FromResult<IList<UserLoginInfo>>(list);
                }
            }

            return Task.FromResult<IList<UserLoginInfo>>(null);
        }

        public Task<IdentityUser> FindAsync(UserLoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                throw new ArgumentNullException(nameof(loginInfo));
            }

            if (string.IsNullOrWhiteSpace(loginInfo.LoginProvider))
            {
                throw new ArgumentException("LoginProvider");
            }

            if (string.IsNullOrWhiteSpace(loginInfo.ProviderKey))
            {
                throw new ArgumentException("ProviderKey");
            }

            using (var session = this.Database.CreateSession())
            {
                var logins = session.Extent<Login>();
                logins.Filter.AddEquals(M.Login.Provider, loginInfo.LoginProvider);
                logins.Filter.AddEquals(M.Login.Key, loginInfo.ProviderKey);

                switch (logins.Count)
                {
                    case 0:
                        return Task.FromResult<IdentityUser>(null);
                    case 1:
                        var user = logins.First.User;
                        var identityUser = new IdentityUser().MapFrom(user);
                        return Task.FromResult(identityUser);
                    default:
                        throw new Exception("Found multiple users with login info for provider " + loginInfo.LoginProvider);
                }
            }
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser user)
        {
            return Task.FromResult(DateTimeOffset.MinValue);
        }

        public Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset lockoutEnd)
        {
            return Task.FromResult<object>(null);
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(1);
        }

        public Task ResetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult<object>(null);
        }

        public Task<int> GetAccessFailedCountAsync(IdentityUser user)
        {
            return Task.FromResult(0);
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetLockoutEnabledAsync(IdentityUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            return Task.FromResult<object>(null);
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber)
        {
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPhoneNumberAsync(IdentityUser user)
        {
            return Task.FromResult<string>(null);
        }

        public Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user)
        {
            return Task.FromResult(false);
        }

        public Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed)
        {
            return Task.FromResult<object>(null);
        }
    }
}