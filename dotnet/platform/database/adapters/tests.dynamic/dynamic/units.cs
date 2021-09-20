// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Units.cs" company="Allors bvba">
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

    public class Units
    {
        public byte[] Binary;
        public bool Boolean;
        public DateTime Date;
        public DateTime DateTime;
        public decimal Decimal;
        public double Float;
        public static Units Dummy = new Units(false);
        public int Integer;
        public long Long;
        public string String;
        public Guid Unique;

        private static readonly TestValueGenerator Generator = new TestValueGenerator();

        public Units(Units different)
        {
            this.Boolean = !different.Boolean;
        }

        public Units(bool generate)
        {
            if (generate)
            {
                this.String = Generator.GenerateString(10);
                this.Decimal = Generator.GenerateDecimal();
                this.Integer = Generator.GenerateInteger();
                this.Float = Generator.GenerateFloat();
                this.Boolean = Generator.GenerateBoolean();
                this.DateTime = Generator.GenerateDateTime();
                this.Unique = Generator.GenerateUnique();
                this.Binary = Generator.GenerateBinary(10);
            }
        }
    }
}
