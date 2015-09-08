//------------------------------------------------------------------------------------------------- 
// <copyright file="ProductCategoriesImportTests.cs" company="Allors bvba">
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
    //public class ProductCategoriesImportTests : DomainTest
    //{
    //    [Test]
    //    public void InsertNoSettingsCodeOnly()
    //    {
    //        var table = new ProductCategoriesTable
    //                        {
    //                            Records =
    //                                new List<ProductCategoriesRecord>
    //                                    {
    //                                        new ProductCategoriesRecord { Code = "SW" },
    //                                        new ProductCategoriesRecord { Code = "HW" }
    //                                    }
    //                        };

    //        var productImport = new ProductCategoriesImport(this.DatabaseSession, table, new ProductCategoriesImportSettings(), new DefaultImportLog());
    //        productImport.Execute();

    //        var productCategories = this.GetObjects(this.DatabaseSession, ProductCategories.Meta.ObjectType);

    //        var skus = new List<string>();

    //        Assert.AreEqual(2, productCategories.Length);
    //        foreach (ProductCategory productCategory in productCategories)
    //        {
    //            // From CatalogueItem
    //            Assert.IsTrue(productCategory.ExistCode);
    //            Assert.IsFalse(productCategory.ExistName);
    //            Assert.IsFalse(productCategory.ExistDescription);

    //            // From ProductCategory
    //            Assert.IsFalse(productCategory.ExistDescription);
    //            Assert.IsFalse(productCategory.ExistName);
    //            Assert.IsTrue(productCategory.ExistCode);

    //            skus.Add(productCategory.Code);
    //        }

    //        Assert.Contains("SW", skus);
    //        Assert.Contains("HW", skus);
    //    }
    //}
}