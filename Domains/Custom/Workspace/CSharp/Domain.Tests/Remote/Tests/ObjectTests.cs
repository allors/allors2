namespace Tests.Remote
{
    using System.Net.Http;

    using Allors.Workspace.Client;
    using Nito.AsyncEx;

    using Xunit;

    public class ObjectTests : RemoteTest
    {
        [Fact]
        public void NonExistingPullController()
        {
            AsyncContext.Run(
                async () =>
                    {
                        var context = new Context("ThisIsWrong", this.Database, this.Workspace);

                        var exceptionThrown = false;
                        try
                        {
                            await context.Load(new { step = 0 });
                        }
                        catch (HttpRequestException e)
                        {
                            exceptionThrown = true;
                        }

                        Assert.True(exceptionThrown);
                    });
        }

        [Fact]
        public void NonExistingContextObject()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                       await context.Load(new { step = 1 });

                       var notHere = context.Objects["NotHere"];
                   });
        }

        [Fact]
        public void NonExistingContextCollection()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                       await context.Load(new { step = 1 });

                       var notHere = context.Collections["NotHere"];
                   });
        }

        [Fact]
        public void NonExistingContextValues()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context("TestUnitSamples", this.Database, this.Workspace);

                       await context.Load(new { step = 1 });

                       var notHere = context.Values["NotHere"];
                   });
        }

    }
}