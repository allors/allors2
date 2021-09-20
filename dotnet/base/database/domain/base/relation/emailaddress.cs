// <copyright file="EmailAddress.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System.Text.RegularExpressions;

    public partial class EmailAddress
    {
        public bool IsPostalAddress => false;

        public static bool IsValid(string emailAddress)
        {
            const string PatternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                         + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                         + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                         + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                         + @"[a-zA-Z]{2,}))$";

            var regexStrict = new Regex(PatternStrict);
            var isStrictMatch = regexStrict.IsMatch(emailAddress);
            return isStrictMatch;
        }

        public void BaseOnPreDerive(ObjectOnPreDerive method)
        {
            var (iteration, changeSet, derivedObjects) = method;

            if (iteration.IsMarked(this) || changeSet.IsCreated(this) || changeSet.HasChangedRoles(this))
            {
                foreach (PartyContactMechanism partyContactMechanism in this.PartyContactMechanismsWhereContactMechanism)
                {
                    iteration.AddDependency(partyContactMechanism, this);
                    iteration.Mark(partyContactMechanism);
                }
            }
        }

        public override string ToString() => this.ElectronicAddressString;
    }
}
