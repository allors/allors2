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
                        var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                        await context.Load(new { step = 0 });

                        var unitSample = (UnitSample)context.Objects["unitSample"];

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
                       var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                       await context.Load(new { step = 1 });

                       var unitSample = (UnitSample)context.Objects["unitSample"];

                       Assert.True(unitSample.ExistAllorsBinary);
                       Assert.True(unitSample.ExistAllorsBoolean);
                       Assert.True(unitSample.ExistAllorsDateTime);
                       Assert.True(unitSample.ExistAllorsDecimal);
                       Assert.True(unitSample.ExistAllorsDouble);
                       Assert.True(unitSample.ExistAllorsInteger);
                       Assert.True(unitSample.ExistAllorsString);
                       Assert.True(unitSample.ExistAllorsUnique);
                       
                       Assert.Equal(unitSample.AllorsBinary, new byte[] { 1, 2, 3 });
                       Assert.Equal(unitSample.AllorsBoolean, true);
                       Assert.Equal(unitSample.AllorsDateTime, new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc));
                       Assert.Equal(unitSample.AllorsDecimal, 12.34m);
                       Assert.Equal(unitSample.AllorsDouble, 123d);
                       Assert.Equal(unitSample.AllorsInteger, 1000);
                       Assert.Equal(unitSample.AllorsString, "a string");
                       Assert.Equal(unitSample.AllorsUnique, new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF"));
                   });
        }
    }
}