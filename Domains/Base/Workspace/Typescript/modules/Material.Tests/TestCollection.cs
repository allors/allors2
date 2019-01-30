namespace Tests.Material
{
    using Xunit;

    [CollectionDefinition("Test collection")]
    public class TestCollection : ICollectionFixture<TestFixture>
    {
    }
}