// <copyright file="UnitTest.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Object.SqlClient
{
    using Allors.Domain;
    using Xunit;

    public abstract class UnitTest : Adapters.UnitTest
    {
        [Fact]
        public override void AllorsDecimal()
        {
            foreach (var init in this.Inits)
            {
                init();

                // Positive
                {
                    var values = C1.Create(this.Session);
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
                    var values = C1.Create(this.Session);
                    values.C1AllorsDecimal = -10.10m;
                    values.I1AllorsDecimal = -10.10m;
                    values.S1AllorsDecimal = -10.10m;

                    this.Session.Commit();

                    Assert.Equal(-10.10m, values.C1AllorsDecimal);
                    Assert.Equal(-10.10m, values.I1AllorsDecimal);
                    Assert.Equal(-10.10m, values.S1AllorsDecimal);
                }

                // Zero
                {
                    var values = C1.Create(this.Session);
                    values.C1AllorsDecimal = 0m;
                    values.I1AllorsDecimal = 0m;
                    values.S1AllorsDecimal = 0m;

                    this.Session.Commit();

                    Assert.Equal(0m, values.C1AllorsDecimal);
                    Assert.Equal(0m, values.I1AllorsDecimal);
                    Assert.Equal(0m, values.S1AllorsDecimal);
                }

                // initial empty
                {
                    var values = C1.Create(this.Session);

                    decimal? value = null;

                    value = values.C1AllorsDecimal;
                    Assert.Null(value);

                    value = values.I1AllorsDecimal;
                    Assert.Null(value);

                    value = values.S1AllorsDecimal;
                    Assert.Null(value);

                    Assert.False(values.ExistC1AllorsDecimal);
                    Assert.False(values.ExistI1AllorsDecimal);
                    Assert.False(values.ExistS1AllorsDecimal);

                    this.Session.Commit();

                    value = values.C1AllorsDecimal;
                    Assert.Null(value);

                    value = values.I1AllorsDecimal;
                    Assert.Null(value);

                    value = values.S1AllorsDecimal;
                    Assert.Null(value);

                    Assert.False(values.ExistC1AllorsDecimal);
                    Assert.False(values.ExistI1AllorsDecimal);
                    Assert.False(values.ExistS1AllorsDecimal);
                }

                // reset empty
                {
                    var values = C1.Create(this.Session);
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

                    decimal? value = null;
                    value = values.C1AllorsDecimal;
                    Assert.Null(value);

                    value = values.I1AllorsDecimal;
                    Assert.Null(value);

                    value = values.S1AllorsDecimal;
                    Assert.Null(value);

                    Assert.False(values.ExistC1AllorsDecimal);
                    Assert.False(values.ExistI1AllorsDecimal);
                    Assert.False(values.ExistS1AllorsDecimal);

                    this.Session.Commit();

                    value = values.C1AllorsDecimal;
                    Assert.Null(value);

                    value = values.I1AllorsDecimal;
                    Assert.Null(value);

                    value = values.S1AllorsDecimal;
                    Assert.Null(value);

                    Assert.False(values.ExistC1AllorsDecimal);
                    Assert.False(values.ExistI1AllorsDecimal);
                    Assert.False(values.ExistS1AllorsDecimal);
                }
            }
        }
    }
}
