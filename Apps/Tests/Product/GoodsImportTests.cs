//------------------------------------------------------------------------------------------------- 
// <copyright file="GoodsImportTests.cs" company="Allors bvba">
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
    // TODO: 
    //[TestFixture]
    //public class GoodsImportTests : DomainTest
    //{
    //    [Test]
    //    public void InsertNoSettingsCodeOnly()
    //    {
    //        var table = new GoodsTable
    //                        {
    //                            Records =
    //                                new List<GoodsRecord>
    //                                    {
    //                                        new GoodsRecord { Code = "X-001" },
    //                                        new GoodsRecord { Code = "X-002" }
    //                                    }
    //                        };

    //        var settings = new GoodsImportSettings();
    //        var importLog = new DefaultImportLog();
    //        var productImport = new GoodsImport(this.DatabaseSession, table, settings, importLog);
    //        productImport.Execute();

    //        var goods = this.GetObjects(this.DatabaseSession, Goods.Meta.ObjectType);

    //        var skus = new List<string>();

    //        Assert.AreEqual(2, goods.Length);
    //        foreach (Good good in goods)
    //        {
    //            // From CatalogueItem
    //            Assert.IsTrue(good.ExistSku);
    //            Assert.IsFalse(good.ExistName);
    //            Assert.IsFalse(good.ExistDescription);

    //            // From Good
    //            Assert.IsFalse(good.ExistPrimaryProductCategory);
    //            Assert.IsFalse(good.ExistVatRate);
    //            Assert.IsFalse(good.ExistUnitOfMeasure);

    //            skus.Add(good.Sku);
    //        }

    //        Assert.Contains("X-001", skus);
    //        Assert.Contains("X-002", skus);
    //    }

    //    [Test]
    //    public void InsertNoSettingsAndEverything()
    //    {
    //        var primaryProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithCode("PRIM").Build();
    //        var secondaryProductCategory = new ProductCategoryBuilder(this.DatabaseSession).WithCode("SEC").Build();
    //        var fabrikant = new OrganisationBuilder(this.DatabaseSession).WithUniqueId(Guid.NewGuid()).Build();

    //        this.DatabaseSession.Commit();

    //        var table = new GoodsTable
    //        {
    //            Records =
    //                new List<GoodsRecord>
    //                                    {
    //                                        new GoodsRecord
    //                                        {
    //                                            Code = "X-001",
    //                                            Name = "naam",
    //                                            Description = "beschrijving",
    //                                            Category = "PRIM, SEC",
    //                                            Manufacturer = "FAB",
    //                                            CostPrice = "60",
    //                                            SellingPrice = "100",
    //                                            VatRate = "0.21",
    //                                            UnitOfMeasure = "Kilogram",
    //                                            QuantityOnHand = "10",
    //                                        }
    //                                    }
    //        };

    //        var settings = new GoodsImportSettings();
    //        var importLog = new DefaultImportLog();
    //        var productImport = new GoodsImport(this.DatabaseSession, table, settings, importLog);
    //        productImport.Execute();

    //        var goods = this.GetObjects(this.DatabaseSession, Goods.Meta.ObjectType);

    //        Assert.AreEqual(1, goods.Length);
    //        foreach (Good good in goods)
    //        {
    //            // From CatalogueItem
    //            Assert.AreEqual("X-001", good.Sku);
    //            Assert.AreEqual("naam", good.Name);
    //            Assert.AreEqual("beschrijving", good.Description);

    //            // From Good
    //            Assert.AreEqual(primaryProductCategory, good.PrimaryProductCategory);
    //            Assert.AreEqual(0.21m, good.VatRate);
    //            Assert.AreEqual("Kilogram", good.UnitOfMeasure);
    //        }
    //    }
    //}
}