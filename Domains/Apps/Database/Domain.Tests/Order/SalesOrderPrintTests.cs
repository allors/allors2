//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesOrderPrintTests.cs" company="Allors bvba">
// Copyright 2002-2009 Allors bvba.
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
// <summary>Defines the MediaTests type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    using System;

    using Xunit;

    public class SalesOrderPrintTests : DomainTest
    {
        [Fact]
        public void GivenSalesOrder_WhenCreatingPrintModel_ThenPrintModelIsNotNull()
        {
            // Arrange
            var demo = new Demo(this.Session, null);
            demo.Execute();
            this.Session.Derive(true);

            var order = new SalesOrders(this.Session).Extent().First;

            // Act
            var printModel = new Print.SalesOrderModel.Model(order);

            // Assert
            Assert.NotNull(printModel);
        }


        [Fact]
        public void GivenSalesOrder_WhenDeriving_henPrintDocumentWithoutMediaCreated()
        {
            // Arrange
            var demo = new Demo(this.Session, null);
            demo.Execute();

            // Act
            this.Session.Derive(true);

            // Assert
            var order = new SalesOrders(this.Session).Extent().First;

            Assert.True(order.ExistPrintDocument);
            Assert.False(order.PrintDocument.ExistMedia);
        }

        [Fact]
        public void GivenSalesOrderPrintDocument_WhenPrinting_ThenMediaCreated()
        {
            // Arrange
            var demo = new Demo(this.Session, null);
            demo.Execute();
            this.Session.Derive(true);
            var order = new SalesOrders(this.Session).Extent().First;

            // Act
            order.Print();

            this.Session.Derive();
            this.Session.Commit();

            // Assert
            Assert.True(order.PrintDocument.ExistMedia);

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var outputFile = System.IO.File.Create(System.IO.Path.Combine(desktopDir, "salesOrder.odt"));
            var stream = new System.IO.MemoryStream(order.PrintDocument.Media.MediaContent.Data);

            stream.CopyTo(outputFile);
            stream.Close();
        }
    }
}