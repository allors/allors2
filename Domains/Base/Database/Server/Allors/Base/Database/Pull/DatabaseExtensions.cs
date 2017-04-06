using Allors;

namespace Allors.Server
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public static class DatabaseExtensions
    {
        private const string FullTreeKey = "Allors.Strategy.FullTree";
         
        public static Tree FullTree(this IDatabase @this, IComposite composite)
        {

            var defaultPullTreeByClass = (IDictionary<IComposite, Tree>)@this[FullTreeKey];
            if (defaultPullTreeByClass == null)
            {
                defaultPullTreeByClass = new ConcurrentDictionary<IComposite, Tree>();
                @this[FullTreeKey] = defaultPullTreeByClass;
            }

            Tree defaultPullTree;
            if (!defaultPullTreeByClass.TryGetValue(composite, out defaultPullTree))
            {
                defaultPullTree = new Tree(composite);
                foreach (var compositeRoleType in composite.RoleTypes.Where(v => v.ObjectType.IsComposite && ((RoleType)v).Workspace))
                {
                    defaultPullTree.Add(compositeRoleType);
                }

                defaultPullTreeByClass[composite] = defaultPullTree;
            }

            return defaultPullTree;
        }
    }
}