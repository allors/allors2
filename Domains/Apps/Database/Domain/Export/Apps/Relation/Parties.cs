// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Parties.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Parties
    {
        protected override void AppsPrepare(Setup setup)
        {
            base.AppsPrepare(setup);

            setup.AddDependency(this.ObjectType, M.ContactMechanismPurpose);
            setup.AddDependency(this.ObjectType, M.Settings);
        }

        public static void AppsOnDeriveRevenues(ISession session)
        {
            foreach (Party party in session.Extent<Party>())
            {
                party.AppsOnDeriveRevenue();
            }
        }
    }
}