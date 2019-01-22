//------------------------------------------------------------------------------------------------- 
// <copyright file="SalesInvoiceTests.cs" company="Allors bvba">
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

using System.Linq;

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
        public void GivenSalesOrder_WhenDeriving_ThenPrintDocumentRendered()
        {
            // Arrange
            var demo = new Demo(this.Session, null);
            demo.Execute();

            // Act
            this.Session.Derive(true);

            // Assert
            var order = new SalesOrders(this.Session).Extent().First;

            Assert.NotNull(order.PrintDocument);
            var result = order.PrintDocument;

            var desktopDir = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var outputFile = System.IO.File.Create(System.IO.Path.Combine(desktopDir, "salesOrder.odt"));
            var stream = new System.IO.MemoryStream(result.Media.MediaContent.Data);

            stream.CopyTo(outputFile);
            stream.Close();
        }
    }
}