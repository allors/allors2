namespace Allors.Database.Adapters.Memory.IO
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Database.Adapters.Memory;
    using Allors.Meta;
    using Allors.Serialization;

    public class ClassData : IClassData
    {
        private readonly List<Strategy> objects;

        public ClassData(IObjectType @class, List<Strategy> objects)
        {
            this.objects = objects;
            this.Class = (IClass)@class;
        }

        public IClass Class { get; }

        public IEnumerator<IObjectData> GetEnumerator() => this.objects
            .Select(@object => new ObjectData(@object))
            .Cast<IObjectData>()
            .GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
