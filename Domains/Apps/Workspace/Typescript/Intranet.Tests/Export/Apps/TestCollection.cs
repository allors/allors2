namespace Tests.Intranet
{
    using Xunit;

    [CollectionDefinition("Test collection")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}