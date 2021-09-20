// <copyright file="SalesOrderPrintTests.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the MediaTests type.</summary>

namespace Allors.Domain
{
    using System;
    using Xunit;

    public class SalesOrderPrintTests : DomainTest
    {
        public SalesOrderPrintTests()
        {
          
        }

        //[Fact]
        //public void GivenSalesOrder_WhenCreatingPrintModel_ThenPrintModelIsNotNull()
        //{
        //    // Arrange
        //    var order = new SalesOrders(this.Session).Extent().First;

        //    // Act
        //    var printModel = new Print.SalesOrderModel.Model(order);

        //    // Assert
        //    Assert.NotNull(printModel);
        //}

        //[Fact]
        //public void GivenSalesOrder_WhenDeriving_henPrintDocumentWithoutMediaCreated()
        //{
        //    // Arrange

        //    // Act
        //    this.Session.Derive(true);

        //    // Assert
        //    var order = new SalesOrders(this.Session).Extent().First;

        //    Assert.True(order.ExistPrintDocument);
        //    Assert.False(order.PrintDocument.ExistMedia);
        //}

        //[Fact]
        //public void GivenSalesOrderPrintDocument_WhenPrinting_ThenMediaCreated()
        //{
        //    // Arrange
        //    var order = new SalesOrders(this.Session).Extent().First;

        //    // Act
        //    order.Print();

        //    this.Session.Derive();
        //    this.Session.Commit();

        //    // Assert
        //    Assert.True(order.PrintDocument.ExistMedia);

        //    var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        //    var outputFile = System.IO.File.Create(System.IO.Path.Combine(desktopDir, "salesOrder.odt"));
        //    var stream = new System.IO.MemoryStream(order.PrintDocument.Media.MediaContent.Data);

        //    stream.CopyTo(outputFile);
        //    stream.Close();
        //}
    }
}
