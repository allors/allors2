// <copyright file="Generation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations.Persistent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Object = Domain.Object;

    public class Cycle : ICycle
    {
        private Properties properties;

        internal Cycle(Derivation derivation)
        {
            this.Derivation = derivation;
            this.ChangeSet = new AccumulatedChangeSet();

            this.Iterations = new Iteration[0];
        }

        IAccumulatedChangeSet ICycle.ChangeSet => this.ChangeSet;

        IIteration ICycle.Iteration => this.Iteration;

        IDerivation ICycle.Derivation => this.Derivation;

        internal AccumulatedChangeSet ChangeSet { get; }

        internal Iteration[] Iterations { get; set; }

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
                var config = this.Derivation.Config;
                var count = 1;

                var postDeriveBacklog = new List<Object>();
                var previousCount = postDeriveBacklog.Count;

                this.Iteration = new Iteration(this);
                this.Iterations = this.Iterations.Append(this.Iteration).ToArray();
                this.Iteration.Execute(postDeriveBacklog, marked);

                while (postDeriveBacklog.Count != previousCount)
                {
                    if (config.MaxIterations != 0 && count++ > config.MaxIterations)
                    {
                        throw new Exception("Maximum amount of iterations reached");
                    }

                    previousCount = postDeriveBacklog.Count;

                    this.Iteration = new Iteration(this);
                    this.Iterations = this.Iterations.Append(this.Iteration).ToArray();
                    this.Iteration.Execute(postDeriveBacklog);
                }

                var postDerived = new HashSet<Object>();
                for (var i = postDeriveBacklog.Count - 1; i >= 0; i--)
                {
                    var @object = postDeriveBacklog[i];
                    if (!postDerived.Contains(@object) && !@object.Strategy.IsDeleted)
                    {
                        @object.OnPostDerive(x => x.WithDerivation(this.Derivation));
                    }

                    postDerived.Add(@object);
                }

                return postDeriveBacklog;
            }
            finally
            {
                this.Iteration = null;
            }
        }
    }
}
