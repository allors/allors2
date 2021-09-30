// <copyright file="RepeatingSalesInvoices.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial class RepeatingSalesInvoices
    {
        public static void Daily(ISession session)
        {
            foreach (RepeatingSalesInvoice repeatingSalesInvoice in new RepeatingSalesInvoices(session).Extent())
            {
                if (repeatingSalesInvoice.NextExecutionDate.Date <= session.Now().Date
                    && (!repeatingSalesInvoice.ExistFinalExecutionDate || repeatingSalesInvoice.FinalExecutionDate >= session.Now().Date))
                {
                    repeatingSalesInvoice.Repeat();
                }
            }
        }
    }
}
