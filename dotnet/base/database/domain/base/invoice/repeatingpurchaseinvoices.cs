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
            foreach (RepeatingPurchaseInvoice repeatingPurchaseInvoice in new RepeatingPurchaseInvoices(session).Extent())
            {
                if (repeatingPurchaseInvoice.NextExecutionDate.Date <= session.Now().Date
                    && (!repeatingPurchaseInvoice.ExistFinalExecutionDate || repeatingPurchaseInvoice.FinalExecutionDate >= session.Now().Date))
                {
                    repeatingPurchaseInvoice.Repeat();
                }
            }
        }
    }
}
