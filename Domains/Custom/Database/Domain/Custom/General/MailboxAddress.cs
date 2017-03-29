//------------------------------------------------------------------------------------------------- 
// <copyright file="MailboxAddress.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the MailboxAddress type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using Allors.Meta;

    /// <summary>
    /// A MailboxAddress is a address in a mailbox in the postoffice
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
