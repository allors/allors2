namespace Tests
{
    using Xunit;

    [CollectionDefinition("Server")]
    public class ServerCollection : ICollectionFixture<ServerFixture>
    {
    }
}