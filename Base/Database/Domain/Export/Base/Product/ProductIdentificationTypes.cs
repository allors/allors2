// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductIdentificationTypes.cs" company="Allors bvba">
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
    using System;

    public partial class ProductIdentificationTypes
    {
        private static readonly Guid SkuId = new Guid("2BFB8F67-B2E1-4730-AC3D-3F2AF39B7EAF");
        private static readonly Guid IsbnId = new Guid("BA6A724F-7E90-4025-BED6-C10884735BAB");
        private static readonly Guid UpcaId = new Guid("27B32B96-0DCA-4414-AC09-1638B7CA4651");
        private static readonly Guid UpceId = new Guid("9AEE7C62-7237-473C-869F-4CB014C90529");
        private static readonly Guid EanId = new Guid("B2F15A78-0728-4041-86B2-6AC4C0FA9C7D");
        private static readonly Guid ManufacturerId = new Guid("3C349265-1794-4403-ADCF-C7D957527607");
        private static readonly Guid PartId = new Guid("5735191A-CDC4-4563-96EF-DDDC7B969CA6");
        private static readonly Guid GoodId = new Guid("B640630D-A556-4526-A2E5-60A84AB0DB3F");

        private UniquelyIdentifiableSticky<ProductIdentificationType> cache;

        public ProductIdentificationType Sku => this.Cache[SkuId];

        public ProductIdentificationType Isbn => this.Cache[IsbnId];

        public ProductIdentificationType Upca => this.Cache[UpcaId];

        public ProductIdentificationType Upce => this.Cache[UpceId];

        public ProductIdentificationType Ean => this.Cache[EanId];

        public ProductIdentificationType Manufacturer => this.Cache[ManufacturerId];

        public ProductIdentificationType Part => this.Cache[PartId];

        public ProductIdentificationType Good => this.Cache[GoodId];

        private UniquelyIdentifiableSticky<ProductIdentificationType> Cache => this.cache ?? (this.cache = new UniquelyIdentifiableSticky<ProductIdentificationType>(this.Session));

        protected override void BaseSetup(Setup setup)
        {


            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("SKU")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("SKU").WithLocale(dutchLocale).Build())
                .WithUniqueId(SkuId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("ISBN")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("ISBN").WithLocale(dutchLocale).Build())
                .WithUniqueId(IsbnId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("UPCA")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("UPCA").WithLocale(dutchLocale).Build())
                .WithUniqueId(UpcaId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("UPCE")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("UPCE").WithLocale(dutchLocale).Build())
                .WithUniqueId(UpceId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("EAN")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("EAN").WithLocale(dutchLocale).Build())
                .WithUniqueId(EanId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("Manufacturer Id")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Fabrikant Id").WithLocale(dutchLocale).Build())
                .WithUniqueId(ManufacturerId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("Product Id")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Product Id").WithLocale(dutchLocale).Build())
                .WithUniqueId(GoodId)
                .WithIsActive(true)
                .Build();

            new ProductIdentificationTypeBuilder(this.Session)
                .WithName("Part Id")
                .WithLocalisedName(new LocalisedTextBuilder(this.Session).WithText("Part Id").WithLocale(dutchLocale).Build())
                .WithUniqueId(PartId)
                .WithIsActive(true)
                .Build();
        }
    }
}
