//------------------------------------------------------------------------------------------------- 
// <copyright file="Default.cs" company="Allors bvba">
// Copyright 2002-2010 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Default type.</summary>
//------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using System;

    using Domain;

    using Xunit;

    public abstract class UnitTest : Adapters.UnitTest
    {
        protected override bool UseFloatMaximum => false;

        protected override bool UseFloatMinimum => false;

        [Fact]
        public override void AllorsDecimal()
        {
            // Positive
            {
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 10.10m;
                values.I1AllorsDecimal = 10.10m;
                values.S1AllorsDecimal = 10.10m;

                this.Session.Commit();

                Assert.Equal(10.10m, values.C1AllorsDecimal);
                Assert.Equal(10.10m, values.I1AllorsDecimal);
                Assert.Equal(10.10m, values.S1AllorsDecimal);
            }

            // Negative
            {
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = -10.10m;
                values.I1AllorsDecimal = -10.10m;
                values.S1AllorsDecimal = -10.10m;

                this.Session.Commit();
                ;

                Assert.Equal(-10.10m, values.C1AllorsDecimal);
                Assert.Equal(-10.10m, values.I1AllorsDecimal);
                Assert.Equal(-10.10m, values.S1AllorsDecimal);
            }

            // Zero
            {
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 0m;
                values.I1AllorsDecimal = 0m;
                values.S1AllorsDecimal = 0m;

                this.Session.Commit();
                ;

                Assert.Equal(0m, values.C1AllorsDecimal);
                Assert.Equal(0m, values.I1AllorsDecimal);
                Assert.Equal(0m, values.S1AllorsDecimal);
            }

            // initial empty
            {
                C1 values = C1.Create(this.Session);

                decimal? value = -1;
                var exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                Assert.Equal(-1, value);

                Assert.False(values.ExistC1AllorsDecimal);
                Assert.False(values.ExistI1AllorsDecimal);
                Assert.False(values.ExistS1AllorsDecimal);

                this.Session.Commit();

                value = -1;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                Assert.Equal(-1, value);

                Assert.False(values.ExistC1AllorsDecimal);
                Assert.False(values.ExistI1AllorsDecimal);
                Assert.False(values.ExistS1AllorsDecimal);
            }

            // reset empty
            {
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 10.10m;
                values.I1AllorsDecimal = 10.10m;
                values.S1AllorsDecimal = 10.10m;

                this.Session.Commit();

                Assert.True(values.ExistC1AllorsDecimal);
                Assert.True(values.ExistI1AllorsDecimal);
                Assert.True(values.ExistS1AllorsDecimal);

                values.RemoveC1AllorsDecimal();
                values.RemoveI1AllorsDecimal();
                values.RemoveS1AllorsDecimal();

                decimal? value = -1;
                var exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                Assert.Equal(-1, value);

                Assert.False(values.ExistC1AllorsDecimal);
                Assert.False(values.ExistI1AllorsDecimal);
                Assert.False(values.ExistS1AllorsDecimal);

                this.Session.Commit();
                value = -1;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch { exceptionThrown = true; }
                Assert.True(exceptionThrown);
                Assert.Equal(-1, value);

                Assert.False(values.ExistC1AllorsDecimal);
                Assert.False(values.ExistI1AllorsDecimal);
                Assert.False(values.ExistS1AllorsDecimal);
            }
        }
    }
}
