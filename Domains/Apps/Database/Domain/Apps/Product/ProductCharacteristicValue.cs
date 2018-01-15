// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProductCharacteristicValue.cs" company="Allors bvba">
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
    using System.Linq;

    using Meta;

    public partial class ProductCharacteristicValue
    {
        public void AppsOnPreDerive(ObjectOnPreDerive method)
        {
            var derivation = method.Derivation;

            if (!derivation.IsCreated(this) &&
                derivation.HasChangedRole(this, M.ProductCharacteristicValue.AssignedValue) &&
                this.ProductCharacteristic.ExistUnitOfMeasure)
            {
                foreach (ProductCharacteristicValue productCharacteristicValue in this.ProductCharacteristic.ProductCharacteristicValuesWhereProductCharacteristic)
                {
                    if (!Equals(productCharacteristicValue.Locale, this.Locale))
                    {
                        derivation.AddDependency(productCharacteristicValue, this);
                    }
                }
            }
        }

        public void AppsOnDerive(ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var defaultLocale = this.strategy.Session.GetSingleton().DefaultLocale;

            if (this.ExistInventoryItemsWhereProductCharacteristicValue)
            {
                var valueInDefaultLocale = this.InventoryItemsWhereProductCharacteristicValue[0]
                    .ProductCharacteristicValues
                    .First(x => Equals(x.Locale, defaultLocale) && Equals(x.ProductCharacteristic, this.ProductCharacteristic));

                if (this.ProductCharacteristic.ExistUnitOfMeasure && valueInDefaultLocale != null &&
                    !Equals(valueInDefaultLocale.Locale, this.Locale))
                {
                    this.DerivedValue = valueInDefaultLocale.AssignedValue;
                    this.RemoveAssignedValue();
                }
            }
        }
    }
}