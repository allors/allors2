// <copyright file="Generation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;
    using System.Linq;
    using Database.Adapters;
    using Object = Domain.Object;

    public class Cycle : ICycle
    {
        private Dictionary<string, object> properties;

        internal Cycle(Derivation derivation, ISet<Object> marked = null)
        {
            this.Derivation = derivation;
            this.Marked = marked;
            this.ChangeSet = new AccumulatedChangeSet();
        }

        IAccumulatedChangeSet ICycle.ChangeSet => this.ChangeSet;

        public AccumulatedChangeSet ChangeSet { get; }

        IIteration ICycle.Iteration => this.Iteration;

        internal Iteration Iteration { get; set; }

        IDerivation ICycle.Derivation => this.Derivation;

        internal Derivation Derivation { get; }

        internal ISet<Object> Marked { get; }

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

        internal List<Object> Execute()
        {
            try
            {
                var postDeriveObjects = new List<Object>();

                this.Iteration = new Iteration(this, this.Marked);
                this.Iteration.Execute(postDeriveObjects);

                while (this.Iteration.Objects.Any())
                {
                    this.Iteration = new Iteration(this);
                    this.Iteration.Execute(postDeriveObjects);
                }

                for (var i = postDeriveObjects.Count - 1; i >= 0; i--)
                {
                    var derivable = postDeriveObjects[i];
                    if (!derivable.Strategy.IsDeleted)
                    {
                        derivable.OnPostDerive(x => x.WithDerivation(this.Derivation));
                    }
                }

                return postDeriveObjects;
            }
            finally
            {
                this.Iteration = null;
            }
        }
    }
}
