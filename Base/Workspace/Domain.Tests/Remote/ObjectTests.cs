namespace Tests.Remote
{
    using System;
    using System.Net.Http;

    using Allors.Workspace.Client;
    using Allors.Workspace.Domain;

    using Nito.AsyncEx;

    using NUnit.Framework;

    using Should;

    public class ObjectTests : Test
    {
        [Test]
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

                        exceptionThrown.ShouldBeTrue();
                    });
        }

        [Test]
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

        [Test]
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

        [Test]
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