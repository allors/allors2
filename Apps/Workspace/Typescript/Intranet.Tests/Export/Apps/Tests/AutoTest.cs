namespace Tests.ApplicationTests
{
    using System;
    using System.Linq;
    using Xunit;

    [Collection("Test collection")]
    public class AutoTest : Test
    {
        public AutoTest(TestFixture fixture)
            : base(fixture)
        {
        }

        [Fact]
        public async void Navigate()
        {
            this.Login();

            var navigateTos = this.Sidenav.GetType()
                .GetMethods()
                .Where(v => v.Name.StartsWith("NavigateTo"))
                .ToArray();

            foreach (var navigatTo in navigateTos)
            {
                var page = navigatTo.Invoke(this.Sidenav, null);
            }
        }
    }
}
