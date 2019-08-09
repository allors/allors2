// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DocumentTests.cs" company="Allors bvba">
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
    using System.Collections.Generic;
    using System.IO;

    using Allors;
    using Allors.Domain;

    using Xunit;

    public class TemplateTests : DomainTest
    {
        [Fact]
        public void Render()
        {
            var media = new MediaBuilder(this.Session).WithInData(this.GetResourceBytes("Domain.Tests.Resources.EmbeddedTemplate.odt")).Build();
            var templateType = new TemplateTypes(this.Session).OpenDocumentType;
            var template = new TemplateBuilder(this.Session).WithMedia(media).WithTemplateType(templateType).WithArguments("logo, people").Build();

            this.Session.Derive();

            var people = new People(this.Session).Extent();
            var logo = this.GetResourceBytes("Domain.Tests.Resources.logo.png");

            var data = new Dictionary<string, object>()
                           {
                               { "people", people },
                           };

            var images = new Dictionary<string, byte[]>()
                            {
                                { "logo", logo },
                            };

            var watch = System.Diagnostics.Stopwatch.StartNew();

            var result = template.Render(data, images);

            watch.Stop();
            var run1 = watch.ElapsedMilliseconds;

            File.WriteAllBytes("Embedded.odt", result);

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            watch = System.Diagnostics.Stopwatch.StartNew();

            result = template.Render(data, images);

            watch.Stop();
            var run2 = watch.ElapsedMilliseconds;

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            // Reuse should make it at least 5 times faster
            Assert.True(run2 < run1 / 5);
        }
    }
}
