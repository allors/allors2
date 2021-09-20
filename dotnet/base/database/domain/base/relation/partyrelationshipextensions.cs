// <copyright file="PartyRelationshipExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public static partial class PartyRelationshipExtensions
    {
        public static void BaseOnBuild(this PartyRelationship @this, ObjectOnBuild method)
        {
            if (!@this.ExistFromDate)
            {
                @this.FromDate = @this.Strategy.Session.Now();
            }
        }

        public static int? PaymentNetDays(this PartyRelationship @this)
        {
            int? customerPaymentNetDays = null;

            foreach (Agreement agreement in @this.Agreements)
            {
                foreach (AgreementTerm term in agreement.AgreementTerms)
                {
                    if (term.TermType.Equals(new InvoiceTermTypes(@this.Strategy.Session).PaymentNetDays))
                    {
                        if (int.TryParse(term.TermValue, out var netDays))
                        {
                            customerPaymentNetDays = netDays;
                        }

                        return customerPaymentNetDays;
                    }
                }
            }

            return null;
        }
    }
}
