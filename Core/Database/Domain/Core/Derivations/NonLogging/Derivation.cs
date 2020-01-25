// <copyright file="Derivation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.NonLogging
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using Allors;
    using Database.Adapters;
    using Object = Allors.Domain.Object;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public class Derivation : IDerivation
    {
        private Dictionary<string, object> properties;

        public Derivation(ISession session)
        {
            this.Session = session;

            this.Id = Guid.NewGuid();

            this.DerivedObjects = new HashSet<Object>();
            this.ChangeSet = new AccumulatedChangeSet();
            this.DerivedObjects = new HashSet<Object>();
            this.Validation = new Validation(this);
        }

        public ISession Session { get; }

        public Guid Id { get; }

        public DateTime TimeStamp { get; private set; }

        public IValidation Validation { get; protected set; }

        public ISet<Object> DerivedObjects { get; }

        ICycle IDerivation.Cycle => this.Cycle;

        IAccumulatedChangeSet IDerive.ChangeSet => this.ChangeSet;

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

        internal Cycle Cycle { get; set; }

        internal AccumulatedChangeSet ChangeSet { get; }

        public IValidation Derive(params Object[] marked)
        {
            try
            {
                this.TimeStamp = this.Session.Now();

                if (this.Cycle != null)
                {
                    throw new Exception("Derive can only be called once. Create a new Derivation object.");
                }

                var markedSet = marked.Length > 0 ? new HashSet<Object>(marked) : null;

                this.Cycle = new Cycle(this, markedSet);
                this.Cycle.Execute();

                while (this.Cycle.DerivedObjects.Any())
                {
                    this.Cycle = new Cycle(this);
                    this.Cycle.Execute();
                }

                return this.Validation;
            }
            finally
            {
                this.Cycle = null;
            }
        }

        private void AssertGeneration()
        {
            if (this.Cycle == null)
            {
                throw new Exception("Add can only be called during a derivation. Use Derive(intial) instead.");
            }
        }
    }
}
