// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PaymentExtensions.cs" company="Allors bvba">
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

using System;
using Allors.Meta;
using Resources;

namespace Allors.Domain
{
    public static partial class PaymentExtensions
    {
        public static void AppsOnBuild(this Payment @this, ObjectOnBuild method)
        {
            if (!@this.ExistEffectiveDate)
            {
                @this.EffectiveDate = DateTime.UtcNow;
            }
        }

        public static void AppsOnPreDerive(this Payment @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (PaymentApplication paymentApplication in @this.PaymentApplications)
            {
                derivation.AddDependency(@this, paymentApplication);
            }
        }

        public static  void AppsOnDerive(this Payment @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            decimal totalAmountApplied = 0;
            foreach (PaymentApplication paymentApplication in @this.PaymentApplications)
            {
                totalAmountApplied += paymentApplication.AmountApplied;
            }

            if (@this.ExistAmount && totalAmountApplied > @this.Amount)
            {
                derivation.Validation.AddError(@this, M.Payment.Amount, ErrorMessages.PaymentAmountIsToSmall);
            }
        }

        public static void AppsDelete(this Payment @this, DeletableDelete method)
        {
            foreach (PaymentApplication paymentApplication in @this.PaymentApplications)
            {
                paymentApplication.Delete();
            }
        }
    }
}