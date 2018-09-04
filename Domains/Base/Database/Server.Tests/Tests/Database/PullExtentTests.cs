namespace Tests
{
    using System;
    using System.Linq;

    using Allors.Data.Protocol;
    using Allors.Domain;
    using Allors.Meta;
    using Allors.Server.Protocol.Pull;

    using Xunit;

    [Collection("Server")]
    public class PullExtentTests : ServerTest
    {
        [Fact]
        public async void WithoutResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var extent = new Allors.Data.Filter(M.Data.ObjectType);

            var pullRequest = new PullRequest
                                  {
                                      P = new[]
                                              {
                                                  new Pull
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
        public async void WithResult()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var data = new DataBuilder(this.Session).WithString("First").Build();

            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var extent = new Allors.Data.Filter(M.Data.ObjectType);

            var pullRequest = new PullRequest
                                  {
                                      P = new[]
                                              {
                                                  new Pull
                                                      {
                                                          Extent = extent.Save(),
                                                          Fetches = new[]
                                                                        {
                                                                            new Fetch
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