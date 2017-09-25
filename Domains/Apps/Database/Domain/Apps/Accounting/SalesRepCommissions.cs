// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SalesRepCommissions.cs" company="Allors bvba">
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
    using Meta;

    public partial class SalesRepCommissions
    {
        public static void AppsFindOrCreateAsDependable(ISession session, SalesRepRevenue dependant)
        {
            var salesRepCommissions = dependant.SalesRep.SalesRepCommissionsWhereSalesRep;
            salesRepCommissions.Filter.AddEquals(M.SalesRepCommission.Year, dependant.Year);
            salesRepCommissions.Filter.AddEquals(M.SalesRepCommission.Month, dependant.Month);
            var salesRepCommission = salesRepCommissions.First;

            if (salesRepCommission == null)
            {
                new SalesRepCommissionBuilder(session)
                    .WithSalesRep(dependant.SalesRep)
                    .WithYear(dependant.Year)
                    .WithMonth(dependant.Month)
                    .WithCurrency(dependant.Currency)
                    .WithCommission(0)
                    .Build();
            }
        }
    }
}
