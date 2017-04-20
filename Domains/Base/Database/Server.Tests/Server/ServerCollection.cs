namespace Tests.Remote
{
    using Xunit;

    [CollectionDefinition("Server")]
    public class ServerCollection : ICollectionFixture<ServerFixture>
    {
    }
}