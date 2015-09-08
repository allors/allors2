// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepCommissions.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    public partial class SalesRepCommissions
    {
        public static void AppsFindOrCreateAsDependable(ISession session, SalesRepRevenue dependant)
        {
            var salesRepCommissions = dependant.SalesRep.SalesRepCommissionsWhereSalesRep;
            salesRepCommissions.Filter.AddEquals(SalesRepCommissions.Meta.Year, dependant.Year);
            salesRepCommissions.Filter.AddEquals(SalesRepCommissions.Meta.Month, dependant.Month);
            var salesRepCommission = salesRepCommissions.First;

            if (salesRepCommission == null)
            {
                salesRepCommission = new SalesRepCommissionBuilder(session)
                    .WithInternalOrganisation(dependant.InternalOrganisation)
                    .WithSalesRep(dependant.SalesRep)
                    .WithYear(dependant.Year)
                    .WithMonth(dependant.Month)
                    .WithCurrency(dependant.Currency)
                    .WithCommission(0)
                    .Build();
            }
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
