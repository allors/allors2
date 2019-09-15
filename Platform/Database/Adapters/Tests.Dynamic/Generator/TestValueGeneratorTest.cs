// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValueGeneratorTest.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Adapters
{
    using System;
    using System.Linq;
    using Xunit;

    
    public class TestValueGeneratorTest
    {
        private readonly TestValueGenerator testValueGenerator = new TestValueGenerator();

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateBoolean()
        {
            bool value = this.testValueGenerator.GenerateBoolean();
            bool differentValueFound = false;

            for (int i = 0; i < 100; i++)
            {
                bool newValue = this.testValueGenerator.GenerateBoolean();
                if (newValue != value)
                {
                    differentValueFound = true;
                    break;
                }
            }

            Assert.True(differentValueFound);
        }
        
        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateDateTime()
        {
            DateTime value1 = this.testValueGenerator.GenerateDateTime();
            DateTime value2 = this.testValueGenerator.GenerateDateTime();

            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateDecimal()
        {
            decimal value1 = this.testValueGenerator.GenerateDecimal();
            decimal value2 = this.testValueGenerator.GenerateDecimal();

            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateFloat()
        {
            double value1 = this.testValueGenerator.GenerateFloat();
            double value2 = this.testValueGenerator.GenerateFloat();

            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateInteger()
        {
            int value1 = this.testValueGenerator.GenerateInteger();
            int value2 = this.testValueGenerator.GenerateInteger();

            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GeneratePercentage()
        {
            double value1 = this.testValueGenerator.GeneratePercentage();
            double value2 = this.testValueGenerator.GeneratePercentage();

            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateString()
        {
            string value1 = this.testValueGenerator.GenerateString(0);
            string value2 = this.testValueGenerator.GenerateString(0);

            Assert.Equal(0, value1.Count());
            Assert.Equal(0, value2.Count());
            Assert.Equal(value1, value2);

            value1 = this.testValueGenerator.GenerateString(1);
            value2 = this.testValueGenerator.GenerateString(1);

            Assert.Equal(1, value1.Count());
            Assert.Equal(1, value2.Count());
            Assert.NotEqual(value1, value2);

            value1 = this.testValueGenerator.GenerateString(100);
            value2 = this.testValueGenerator.GenerateString(100);

            Assert.Equal(100, value1.Count());
            Assert.Equal(100, value2.Count());
            Assert.NotEqual(value1, value2);
        }

        [Fact]
        [Trait("Category", "Dynamic")]
        public void GenerateUnique()
        {
            Guid value1 = this.testValueGenerator.GenerateUnique();
            Guid value2 = this.testValueGenerator.GenerateUnique();

            Assert.NotEqual(value1, value2);
        }
    }
}
