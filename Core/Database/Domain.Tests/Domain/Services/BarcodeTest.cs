// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BarcodeTest.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
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
