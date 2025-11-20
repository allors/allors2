// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestValueGenerator.cs" company="Allors bv">
//   Copyright Allors bv.
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
    using System.Globalization;
    using System.Text;

    public class TestValueGenerator
    {
        private readonly Random random = new Random(DateTime.Now.Millisecond);

        public byte[] GenerateBinary(int size)
        {
            var binary = new byte[size];
            for (var i = 0; i < binary.Length; i++)
            {
                binary[i] = (byte)this.random.Next(byte.MinValue, byte.MaxValue);
            }

            return binary;
        }

        public bool GenerateBoolean()
        {
            if (this.random.Next(int.MinValue, int.MaxValue) > 0)
            {
                return true;
            }

            return false;
        }

        public DateTime GenerateDateTime()
        {
            var ticks = (long)(DateTime.MaxValue.Ticks * this.GeneratePercentage());
            var dateTime = new DateTime(ticks);
            dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, dateTime.Millisecond, DateTimeKind.Utc); // not interested in nanoseconds

            // SQL hack
            if (dateTime.Year < 1800)
            {
                var adjustment = 1800 - dateTime.Year;
                dateTime = dateTime.AddYears(adjustment);
            }

            if (dateTime.Year > 3000)
            {
                var adjustment = dateTime.Year - 3000;
                dateTime = dateTime.AddYears(-adjustment);
            }

            return dateTime;
        }

        public decimal GenerateDecimal()
        {
            var unitBuffer = new StringBuilder();

            unitBuffer.Append(this.GenerateBoolean() ? "+" : "-");
            for (var i = 0; i < 8; i++)
            {
                unitBuffer.Append(this.random.Next(9));
            }

            unitBuffer.Append(NumberFormatInfo.CurrentInfo.CurrencyDecimalSeparator);
            for (var i = 0; i < 2; i++)
            {
                unitBuffer.Append(this.random.Next(9));
            }

            var unit = decimal.Parse(unitBuffer.ToString());
            return unit;
        }

        public double GenerateFloat()
        {
            return (double)this.random.Next(int.MinValue, int.MaxValue) / this.random.Next(int.MinValue, int.MaxValue);
        }

        public int GenerateInteger()
        {
            return this.random.Next(int.MinValue, int.MaxValue);
        }

        public double GeneratePercentage()
        {
            return (double)this.random.Next(0, int.MaxValue) / int.MaxValue;
        }

        public string GenerateString(int size)
        {
            var stringBuilder = new StringBuilder(size);
            for (var i = 0; i < size; i++)
            {
                var ch = (char)(this.random.Next(0x01F) + 0x020);
                stringBuilder.Append(ch);
            }

            return stringBuilder.ToString();
        }

        public Guid GenerateUnique()
        {
            return Guid.NewGuid();
        }
    }
}
