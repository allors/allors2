// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectBuilder.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
//
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
//
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
//
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using System;

    public abstract class ObjectBuilder<T> : ObjectBuilder where T : Domain.Object
    {
        private readonly ISession session;

        private bool built;
        private Exception exception;

        protected ObjectBuilder(ISession session)
        {
            this.session = session;
        }

        ~ObjectBuilder()
        {
            if (this.exception == null && !this.built)
            {
                throw new Exception(this + " was not built.");
            }
        }

        protected ISession Session => this.session;

        protected ISession DatabaseSession => this.session;

        public override void Dispose()
        {
            this.Build();
        }

        public override string ToString()
        {
            return "Builder for " + typeof(T).Name;
        }

        public override IObject DefaultBuild()
        {
            return this.Build();
        }

        public virtual T Build()
        {
            GC.SuppressFinalize(this);

            try
            {
                var instance = this.session.Create<T>();
                this.OnBuild(instance);
                instance.OnBuild(x => x.WithBuilder(this));

                instance.OnPostBuild(x => x.WithBuilder(this));

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