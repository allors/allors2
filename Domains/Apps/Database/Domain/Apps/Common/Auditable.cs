//------------------------------------------------------------------------------------------------- 
// <copyright file="Auditable.cs" company="inxin bvba">
// Copyright 2014-2015 inxin bvba.
// 
// Dual Licensed under
//   a) the Affero General Public Licence v3 (AGPL)
//   b) the Allors License
// 
// The AGPL License is included in the file LICENSE.
// The Allors License is an addendum to your contract.
// 
// Dipu is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// For more information visit http://www.dipu.com/legal
// </copyright>
//-------------------------------------------------------------------------------------------------

using System;

namespace Allors.Domain
{
    public static class AuditableExtension
    {
        public static void DipuOnDerive(this Auditable @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var user = new Users(@this.Strategy.Session).GetCurrentAuthenticatedUser();

            if (user != null)
            {
                var changeSet = derivation.ChangeSet;
                if (changeSet.Created.Contains(@this.Id))
                {
                    @this.CreationDate = DateTime.UtcNow;
                    @this.CreatedBy = user;
                }

                if (changeSet.Associations.Contains(@this.Id))
                {
                    @this.LastModifiedDate = DateTime.UtcNow;
                    @this.LastModifiedBy = user;
                }
            }
        }
    }
}
