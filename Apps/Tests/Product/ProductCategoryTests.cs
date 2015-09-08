 //------------------------------------------------------------------------------------------------- 
// <copyright file="ProductCategoryTests.cs" company="Allors bvba">
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
    

    using NUnit.Framework;

    [TestFixture]
    public class ProductCategoryTests : DomainTest
    {
        [Test]
        public void GivenProductCategory_WhenDeriving_ThenRequiredRelationsMustExist()
        {
            var builder = new ProductCategoryBuilder(this.DatabaseSession);
            var productCategory = builder.Build();

            Assert.IsTrue(this.DatabaseSession.Derive().HasErrors);

            this.DatabaseSession.Rollback();

            builder.WithDescription("category");
            productCategory = builder.Build();

            Assert.IsFalse(this.DatabaseSession.Derive().HasErrors);
        }

        [Test]
        public void GivenLeafeProductCategory_WhenPackageIsDefined_ThenValidationHasNoErrors()
        {
            var package = new PackageBuilder(this.DatabaseSession).WithName("package").Build();

            new ProductCategoryBuilder(this.DatabaseSession).WithDescription("Category").WithPackage(package).Build();

            Assert.IsFalse(new Derivation(this.DatabaseSession).Derive().HasErrors);

            this.DatabaseSession.Rollback();
        }

        [Test]
        public void GivenParentProductCategory_WhenPackageIsDefined_ThenValidationHasErrors()
        {
            var package = new PackageBuilder(this.DatabaseSession).WithName("package").Build();

            var parent = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("parent").WithPackage(package).Build();
            var child = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("child").WithParent(parent).Build();

            Assert.IsTrue(new Derivation(this.DatabaseSession).Derive().HasErrors);
        }

        [Test]
        public void GivenProductCategory_WhenDeriving_ThenAncestorsAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();
            var productCategory122 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.2").WithParent(productCategory12).Build();

            this.DatabaseSession.Derive(true); 

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);

            Assert.AreEqual(2, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);
            Assert.Contains(productCategory2, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(3, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);
            Assert.Contains(productCategory2, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(3, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);
        }

        [Test]
        public void GivenProductCategory_WhenNewParentsAreInserted_ThenAncestorsAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();
            var productCategory122 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.2").WithParent(productCategory12).Build();

            this.DatabaseSession.Derive(true); 

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);

            Assert.AreEqual(2, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);
            Assert.Contains(productCategory2, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(3, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);
            Assert.Contains(productCategory2, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(3, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);

            var productCategory3 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("3").Build();
            productCategory11.AddParent(productCategory3);

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);
            Assert.IsFalse(productCategory3.ExistAncestors);

            Assert.AreEqual(3, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);
            Assert.Contains(productCategory2, productCategory11.Ancestors);
            Assert.Contains(productCategory3, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(4, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);
            Assert.Contains(productCategory2, productCategory111.Ancestors);
            Assert.Contains(productCategory3, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(3, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);

            var productCategory13 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.3").WithParent(productCategory1).Build();
            productCategory122.AddParent(productCategory13);

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);
            Assert.IsFalse(productCategory3.ExistAncestors);

            Assert.AreEqual(3, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);
            Assert.Contains(productCategory2, productCategory11.Ancestors);
            Assert.Contains(productCategory3, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(1, productCategory13.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory13.Ancestors);

            Assert.AreEqual(4, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);
            Assert.Contains(productCategory2, productCategory111.Ancestors);
            Assert.Contains(productCategory3, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(4, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory13, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);
        }

        [Test]
        public void GivenProductCategory_WhenNewParentsAreRemoved_ThenAncestorsAreRecalculated()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();
            var productCategory122 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.2").WithParent(productCategory12).Build();

            this.DatabaseSession.Derive(true); 

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);

            Assert.AreEqual(2, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);
            Assert.Contains(productCategory2, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(3, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);
            Assert.Contains(productCategory2, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(3, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);

            productCategory11.RemoveParent(productCategory2);

            this.DatabaseSession.Derive(true);

            Assert.IsFalse(productCategory1.ExistAncestors);
            Assert.IsFalse(productCategory2.ExistAncestors);

            Assert.AreEqual(1, productCategory11.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory11.Ancestors);

            Assert.AreEqual(2, productCategory12.Ancestors.Count);
            Assert.Contains(productCategory1, productCategory12.Ancestors);
            Assert.Contains(productCategory2, productCategory12.Ancestors);

            Assert.AreEqual(2, productCategory111.Ancestors.Count);
            Assert.Contains(productCategory11, productCategory111.Ancestors);
            Assert.Contains(productCategory1, productCategory111.Ancestors);

            Assert.AreEqual(3, productCategory121.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory121.Ancestors);
            Assert.Contains(productCategory1, productCategory121.Ancestors);
            Assert.Contains(productCategory2, productCategory121.Ancestors);

            Assert.AreEqual(3, productCategory122.Ancestors.Count);
            Assert.Contains(productCategory12, productCategory122.Ancestors);
            Assert.Contains(productCategory1, productCategory122.Ancestors);
            Assert.Contains(productCategory2, productCategory122.Ancestors);
        }

        [Test]
        public void GivenProductCategory_WhenDeriving_ThenChildrenAreSet()
        {
            var productCategory1 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1").Build();
            var productCategory2 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("2").Build();
            var productCategory11 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1").WithParent(productCategory1).Build();
            var productCategory12 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2").WithParent(productCategory1).WithParent(productCategory2).Build();
            var productCategory111 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.1.1").WithParent(productCategory11).Build();
            var productCategory121 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.1").WithParent(productCategory12).Build();
            var productCategory122 = new ProductCategoryBuilder(this.DatabaseSession).WithDescription("1.2.2").WithParent(productCategory12).Build();

            this.DatabaseSession.Derive(true); 

            Assert.AreEqual(5, productCategory1.Children.Count);
            Assert.Contains(productCategory11, productCategory1.Children);
            Assert.Contains(productCategory12, productCategory1.Children);
            Assert.Contains(productCategory111, productCategory1.Children);
            Assert.Contains(productCategory121, productCategory1.Children);
            Assert.Contains(productCategory122, productCategory1.Children);

            Assert.AreEqual(3, productCategory2.Children.Count);
            Assert.Contains(productCategory12, productCategory2.Children);
            Assert.Contains(productCategory121, productCategory2.Children);
            Assert.Contains(productCategory122, productCategory2.Children);

            Assert.AreEqual(1, productCategory11.Children.Count);
            Assert.Contains(productCategory111, productCategory11.Children);

            Assert.AreEqual(2, productCategory12.Children.Count);
            Assert.Contains(productCategory121, productCategory12.Children);
            Assert.Contains(productCategory122, productCategory12.Children);

            Assert.IsFalse(productCategory111.ExistChildren);
            Assert.IsFalse(productCategory121.ExistChildren);
            Assert.IsFalse(productCategory122.ExistChildren);
        }
    }
}
