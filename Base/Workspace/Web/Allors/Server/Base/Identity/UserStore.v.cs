// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserStore.v.cs" company="Allors bvba">
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
    using Allors.Domain;

    public partial class UserStore 
    {
        private void CreateUser(IdentityUser identityUser)
        {
            using (var session = this.Database.CreateSession())
            {
                var person = new PersonBuilder(session)
                    .WithUserName(identityUser.UserName)
                    .WithUserEmail(identityUser.Email)
                    .WithUserEmailConfirmed(identityUser.EmailConfirmed)
                    .WithUserPasswordHash(identityUser.PasswordHash)
                    .Build();

                session.Derive();
                identityUser.MapFrom(person);
                session.Commit();
            }
        }

        private void UpdateUser(IdentityUser identityUser)
        {
            if (!string.IsNullOrWhiteSpace(identityUser.Id))
            {
                using (var session = this.Database.CreateSession())
                {
                    var person = (Person)session.Instantiate(identityUser.Id);
                    if (person != null)
                    {
                        person.UserName = identityUser.UserName;
                        person.UserEmail = identityUser.Email;
                        person.UserEmailConfirmed = identityUser.EmailConfirmed;
                        person.UserPasswordHash = identityUser.PasswordHash;

                        session.Derive();
                        session.Commit();
                    }
                }
            }
        }
    }
}