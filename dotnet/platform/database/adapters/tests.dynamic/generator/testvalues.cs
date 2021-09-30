// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValues.cs" company="Allors bvba">
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

    public class TestValues
    {
        public bool Boolean = true;

        public bool[] Booleans = { true, false };

        public DateTime dateTime = Generator.GenerateDateTime();

        public DateTime[] DateTimes = {
                                          Generator.GenerateDateTime(),
                                          DateTime.UtcNow,
                                          DateTime.MinValue.ToUniversalTime().AddMilliseconds(1),
                                          DateTime.MaxValue.ToUniversalTime().AddMilliseconds(-1)
                                      };

        public decimal Decimal = Generator.GenerateDecimal();

        public decimal[] Decimals = {
                                        0,
                                        -1,
                                        1,
                                        Generator.GenerateDecimal(),
                                        +99999999.99m,
                                        -99999999.99m
                                    };

        public double[] Floats = {
                                      0,
                                      -1,
                                      1,
                                      Generator.GenerateFloat(),
                                      Double.MinValue,
                                      Double.MinValue + 1,
                                      Double.MaxValue,
                                      Double.MaxValue - 1
                                  };
        public double Float = Generator.GenerateFloat();

        public int Integer = Generator.GenerateInteger();

        public int[] Integers = {
                                    0,
                                    -1,
                                    1,
                                    Generator.GenerateInteger(),
                                    Int32.MinValue,
                                    Int32.MinValue + 1,
                                    int.MaxValue,
                                    int.MaxValue - 1
                                };

        public string String = Generator.GenerateString(100);

        public Guid Unique = Generator.GenerateUnique();

        public Guid[] Uniques = {
                                    Guid.Empty,
                                    Generator.GenerateUnique()
                                };

        private static readonly TestValueGenerator Generator = new TestValueGenerator();
    }
}
