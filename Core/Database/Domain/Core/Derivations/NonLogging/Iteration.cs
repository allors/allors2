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
        private Properties properties;

        public Iteration(Cycle cycle)
        {
            this.Cycle = cycle;
            this.ChangeSet = new AccumulatedChangeSet();
            this.Graph = new Graph(this.Cycle);
        }

        ICycle IIteration.Cycle => this.Cycle;

        public Cycle Cycle { get; }

        public ISet<Object> MarkedBacklog { get; private set; }

        IPreparation IIteration.Preparation => this.Preparation;

        public Preparation Preparation { get; set; }

        IAccumulatedChangeSet IIteration.ChangeSet => this.ChangeSet;

        public AccumulatedChangeSet ChangeSet { get; }

        public Graph Graph { get; }

        public object this[string name]
        {
            get => this.properties?.Get(name);

            set
            {
                this.properties ??= new Properties();
                this.properties.Set(name, value);
            }
        }

        public void Schedule(Object @object) => this.Graph.Schedule(@object);

        public void Mark(Object @object)
        {
            if (@object != null && !this.Graph.IsMarked(@object))
            {
                this.Graph.Mark(@object);
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

        public bool IsMarked(Object @object) => this.Graph.IsMarked(@object);

        public void Execute(List<Object> postDeriveObjects, Object[] marked = null)
        {
            try
            {
                if (marked != null)
                {
                    this.Graph.Mark(marked);
                }

                this.Preparation = new Preparation(this, marked);
                this.MarkedBacklog = new HashSet<Object>();
                this.Preparation.Execute();

                while (this.Preparation.Objects.Any() || this.MarkedBacklog.Count > 0)
                {
                    this.Preparation = new Preparation(this, this.MarkedBacklog);
                    this.MarkedBacklog = new HashSet<Object>();
                    this.Preparation.Execute();
                }

                this.Graph.Derive(postDeriveObjects);

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
                this.Graph.AddDependency(dependent, dependee);
            }
        }
    }
}
