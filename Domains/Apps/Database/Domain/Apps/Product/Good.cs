// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Good.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
namespace Allors.Domain
{
    using System.Collections.Generic;
    using System.Linq;

    using Meta;

    public partial class Good
    {
        public void AppsOnBuild(ObjectOnBuild method)
        {
            if (!this.ExistSoldBy)
            {
                this.SoldBy = Singleton.Instance(this.Strategy.Session).DefaultInternalOrganisation;
            }
        }

        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            // TODO:
            if (this.ExistInventoryItemVersionedsWhereGood)
            {
                foreach (InventoryItemVersioned inventoryItem in this.InventoryItemVersionedsWhereGood)
                {
                    derivation.AddDependency(inventoryItem, this);
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = Singleton.Instance(this.strategy.Session).DefaultLocale;

            derivation.Validation.AssertAtLeastOne(this, M.Good.FinishedGood, M.Good.InventoryItemKind);
            derivation.Validation.AssertExistsAtMostOne(this, M.Good.FinishedGood, M.Good.InventoryItemKind);

            if (this.LocalisedNames.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Name = this.LocalisedNames.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.LocalisedDescriptions.Any(x => x.Locale.Equals(defaultLocale)))
            {
                this.Description = this.LocalisedDescriptions.First(x => x.Locale.Equals(defaultLocale)).Text;
            }

            if (this.ProductCategories.Count == 1 && !this.ExistPrimaryProductCategory)
            {
                this.PrimaryProductCategory = this.ProductCategories.First;
            }

            if (this.ExistPrimaryProductCategory && !this.ExistProductCategories)
            {
                this.AddProductCategory(this.PrimaryProductCategory);
            }

            this.DeriveVirtualProductPriceComponent();
            this.DeriveProductCategoriesExpanded();
            this.DeriveQuantityOnHand();
            this.DeriveAvailableToPromise();
            this.DeriveThumbnail();
        }

        public void DeriveVirtualProductPriceComponent()
        {
            if (!this.ExistProductWhereVariant)
            {
                this.RemoveVirtualProductPriceComponents();
            }

            if (this.ExistVariants)
            {
                this.RemoveVirtualProductPriceComponents();

                var priceComponents = this.PriceComponentsWhereProduct;

                foreach (Good product in this.Variants)
                {
                    foreach (PriceComponent priceComponent in priceComponents)
                    {
                        product.AddVirtualProductPriceComponent(priceComponent);

                        var basePrice = priceComponent as BasePrice;
                        if (basePrice != null && !priceComponent.ExistProductFeature)
                        {
                            product.AddToBasePrice(basePrice);
                        }
                    }
                }
            }
        }

        public void DeriveProductCategoriesExpanded()
        {
            this.RemoveProductCategoriesExpanded();

            if (this.ExistPrimaryProductCategory)
            {
                this.AddProductCategoriesExpanded(this.PrimaryProductCategory);
                foreach (ProductCategory ancestor in this.PrimaryProductCategory.Ancestors)
                {
                    this.AddProductCategoriesExpanded(ancestor);
                }
            }

            foreach (ProductCategory productCategory in this.ProductCategories)
            {
                this.AddProductCategoriesExpanded(productCategory);
                foreach (ProductCategory ancestor in productCategory.Ancestors)
                {
                    this.AddProductCategoriesExpanded(ancestor);
                }
            }
        }

        public void DeriveQuantityOnHand()
        {
            this.QuantityOnHand = 0;

            foreach (InventoryItemVersioned inventoryItem in this.InventoryItemVersionedsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.QuantityOnHand += nonSerialised.QuantityOnHand;
                }
            }
        }

        public void DeriveAvailableToPromise()
        {
            this.AvailableToPromise = 0;

            foreach (InventoryItemVersioned inventoryItem in this.InventoryItemVersionedsWhereGood)
            {
                if (inventoryItem is NonSerialisedInventoryItem)
                {
                    var nonSerialised = (NonSerialisedInventoryItem)inventoryItem;
                    this.AvailableToPromise += nonSerialised.AvailableToPromise;
                }
            }
        }

        public void DeriveThumbnail()
        {
            if (this.ExistPrimaryPhoto)
            {
                if (!this.ExistThumbnail)
                {
                    this.Thumbnail = new MediaBuilder(this.Strategy.Session).WithInData(this.PrimaryPhoto.InData).Build();
                }

                // TODO: Resize
                // var thumbNail = Media.CreateThumbnail(this.Photo.Bitmap, ThumbnailWidth);
                // this.Thumbnail.Load(thumbNail, ImageFormat.Jpeg);
            }
            else
            {
                this.RemoveThumbnail();
            }
        }
    }
}