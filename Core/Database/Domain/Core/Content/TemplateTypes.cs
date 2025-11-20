// <copyright file="TemplateTypes.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain
{
    using System;

    public partial class TemplateTypes
    {
        public static readonly Guid OpenDocumentTypeId = new Guid("64B48FA3-EDF2-45A3-ADFB-4A55E14E0B34");

        private UniquelyIdentifiableSticky<TemplateType> cache;

        public Sticky<Guid, TemplateType> Cache => this.cache ??= new UniquelyIdentifiableSticky<TemplateType>(this.Session);

        public TemplateType OpenDocumentType => this.Cache[OpenDocumentTypeId];

        protected override void CoreSetup(Setup setup)
        {
            var merge = this.Cache.Merger().Action();

            merge(OpenDocumentTypeId, v => v.Name = "Odt Template");
        }
    }
}
