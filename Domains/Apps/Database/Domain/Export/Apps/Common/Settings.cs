namespace Allors.Domain
{
    using System;

    public partial class Settings
    {
        public void AppsOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistSkuCounter)
            {
                this.SkuCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistSerialisedItemCounter)
            {
                this.SerialisedItemCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }

            if (!this.ExistProductNumberCounter)
            {
                this.ProductNumberCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }
        }

        public string NextSkuNumber()
        {
            var skuNumber = this.SkuCounter.NextValue();
            return string.Concat(this.SkuPrefix, skuNumber);
        }

        public string NextSerialisedItemNumber()
        {
            var serialisedItemNumber = this.SerialisedItemCounter.NextValue();
            return string.Concat(this.SerialisedItemPrefix, serialisedItemNumber);
        }

        public string NextProductNumber()
        {
            var productNumber = this.ProductNumberCounter.NextValue();
            return string.Concat(this.ProductNumberPrefix, productNumber);
        }

        public string NextPartNumber()
        {
            var partNumber = this.PartNumberCounter.NextValue();
            return string.Concat(this.ExistPartNumberPrefix ? this.PartNumberPrefix : string.Empty, partNumber);
        }
    }
}
