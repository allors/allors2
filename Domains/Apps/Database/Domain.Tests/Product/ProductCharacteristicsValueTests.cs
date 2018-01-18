//------------------------------------------------------------------------------------------------- 
// <copyright file="ProductCharacteristicsValueTests.cs" company="Allors bvba">
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
    using Xunit;
    
    public class ProductCharacteristicsValueTests : DomainTest
    {
        [Fact]
        public void GivenProductCharacteristicValueNotDefaultLocaleWithUom_WhenDeriving_ThenDerivedValueIsSetFromDefaultLocale()
        {
            var defaultLocale = this.Session.GetSingleton().DefaultLocale;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            this.Session.GetSingleton().AddLocale(dutchLocale);

            var characteristic = new ProductCharacteristicBuilder(this.Session)
                .WithName("category")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Meter)
                .Build();

            var englishValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithAssignedValue("100")
                .WithLocale(defaultLocale)
                .Build();

            this.Session.Derive();

            var ducthValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithLocale(dutchLocale)
                .Build();

            this.Session.Derive();

            Assert.Equal(englishValue.AssignedValue, ducthValue.DerivedValue);
        }

        [Fact]
        public void GivenProductCharacteristicValueDefaultLocaleWithUom_WhenDeriving_ThenDerivedValueIsNotSet()
        {
            var defaultLocale = this.Session.GetSingleton().DefaultLocale;

            var characteristic = new ProductCharacteristicBuilder(this.Session)
                .WithName("category")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Meter)
                .Build();

            var englishValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithAssignedValue("100")
                .WithLocale(defaultLocale)
                .Build();

            this.Session.Derive();

            Assert.Null(englishValue.DerivedValue);
        }

        [Fact]
        public void GivenProductCharacteristicValueWithoutUom_WhenDeriving_ThenDerivedValueIsNotSet()
        {
            var defaultLocale = this.Session.GetSingleton().DefaultLocale;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            this.Session.GetSingleton().AddLocale(dutchLocale);

            var characteristic = new ProductCharacteristicBuilder(this.Session)
                .WithName("category")
                .Build();

            var englishValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithAssignedValue("english text")
                .WithLocale(defaultLocale)
                .Build();

            this.Session.Derive();

            var ducthValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithAssignedValue("nederlandse tekst")
                .WithLocale(dutchLocale)
                .Build();

            this.Session.Derive();

            Assert.Equal("nederlandse tekst", ducthValue.AssignedValue);
        }

        [Fact]
        public void GivenProductCharacteristicValueNotDefaultLocaleWithUom_WhenValueChanges_ThenDerivedValueIsSetFromDefaultLocale()
        {
            var defaultLocale = this.Session.GetSingleton().DefaultLocale;
            var dutchLocale = new Locales(this.Session).DutchNetherlands;

            this.Session.GetSingleton().AddLocale(dutchLocale);

            var characteristic = new ProductCharacteristicBuilder(this.Session)
                .WithName("category")
                .WithUnitOfMeasure(new UnitsOfMeasure(this.Session).Meter)
                .Build();

            var englishValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithAssignedValue("100")
                .WithLocale(defaultLocale)
                .Build();

            this.Session.Derive();

            var ducthValue = new ProductCharacteristicValueBuilder(this.Session)
                .WithProductCharacteristic(characteristic)
                .WithLocale(dutchLocale)
                .Build();

            this.Session.Derive();

            Assert.Equal(englishValue.AssignedValue, ducthValue.DerivedValue);

            englishValue.AssignedValue = "200";

            this.Session.Derive();

            Assert.Equal(englishValue.AssignedValue, ducthValue.DerivedValue);
        }
    }
}
