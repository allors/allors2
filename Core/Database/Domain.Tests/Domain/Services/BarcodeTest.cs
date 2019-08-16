// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodeTest.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
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

namespace Tests
{
    using System.IO;

    using Allors.Services;

    using Microsoft.Extensions.DependencyInjection;

    using Xunit;

    public class BarcodeTest : DomainTest
    {
        [Fact]
        public void Default()
        {
            var barcodeService = this.Session.ServiceProvider.GetRequiredService<IBarcodeService>();
            var image = barcodeService.Generate("Allors", BarcodeType.CODE_128);
            File.WriteAllBytes("barcode.png", image);
        }
    }
}
