// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegalForms.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    public partial class LegalForms
    {
        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new LegalFormBuilder(this.Session).WithDescription("UK - Public Limited Company").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Limited Liability Company").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - One Person Private Limited Company").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Cooperative Company WithLimited Liability").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Cooperative Company With Unlimited Liability").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - General Partnership").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Limited Partnership").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Non Stock Corporation").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Company Established For Social Purposes").Build();
            new LegalFormBuilder(this.Session).WithDescription("UK - Self Employed").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SA").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SAS").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SASU").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SARL").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SNC").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SCS").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - SCA").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - EURL").Build();
            new LegalFormBuilder(this.Session).WithDescription("FR - Indépendent").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - NV / SA").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - BVBA / SPRL").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - EBVBA / SPRLU").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - CBVBA / SCRL").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - CVOA / SCRI").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - Comm VA / SNC").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - Comm V / SCS").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - Maatschap / Société de Droit Commun").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - VZW / ASBL").Build();
            new LegalFormBuilder(this.Session).WithDescription("BE - Zelfstandige / Indépendent").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Eenmanszaak").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Vennotschap onder firma (vof)").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Commanditaire vennootschap (cv)").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Maatschap").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Besloten vennootschap (bv)").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Naamloze vennootschap (nv)").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Vereniging").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Coörperatie en onderlinge waarborgmaatschappij").Build();
            new LegalFormBuilder(this.Session).WithDescription("NL - Stichting").Build();
        }
    }
}