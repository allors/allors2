namespace Tests
{
    using Allors.Domain;

    using Xunit;

    [Collection("Server")]
    public class PushTests : ServerTest
    {
        [Fact]
        public async void CorrectUserAndPassword()
        {
            var administrator = new Users(this.Session).GetUser("administrator");
            await this.SignIn(administrator);
        }
    }
}