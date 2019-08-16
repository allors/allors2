//-------------------------------------------------------------------------------------------------
// <copyright file="MailboxAddress.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MailboxAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;

    /// <summary>
    /// A MailboxAddress is a address in a mailbox in the postoffice.
    /// </summary>
    public partial class MailboxAddress
    {
        public void CustomOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;

            derivation.Validation.AssertExists(this, M.MailboxAddress.PoBox);
            derivation.Validation.AssertNonEmptyString(this, M.MailboxAddress.PoBox);

            derivation.Validation.AssertExists(this, M.MailboxAddress.Place);
        }
    }
}
