// <copyright file="Cycle.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Default
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Object = Domain.Object;

    public class Iteration : IIteration
    {
        private Properties properties;

        public Iteration(Cycle cycle)
        {
            this.Cycle = cycle;
            this.ChangeSet = new AccumulatedChangeSet();
            this.Graph = new Graph(this.Cycle.Derivation);
        }

        ICycle IIteration.Cycle => this.Cycle;

        IPreparation IIteration.Preparation => this.Preparation;

        IAccumulatedChangeSet IIteration.ChangeSet => this.ChangeSet;

        internal Cycle Cycle { get; }

        internal ISet<Object> MarkedBacklog { get; private set; }

        internal Preparation Preparation { get; set; }

        internal AccumulatedChangeSet ChangeSet { get; }

        internal Graph Graph { get; }

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
                if (!this.Preparation.Objects.Contains(@object) || this.Preparation.PreDerived.Contains(@object))
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

        public void Execute(List<Object> postDeriveBacklog, Object[] marked = null)
        {
            try
            {
                var config = this.Cycle.Derivation.Config;
                var count = 1;

                if (marked != null)
                {
                    this.Graph.Mark(marked);
                }

                this.Preparation = new Preparation(this, marked);
                this.MarkedBacklog = new HashSet<Object>();
                this.Preparation.Execute();

                while (this.Preparation.Objects.Any() || this.MarkedBacklog.Count > 0)
                {
                    if (config.MaxPreparations != 0 && count++ > config.MaxPreparations)
                    {
                        throw new Exception("Maximum amount of preparations reached");
                    }

                    this.Preparation = new Preparation(this, this.MarkedBacklog);
                    this.MarkedBacklog = new HashSet<Object>();
                    this.Preparation.Execute();
                }

                this.Graph.Derive(postDeriveBacklog);

                this.Cycle.Derivation.DerivedObjects.UnionWith(postDeriveBacklog);
            }
            finally
            {
                this.Preparation = null;
            }
        }

        public void AddDependency(Object dependent, params Object[] dependencies) => this.Graph.AddDependency(dependent, dependencies);
    }
}
