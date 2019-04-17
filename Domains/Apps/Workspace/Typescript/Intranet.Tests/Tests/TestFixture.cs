namespace Tests
{
    using System;
    using System.Globalization;

    public class TestFixture : IDisposable
    {
        public TestFixture()
        {
            CultureInfo.CurrentCulture = new CultureInfo("nl-BE");
        }

        public void Dispose()
        {
        }
    }
}