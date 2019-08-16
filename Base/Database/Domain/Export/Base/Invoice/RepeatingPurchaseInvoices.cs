
// <copyright file="RepeatingPurchaseInvoices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class RepeatingPurchaseInvoices
    {
        public static void Daily(ISession session)
        {
            var repeatingPurchaseInvoices = new RepeatingPurchaseInvoices(session).Extent();

            foreach (RepeatingPurchaseInvoice repeatingPurchaseInvoice in repeatingPurchaseInvoices)
            {
                if (repeatingPurchaseInvoice.NextExecutionDate.Date == session.Now().Date)
                {
                    repeatingPurchaseInvoice.Repeat();
                }
            }
        }
    }
}
