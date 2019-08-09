// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateTypes.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
// --------------------------------------------------------------------------------------------------------------------

using System;

namespace Allors.Domain
{
    public partial class TemplateTypes
    {
        public static readonly Guid OpenDocumentTypeId = new Guid("64B48FA3-EDF2-45A3-ADFB-4A55E14E0B34");

        private UniquelyIdentifiableSticky<TemplateType> sticky;

        public Sticky<Guid, TemplateType> Sticky => this.sticky ?? (this.sticky = new UniquelyIdentifiableSticky<TemplateType>(this.Session));

        public TemplateType OpenDocumentType => this.Sticky[OpenDocumentTypeId];

        protected override void CoreSetup(Setup setup)
        {
            new TemplateTypeBuilder(this.Session).WithUniqueId(OpenDocumentTypeId).WithName("Odt Template").Build();
        }

        protected override void CoreSecure(Domain.Security config)
        {
            var full = new[] { Operations.Read, Operations.Write, Operations.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}