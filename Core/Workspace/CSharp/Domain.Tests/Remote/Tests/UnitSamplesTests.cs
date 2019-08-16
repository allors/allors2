namespace Tests.Remote
{
    using System;

    using Allors.Workspace.Client;
    using Allors.Workspace.Domain;

    using Nito.AsyncEx;

    using Xunit;

    public class UnitSamplesTests : RemoteTest
    {
        [Fact]
        public void Null()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var context = new Context(this.Database, this.Workspace);

                        var result = await context.Load(new { step = 0 }, "TestUnitSamples");

                        result.GetObject("unitSample", out UnitSample unitSample);

                        Assert.False(unitSample.ExistAllorsBinary);
                        Assert.False(unitSample.ExistAllorsBoolean);
                        Assert.False(unitSample.ExistAllorsDateTime);
                        Assert.False(unitSample.ExistAllorsDecimal);
                        Assert.False(unitSample.ExistAllorsDouble);
                        Assert.False(unitSample.ExistAllorsInteger);
                        Assert.False(unitSample.ExistAllorsString);
                        Assert.False(unitSample.ExistAllorsUnique);
                    });
        }

        [Fact]
        public void Values()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context(this.Database, this.Workspace);

                       var result = await context.Load(new { step = 1 }, "TestUnitSamples");

                       result.GetObject("unitSample", out UnitSample unitSample);

                       Assert.True(unitSample.ExistAllorsBinary);
                       Assert.True(unitSample.ExistAllorsBoolean);
                       Assert.True(unitSample.ExistAllorsDateTime);
                       Assert.True(unitSample.ExistAllorsDecimal);
                       Assert.True(unitSample.ExistAllorsDouble);
                       Assert.True(unitSample.ExistAllorsInteger);
                       Assert.True(unitSample.ExistAllorsString);
                       Assert.True(unitSample.ExistAllorsUnique);

                       Assert.Equal(new byte[] { 1, 2, 3 }, unitSample.AllorsBinary);
                       Assert.True(unitSample.AllorsBoolean);
                       Assert.Equal(new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc), unitSample.AllorsDateTime);
                       Assert.Equal(12.34m, unitSample.AllorsDecimal);
                       Assert.Equal(123d, unitSample.AllorsDouble);
                       Assert.Equal(1000, unitSample.AllorsInteger);
                       Assert.Equal("a string", unitSample.AllorsString);
                       Assert.Equal(new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF"), unitSample.AllorsUnique);
                   });
        }
    }
}
