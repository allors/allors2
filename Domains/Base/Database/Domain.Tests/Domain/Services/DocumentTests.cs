// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateTests.cs" company="Allors bvba">
//   Copyright 2002-2009 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Allors.Domain;
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Sandwych.Reporting;
    using Sandwych.Reporting.OpenDocument;

    using Xunit;

    public class DocumentTests : DomainTest
    {
        [Fact]
        public void Render()
        {
            var documentService = (DocumentService)this.DocumentService;
            documentService.Register("Embedded", this.GetResource("Domain.Tests.Resources.EmbeddedTemplate.odt"));
            documentService.Register("File", new FileInfo("Resources/FileTemplate.odt"));

            var people = new People(this.Session).Extent();
            var image = new ImageBlob("jpeg", this.GetResourceBytes("Domain.Tests.Resources.logo.png"));

            var data = new Dictionary<string, object>()
                           {
                               { "logo", image },
                               { "people", people },
                           };

            var embedded = documentService.Render("Embedded", data).Result;

            File.WriteAllBytes("Embedded.odt", embedded);
            
            var file = documentService.Render("File", data).Result;

            File.WriteAllBytes("File.odt", file);

            Assert.Equal(embedded, file);
        }
    }
}
