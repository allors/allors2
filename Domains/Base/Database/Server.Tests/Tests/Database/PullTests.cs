namespace Tests
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Meta;
    using Allors.Protocol.Remote.Pull;

    using Xunit;

    [Collection("Server")]
    public class PullTests : ServerTest
    {
        [Fact]
        public async void ExtentWithoutResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var extent = new Allors.Data.Extent(M.Data.ObjectType);

            var pullRequest = new PullRequest
                                  {
                                      E = new[]
                                              {
                                                  new PullExtent
                                                      {
                                                          Extent = extent.Save(),
                                                      },
                                              }
                                  };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Datas"];

            Assert.Single(organisations);

            var dataId = organisations.First();

            Assert.Equal(data.Id.ToString(), dataId);
        }

        [Fact]
        public async void ExtentWithResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var extent = new Allors.Data.Extent(M.Data.ObjectType);

            var pullRequest = new PullRequest
                                  {
                                      E = new[]
                                              {
                                                  new PullExtent
                                                      {
                                                          Extent = extent.Save(),
                                                          Results = new[]
                                                                        {
                                                                            new PullResult
                                                                                {
                                                                                    Name = "Datas"
                                                                                }, 
                                                                        }
                                                      },
                                              }
                                  };

            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);

            var organisations = pullResponse.NamedCollections["Datas"];

            Assert.Single(organisations);

            var dataId = organisations.First();

            Assert.Equal(data.Id.ToString(), dataId);
        }
    }
}