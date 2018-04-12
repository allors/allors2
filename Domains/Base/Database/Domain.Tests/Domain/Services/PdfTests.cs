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
    using System.IO;

    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class PdfTests : DomainTest
    {
        [Fact]
        public async void Render()
        {
            const string Html = @"<!DOCTYPE html>
<html>
<head>
<title>Martien Title</title>
</head>

<body>
Hallo Walter!
</body>
<html>";

            var pdfService = this.Session.ServiceProvider.GetRequiredService<IPdfService>();
            
            byte[] pdf = null;
            for (var i = 0; i < 1000; i++)
            {
                pdf = await pdfService.FromHtmlToPdf(Html);
                Console.WriteLine(i);
            }

            File.WriteAllBytes(@"C:\temp\generated.pdf", pdf);

            Assert.NotNull(pdf);
        }
    }
}
