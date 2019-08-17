// <copyright file="ObjectsGraph.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Collections.Generic;

    public class ObjectsGraph
    {
        private readonly Dictionary<IObjects, ObjectsNode> objectsNodeByObjects;

        public ObjectsGraph() => this.objectsNodeByObjects = new Dictionary<IObjects, ObjectsNode>();

        public void Invoke(Action<IObjects> action)
        {
            foreach (var dictionaryEntry in this.objectsNodeByObjects)
            {
                var derivationNode = dictionaryEntry.Value;
                derivationNode.Execute(action);
            }
        }

        public ObjectsNode Add(IObjects objects)
        {
            if (!this.objectsNodeByObjects.TryGetValue(objects, out var objectsNode))
            {
                objectsNode = new ObjectsNode(objects);
                this.objectsNodeByObjects.Add(objects, objectsNode);
            }

            return objectsNode;
        }

        public void AddDependency(IObjects dependent, IObjects dependency)
        {
            var objectsNode = this.Add(dependent);
            var dependencyNode = this.Add(dependency);
            objectsNode.AddDependency(dependencyNode);
        }
    }
}
