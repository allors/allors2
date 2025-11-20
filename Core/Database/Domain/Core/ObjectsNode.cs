// <copyright file="ObjectsNode.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Collections.Generic;

    public class ObjectsNode : IEquatable<ObjectsNode>
    {
        private readonly IObjects objects;

        private bool visited;
        private ObjectsNode previousRoot;
        private HashSet<ObjectsNode> dependencies;

        public ObjectsNode(IObjects objects) => this.objects = objects;

        public void Execute(Action<IObjects> execute) => this.Execute(this, execute);

        public void AddDependency(ObjectsNode objectsNode)
        {
            if (this.dependencies == null)
            {
                this.dependencies = new HashSet<ObjectsNode>();
            }

            this.dependencies.Add(objectsNode);
        }

        public bool Equals(ObjectsNode other) => other != null && this.objects.Equals(other.objects);

        public override bool Equals(object obj) => this.Equals((ObjectsNode)obj);

        public override int GetHashCode() => this.objects.GetHashCode();

        public override string ToString() => this.objects.ToString();

        private void Execute(ObjectsNode currentRoot, Action<IObjects> execute)
        {
            if (this.visited)
            {
                if (currentRoot.Equals(this.previousRoot))
                {
                    throw new Exception("This populate has a cycle. (" + this.previousRoot + " -> " + this + ")");
                }

                return;
            }

            this.visited = true;
            this.previousRoot = currentRoot;

            if (this.dependencies != null)
            {
                foreach (var dependency in this.dependencies)
                {
                    dependency.Execute(currentRoot, execute);
                }
            }

            execute(this.objects);

            this.previousRoot = null;
        }
    }
}
