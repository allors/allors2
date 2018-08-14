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

            if (!this.ExistReferenceNumberCounter)
            {
                this.ReferenceNumberCounter = new CounterBuilder(this.strategy.Session).WithUniqueId(Guid.NewGuid()).WithValue(0).Build();
            }
        }

        public string NextSkuNumber()
        {
            var skuNumber = this.SkuCounter.NextValue();
            return string.Concat(this.SkuPrefix, skuNumber);
        }

        public string NextReferenceNumber()
        {
            var referenceNumber = this.ReferenceNumberCounter.NextValue();
            return string.Concat(this.ReferenceNumberPrefix, referenceNumber);
        }
    }
}
