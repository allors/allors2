namespace Allors.Serialization
{
    using System.Collections.Generic;

    public interface IObjectData : IEnumerable<IRoleData>
    {
        long Id { get; }

        long Version { get; }
    }
}
