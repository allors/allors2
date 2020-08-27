// <copyright file="Domain.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using Allors.Meta;

    public partial class Security
    {
        private void CustomOnPreSetup()
        {
            // Default access policy
            var security = new Security(this.session);
            foreach (ObjectType @class in this.session.Database.MetaPopulation.Classes)
            {
                security.GrantAdministrator(@class, Operations.Read, Operations.Write, Operations.Execute);
                security.GrantCreator(@class, Operations.Read, Operations.Write, Operations.Execute);
            }
        }

        private void CustomOnPostSetup()
        {
        }
    }
}
