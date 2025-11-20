// <copyright file="BarcodeTest.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

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
