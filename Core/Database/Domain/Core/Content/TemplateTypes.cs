// <copyright file="TemplateTypes.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class TemplateTypes
    {
        public static readonly Guid OpenDocumentTypeId = new Guid("64B48FA3-EDF2-45A3-ADFB-4A55E14E0B34");

        private UniquelyIdentifiableSticky<TemplateType> sticky;

        public Sticky<Guid, TemplateType> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<TemplateType>(this.Session));

        public TemplateType OpenDocumentType => this.Sticky[OpenDocumentTypeId];

        protected override void CoreSetup(Setup setup) => new TemplateTypeBuilder(this.Session).WithUniqueId(OpenDocumentTypeId).WithName("Odt Template").Build();

        protected override void CoreSecure(Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}
