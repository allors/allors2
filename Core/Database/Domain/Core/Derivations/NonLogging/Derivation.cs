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
        private bool guard;
        private Properties properties;

        public Derivation(ISession session)
        {
            this.Session = session;

            this.Id = Guid.NewGuid();
            this.TimeStamp = this.Session.Now();

            this.ChangeSet = new AccumulatedChangeSet();
            this.DerivedObjects = new HashSet<Object>();
            this.Validation = new Validation(this);

            this.guard = false;
        }

        public ISession Session { get; }

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

        public IValidation Derive() => this.Derive(null);

        public IValidation Derive(params Object[] marked)
        {
            try
            {
                this.Guard();

                this.Cycle = new Cycle(this);
                var derivedObjects = this.Cycle.Execute(marked);

                while (derivedObjects.Count > 0)
                {
                    this.Cycle = new Cycle(this);
                    derivedObjects = this.Cycle.Execute();
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
