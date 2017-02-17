// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsNode.cs" company="Allors bvba">
//   Copyright 2002-2016 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

        public ObjectsNode(IObjects objects)
        {
            this.objects = objects;
        }

        public void Execute(Action<IObjects> execute)
        {
            this.Execute(this, execute);
        }

        public void AddDependency(ObjectsNode objectsNode)
        {
            if (this.dependencies == null)
            {
                this.dependencies = new HashSet<ObjectsNode>();
            }

            this.dependencies.Add(objectsNode);
        }

        public bool Equals(ObjectsNode other)
        {
            return other != null && this.objects.Equals(other.objects);
        }

        public override bool Equals(object obj)
        {
            return this.Equals((ObjectsNode)obj);
        }

        public override int GetHashCode()
        {
            return this.objects.GetHashCode();
        }

        public override string ToString()
        {
            return this.objects.ToString();
        }

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