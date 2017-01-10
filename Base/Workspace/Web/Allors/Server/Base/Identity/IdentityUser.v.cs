// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IdentityUser.v.cs" company="Allors bvba">
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

    public partial class IdentityUser
    {
        public IdentityUser MapFrom(User user)
        {
            this.Id = user.Id.ToString();
            this.UserName = user.UserName;
            this.EmailConfirmed = user.UserEmailConfirmed.HasValue && user.UserEmailConfirmed.Value;
            this.Email = user.UserEmail;
            this.PasswordHash = user.UserPasswordHash;

            return this;
        }
    }
}