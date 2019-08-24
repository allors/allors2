// <copyright file="AgreementTermExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public static partial class AgreementTermExtensions
    {
        public static void BaseOnDerive(this AgreementTerm @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertAtLeastOne(@this, M.AgreementTerm.TermType, M.AgreementTerm.Description);
        }
    }
}
