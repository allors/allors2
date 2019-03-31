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
                        var context = new Context(this.Database, this.Workspace);

                        var exceptionThrown = false;
                        try
                        {
                            await context.Load(new { step = 0 }, "ThisIsWrong");
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
                       var context = new Context(this.Database, this.Workspace);

                       var result = await context.Load(new { step = 1 }, "TestUnitSamples");

                       var notHere = result.Objects["NotHere"];
                   });
        }

        [Fact]
        public void NonExistingContextCollection()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context(this.Database, this.Workspace);

                       var result = await context.Load(new { step = 1 }, "TestUnitSamples");

                       var notHere = result.Collections["NotHere"];
                   });
        }

        [Fact]
        public void NonExistingContextValues()
        {
            AsyncContext.Run(
                   async () =>
                   {
                       var context = new Context(this.Database, this.Workspace);

                       var result = await context.Load(new { step = 1 });

                       var notHere = result.Values["NotHere"];
                   });
        }

    }
}