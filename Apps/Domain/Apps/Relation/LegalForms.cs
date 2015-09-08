// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LegalForms.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Domain
{
    public partial class LegalForms
    {
        protected override void AppsSetup(Setup setup)
        {
            base.AppsSetup(setup);

            new LegalFormBuilder(Session).WithDescription("UK - Public Limited Company").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Limited Liability Company").Build();
            new LegalFormBuilder(Session).WithDescription("UK - One Person Private Limited Company").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Cooperative Company WithLimited Liability").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Cooperative Company With Unlimited Liability").Build();
            new LegalFormBuilder(Session).WithDescription("UK - General Partnership").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Limited Partnership").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Non Stock Corporation").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Company Established For Social Purposes").Build();
            new LegalFormBuilder(Session).WithDescription("UK - Self Employed").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SA").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SAS").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SASU").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SARL").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SNC").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SCS").Build();
            new LegalFormBuilder(Session).WithDescription("FR - SCA").Build();
            new LegalFormBuilder(Session).WithDescription("FR - EURL").Build();
            new LegalFormBuilder(Session).WithDescription("FR - Indépendent").Build();
            new LegalFormBuilder(Session).WithDescription("BE - NV / SA").Build();
            new LegalFormBuilder(Session).WithDescription("BE - BVBA / SPRL").Build();
            new LegalFormBuilder(Session).WithDescription("BE - EBVBA / SPRLU").Build();
            new LegalFormBuilder(Session).WithDescription("BE - CBVBA / SCRL").Build();
            new LegalFormBuilder(Session).WithDescription("BE - CVOA / SCRI").Build();
            new LegalFormBuilder(Session).WithDescription("BE - Comm VA / SNC").Build();
            new LegalFormBuilder(Session).WithDescription("BE - Comm V / SCS").Build();
            new LegalFormBuilder(Session).WithDescription("BE - Maatschap / Société de Droit Commun").Build();
            new LegalFormBuilder(Session).WithDescription("BE - VZW / ASBL").Build();
            new LegalFormBuilder(Session).WithDescription("BE - Zelfstandige / Indépendent").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Eenmanszaak").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Vennotschap onder firma (vof)").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Commanditaire vennootschap (cv)").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Maatschap").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Besloten vennootschap (bv)").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Naamloze vennootschap (nv)").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Vereniging").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Coörperatie en onderlinge waarborgmaatschappij").Build();
            new LegalFormBuilder(Session).WithDescription("NL - Stichting").Build();
        }

        protected override void AppsSecure(Security config)
        {
            base.AppsSecure(config);
            
            var full = new[] { Operation.Read, Operation.Write, Operation.Execute };

            config.GrantAdministrator(this.ObjectType, full);
        }
    }
}