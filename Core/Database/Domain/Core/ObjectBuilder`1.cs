// <copyright file="ObjectBuilder`1.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;

    public abstract class ObjectBuilder<T> : ObjectBuilder where T : Domain.Object
    {
        private bool built;
        private Exception exception;

        protected ObjectBuilder(ISession session) => this.Session = session;

        ~ObjectBuilder()
        {
            if (this.exception == null && !this.built)
            {
                throw new Exception(this + " was not built.");
            }
        }

        public ISession Session { get; }

        public override void Dispose() => this.Build();

        public override string ToString() => "Builder for " + typeof(T).Name;

        public override IObject DefaultBuild() => this.Build();

        public virtual T Build()
        {
            GC.SuppressFinalize(this);

            try
            {
                var instance = this.Session.Create<T>();
                this.OnBuild(instance);

                instance.OnBuild(x => x.WithBuilder(this));
                instance.OnPostBuild();

                this.built = true;

                return instance;
            }
            catch (Exception e)
            {
                this.exception = e;
                throw;
            }
        }

        protected abstract void OnBuild(T instance);
    }
}
