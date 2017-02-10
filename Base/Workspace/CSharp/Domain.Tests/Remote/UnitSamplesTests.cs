namespace Tests.Remote
{
    using System;

    using Allors.Workspace.Client;
    using Allors.Workspace.Domain;

    using Nito.AsyncEx;

    using NUnit.Framework;

    using Should;

    public class UnitSamplesTests : Test
    {
        [Test]
        public void Null()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                        await context.Load(new { step = 0 });

                        var unitSample = (UnitSample)context.Objects["unitSample"];

                        unitSample.ExistAllorsBinary.ShouldBeFalse();
                        unitSample.ExistAllorsBoolean.ShouldBeFalse();
                        unitSample.ExistAllorsDateTime.ShouldBeFalse();
                        unitSample.ExistAllorsDecimal.ShouldBeFalse();
                        unitSample.ExistAllorsDouble.ShouldBeFalse();
                        unitSample.ExistAllorsInteger.ShouldBeFalse();
                        unitSample.ExistAllorsString.ShouldBeFalse();
                        unitSample.ExistAllorsUnique.ShouldBeFalse();
                    });
        }

        [Test]
        public void Values()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                       await context.Load(new { step = 1 });

                       var unitSample = (UnitSample)context.Objects["unitSample"];

                       unitSample.ExistAllorsBinary.ShouldBeTrue();
                       unitSample.ExistAllorsBoolean.ShouldBeTrue();
                       unitSample.ExistAllorsDateTime.ShouldBeTrue();
                       unitSample.ExistAllorsDecimal.ShouldBeTrue();
                       unitSample.ExistAllorsDouble.ShouldBeTrue();
                       unitSample.ExistAllorsInteger.ShouldBeTrue();
                       unitSample.ExistAllorsString.ShouldBeTrue();
                       unitSample.ExistAllorsUnique.ShouldBeTrue();
                       
                       unitSample.AllorsBinary.ShouldEqual(new byte[] { 1, 2, 3 });
                       unitSample.AllorsBoolean.ShouldEqual(true);
                       unitSample.AllorsDateTime.ShouldEqual(new DateTime(1973, 3, 27, 0, 0, 0, DateTimeKind.Utc));
                       unitSample.AllorsDecimal.ShouldEqual(12.34m);
                       unitSample.AllorsDouble.ShouldEqual(123d);
                       unitSample.AllorsInteger.ShouldEqual(1000);
                       unitSample.AllorsString.ShouldEqual("a string");
                       unitSample.AllorsUnique.ShouldEqual(new Guid("2946CF37-71BE-4681-8FE6-D0024D59BEFF"));
                   });
        }
    }
}