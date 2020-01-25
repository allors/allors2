// <copyright file="Preparation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;
    using System.Linq;
    using Antlr.Runtime.Misc;

    public class Preparation : IPreparation
    {
        public Preparation(Iteration iteration, ISet<Object> marked)
        {
            this.Iteration = iteration;
            var cycle = this.Iteration.Cycle;
            var derivation = cycle.Derivation;
            var session = derivation.Session;

            var changeSet = session.Checkpoint();

            derivation.ChangeSet.Add(changeSet);
            cycle.ChangeSet.Add(changeSet);
            iteration.ChangeSet.Add(changeSet);

            // Initialization
            if (changeSet.Created.Any())
            {
                var newObjects = derivation.Session.Instantiate(changeSet.Created);
                foreach (var newObject in newObjects)
                {
                    ((Object)newObject).OnInit();
                }
            }

            // ChangedObjects
            var changedObjectIds = new HashSet<long>(changeSet.Associations);
            changedObjectIds.UnionWith(changeSet.Roles);
            changedObjectIds.UnionWith(changeSet.Created);

            this.Objects = new HashSet<Object>(derivation.Session.Instantiate(changedObjectIds).Cast<Object>());
            this.Objects.ExceptWith(this.Iteration.Cycle.Derivation.DerivedObjects);

            if (marked != null)
            {
                this.Objects.UnionWith(marked);
                this.Iteration.Nodes.Marked.UnionWith(marked);
            }

            if (this.Objects.Contains(null))
            {

            }
        }

        public Iteration Iteration { get; }

        public ISet<Object> Objects { get; set; }

        public void Execute()
        {
            foreach (var @object in this.Objects)
            {
                if (!@object.Strategy.IsDeleted)
                {
                    @object.OnPreDerive(x => x.WithIteration(this.Iteration));
                }
            }
        }
    }
}
