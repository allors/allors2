// <copyright file="TemplateTests.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>
//   Defines the PersonTests type.
// </summary>

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

            var result = template.Render(data, images);

            File.WriteAllBytes("Embedded.odt", result);

            Assert.NotNull(result);
            Assert.NotEmpty(result);

            result = template.Render(data, images);

            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }
    }
}
