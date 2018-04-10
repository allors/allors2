namespace Tests
{
    using System;
    using System.Linq;

    using Allors.Domain;
    using Allors.Server;

    using Xunit;

    [Collection("Server")]
    public class PullTests : ServerTest
    {
        [Fact]
        public async void FetchExistingObject()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var organisation = new OrganisationBuilder(this.Session).Build();
            this.Session.Commit();

            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var pullRequest = new PullRequest
                                  {
                                      F = new[]
                                              {
                                                  new PullRequestFetch
                                                      {
                                                          Id = organisation.Id.ToString(),
                                                          Name = "organisation"
                                                      },
                                              }
                                  };
            var response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);
            
            Assert.Single(pullResponse.NamedObjects);
            var pullNameObject = pullResponse.NamedObjects.First();

            Assert.Equal("organisation", pullNameObject.Key);
            Assert.Equal(organisation.Id.ToString(), pullNameObject.Value);

            Assert.Single(pullResponse.Objects);
            var pullObject = pullResponse.Objects.First();
            Assert.Equal(organisation.Id.ToString(), pullObject[0]);
            Assert.Equal(organisation.Strategy.ObjectVersion.ToString(), pullObject[1]);
        }

        [Fact]
        public async void DeletedObject()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);

            var organisation = new OrganisationBuilder(this.Session).Build();
            this.Session.Commit();
            
            var uri = new Uri(@"Pull/Pull", UriKind.Relative);

            var pullRequest = new PullRequest
                                  {
                                      F = new[]
                                              {
                                                  new PullRequestFetch
                                                      {
                                                          Id = organisation.Id.ToString(),
                                                          Name = "organisation"
                                                      },
                                              }
                                  };
            var response = await this.PostAsJsonAsync(uri, pullRequest);

            organisation.Strategy.Delete();

            this.Session.Commit();

            response = await this.PostAsJsonAsync(uri, pullRequest);
            var pullResponse = await this.ReadAsAsync<PullResponse>(response);
            
            Assert.Empty(pullResponse.Objects);
        }
    }
}