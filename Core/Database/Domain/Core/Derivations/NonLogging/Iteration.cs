// <copyright file="Cycle.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;
    using System.Linq;

    public class Iteration : IIteration
    {
        private Dictionary<string, object> properties;

        public Iteration(Cycle cycle, ISet<Object> marked = null)
        {
            this.Cycle = cycle;
            this.MarkedBacklog = marked ?? new HashSet<Object>();
            this.ChangeSet = new AccumulatedChangeSet();
            this.Nodes = new DerivationNodes(this.Cycle.Derivation);
        }

        ICycle IIteration.Cycle => this.Cycle;

        public Cycle Cycle { get; }

        public ISet<Object> MarkedBacklog { get; private set; }

        IPreparation IIteration.Preparation => this.Preparation;

        public Preparation Preparation { get; set; }

        IAccumulatedChangeSet IDerive.ChangeSet => this.ChangeSet;

        public AccumulatedChangeSet ChangeSet { get; }

        public ISet<Object> Objects => this.Nodes.Scheduled;

        public DerivationNodes Nodes { get; }

        public object this[string name]
        {
            get
            {
                var lowerName = name.ToLowerInvariant();

                if (this.properties != null && this.properties.TryGetValue(lowerName, out var value))
                {
                    return value;
                }

                return null;
            }

            set
            {
                var lowerName = name.ToLowerInvariant();

                if (value == null)
                {
                    if (this.properties != null)
                    {
                        this.properties.Remove(lowerName);
                        if (this.properties.Count == 0)
                        {
                            this.properties = null;
                        }
                    }
                }
                else
                {
                    if (this.properties == null)
                    {
                        this.properties = new Dictionary<string, object>();
                    }

                    this.properties[lowerName] = value;
                }
            }
        }

        public void Schedule(Object @object) => this.Nodes.Schedule(@object);

        public void Mark(Object @object)
        {
            if (@object != null && !this.Nodes.Marked.Contains(@object))
            {
                this.Nodes.Marked.Add(@object);
                if (!this.Preparation.Objects.Contains(@object))
                {
                    this.MarkedBacklog.Add(@object);
                }
            }
        }

        public void Mark(params Object[] objects)
        {
            foreach (var @object in objects)
            {
                this.Mark(@object);
            }
        }

        public bool IsMarked(Object @object) => this.Nodes.Marked.Contains(@object);

        public void Execute(List<Object> postDeriveObjects)
        {
            try
            {
                this.Preparation = new Preparation(this, this.MarkedBacklog);
                this.MarkedBacklog = new HashSet<Object>();
                this.Preparation.Execute();

                while (this.Preparation.Objects.Any() || this.MarkedBacklog.Count > 0)
                {
                    this.Preparation = new Preparation(this, this.MarkedBacklog);
                    this.MarkedBacklog = new HashSet<Object>();
                    this.Preparation.Execute();
                }

                this.Nodes.Derive(postDeriveObjects);

                this.Cycle.DerivedObjects.UnionWith(postDeriveObjects);
                this.Cycle.Derivation.DerivedObjects.UnionWith(postDeriveObjects);
            }
            finally
            {
                this.Preparation = null;
            }
        }

        public void AddDependency(Object dependent, Object dependee)
        {
            if (dependent != null && dependee != null && !dependent.Equals(dependee))
            {
                this.Nodes.AddDependency(dependent, dependee);
            }
        }
    }
}
