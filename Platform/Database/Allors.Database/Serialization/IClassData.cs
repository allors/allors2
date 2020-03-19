namespace Allors.Serialization
{
    using System.Collections.Generic;
    using Meta;

    public interface IClassData : IEnumerable<IObjectData>
    {
        IClass Class { get; }
    }
}
