// <copyright file="TreeNode.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Data
{
    using System.Collections.Generic;
    using System.Linq;

    using Allors.Meta;

    public class TreeNode
    {
        public TreeNode(IRoleType roleType, IComposite composite = null, TreeNodes nodes = null)
        {
            this.RoleType = roleType;
            this.Composite = composite ?? (roleType.ObjectType.IsComposite ? (IComposite)roleType.ObjectType : null);

            if (this.Composite != null)
            {
                this.Nodes = nodes ?? new TreeNodes(this.Composite);
            }
        }

        public IRoleType RoleType { get; }

        public IComposite Composite { get; }

        public TreeNodes Nodes { get; }

        public Protocol.Data.TreeNode Save() =>
            new Protocol.Data.TreeNode
            {
                RoleType = this.RoleType.Id,
                Nodes = this.Nodes.Select(v => v.Save()).ToArray(),
            };

        public void Resolve(IObject obj, HashSet<IObject> objects)
        {
            if (obj != null)
            {
                if (this.RoleType.ObjectType.IsComposite)
                {
                    if (this.RoleType.IsOne)
                    {
                        var role = obj.Strategy.GetCompositeRole(this.RoleType.RelationType);
                        if (role != null)
                        {
                            objects.Add(role);

                            foreach (var node in this.Nodes)
                            {
                                node.Resolve(role, objects);
                            }
                        }
                    }
                    else
                    {
                        var roles = obj.Strategy.GetCompositeRoles(this.RoleType.RelationType);
                        foreach (IObject role in roles)
                        {
                            objects.Add(role);

                            foreach (var node in this.Nodes)
                            {
                                node.Resolve(role, objects);
                            }
                        }
                    }
                }
            }
        }

        public void BuildPrefetchPolicy(PrefetchPolicyBuilder prefetchPolicyBuilder)
        {
            if (this.Nodes == null || this.Nodes.Count == 0)
            {
                prefetchPolicyBuilder.WithRule(this.RoleType);
            }
            else
            {
                var nestedPrefetchPolicyBuilder = new PrefetchPolicyBuilder();
                foreach (var node in this.Nodes)
                {
                    node.BuildPrefetchPolicy(nestedPrefetchPolicyBuilder);
                }

                var nestedPrefetchPolicy = nestedPrefetchPolicyBuilder.Build();
                prefetchPolicyBuilder.WithRule(this.RoleType, nestedPrefetchPolicy);
            }
        }
    }
}
