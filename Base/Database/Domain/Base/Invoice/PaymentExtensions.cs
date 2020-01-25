// <copyright file="PaymentExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;
    using Resources;

    public static partial class PaymentExtensions
    {
        public static void BaseOnBuild(this Payment @this, ObjectOnBuild method)
        {
            if (!@this.ExistEffectiveDate)
            {
                @this.EffectiveDate = @this.Strategy.Session.Now().Date;
            }
        }

        public static void BaseOnPreDerive(this Payment @this, ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(@this) || changeSet.IsCreated(@this) || changeSet.HasChangedRoles(@this))
            {
                foreach (PaymentApplication paymentApplication in @this.PaymentApplications)
                {
                    iteration.AddDependency(@this, paymentApplication);
                    iteration.Mark(paymentApplication);
                }
            }
        }

        public static void BaseOnDerive(this Payment @this, ObjectOnDerive method)
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

        public static void BaseDelete(this Payment @this, DeletableDelete method)
        {
            foreach (PaymentApplication paymentApplication in @this.PaymentApplications)
            {
                paymentApplication.Delete();
            }
        }
    }
}
