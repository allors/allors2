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
    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class TemplateTests : DomainTest
    {
        [Fact]
        public void Render()
        {
            var model = new EmailViewModel
                            {
                                UserName = "Koen"
                            };

            var templateService = this.Session.ServiceProvider.GetRequiredService<ITemplateService>();
            var result = templateService.Render("Views/EmailTemplate.cshtml", model).Result;

            Assert.Equal("Hello Koen!", result.Trim());
        }
    }
}
