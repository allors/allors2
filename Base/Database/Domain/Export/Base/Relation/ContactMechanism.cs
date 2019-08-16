
// <copyright file="ContactMechanism.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    public partial interface ContactMechanism
    {
        bool IsPostalAddress { get; }
    }

    public static partial class ContactMechanismExtensions
    {
        public static void BaseOnPreDerive(this ContactMechanism @this, ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            foreach (PartyContactMechanism partyContactMechanism in @this.PartyContactMechanismsWhereContactMechanism)
            {
                derivation.AddDependency(partyContactMechanism, @this);
            }
        }

        public static void BaseDelete(this ContactMechanism @this, DeletableDelete method)
        {
            foreach (PartyContactMechanism partyContactMechanism in @this.PartyContactMechanismsWhereContactMechanism)
            {
                partyContactMechanism.Delete();
            }
        }
    }
}
