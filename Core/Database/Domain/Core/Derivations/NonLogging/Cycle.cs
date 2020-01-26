// <copyright file="Generation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System.Collections.Generic;
    using Object = Domain.Object;

    public class Cycle : ICycle
    {
        private Properties properties;

        internal Cycle(Derivation derivation)
        {
            this.Derivation = derivation;
            this.ChangeSet = new AccumulatedChangeSet();
        }

        IAccumulatedChangeSet ICycle.ChangeSet => this.ChangeSet;

        public AccumulatedChangeSet ChangeSet { get; }

        IIteration ICycle.Iteration => this.Iteration;

        IDerivation ICycle.Derivation => this.Derivation;

        internal Iteration Iteration { get; set; }

        internal Derivation Derivation { get; }

        public object this[string name]
        {
            get => this.properties?.Get(name);

            set
            {
                this.properties ??= new Properties();
                this.properties.Set(name, value);
            }
        }

        internal List<Object> Execute(Object[] marked = null)
        {
            try
            {
                var postDeriveObjects = new List<Object>();
                var previousCount = postDeriveObjects.Count;

                this.Iteration = new Iteration(this);
                this.Iteration.Execute(postDeriveObjects, marked);

                while (postDeriveObjects.Count != previousCount)
                {
                    previousCount = postDeriveObjects.Count;

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
