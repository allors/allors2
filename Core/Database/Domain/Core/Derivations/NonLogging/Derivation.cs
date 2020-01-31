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
    using Object = Allors.Domain.Object;

    [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1121:UseBuiltInTypeAlias", Justification = "Allors Object")]
    public class Derivation : IDerivation
    {
        private bool guard;
        private Properties properties;

        public Derivation(ISession session, DerivationConfig config = null)
        {
            this.Config = config ?? new DerivationConfig();
            this.Session = session;

            this.Id = Guid.NewGuid();
            this.TimeStamp = this.Session.Now();

            this.ChangeSet = new AccumulatedChangeSet();
            this.DerivedObjects = new HashSet<Object>();
            this.Validation = new Validation(this);

            this.MarkedBacklog = new HashSet<Object>();

            this.guard = false;
        }

        public ISession Session { get; }

        public DerivationConfig Config { get; }

        public Guid Id { get; }

        public DateTime TimeStamp { get; private set; }

        public IValidation Validation { get; private set; }

        public ISet<Object> DerivedObjects { get; }

        ICycle IDerivation.Cycle => this.Cycle;

        IAccumulatedChangeSet IDerivation.ChangeSet => this.ChangeSet;

        public object this[string name]
        {
            get => this.properties?.Get(name);

            set
            {
                this.properties ??= new Properties();
                this.properties.Set(name, value);
            }
        }

        internal Cycle Cycle { get; set; }

        internal AccumulatedChangeSet ChangeSet { get; }

        internal ISet<Object> MarkedBacklog { get; private set; }

        public void Mark(Object @object) => this.MarkedBacklog.Add(@object);

        public void Mark(params Object[] objects) => this.MarkedBacklog.UnionWith(objects);

        public IValidation Derive()
        {
            Object[] GetAndResetMarked()
            {
                var marked = this.MarkedBacklog.Where(v => v != null).ToArray();
                this.MarkedBacklog = new HashSet<Object>();
                return marked;
            }

            try
            {
                this.Guard();

                var count = 1;

                this.Cycle = new Cycle(this);
                var derivedObjects = this.Cycle.Execute(GetAndResetMarked());

                while (derivedObjects.Any() || this.MarkedBacklog.Any())
                {
                    if (this.Config.MaxCycles != 0 && count++ > this.Config.MaxCycles)
                    {
                        throw new Exception("Maximum amount of cycles reached");
                    }

                    this.Cycle = new Cycle(this);
                    derivedObjects = this.Cycle.Execute(GetAndResetMarked());
                }

                return this.Validation;
            }
            finally
            {
                this.Cycle = null;
            }
        }

        private void Guard()
        {
            if (this.guard)
            {
                throw new Exception("Derive can only be called once. Create a new Derivation object.");
            }

            this.guard = true;
        }
    }
}
