// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsBase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors
{
    using Allors.Meta;

    using Allors.Domain;

    public abstract partial class ObjectsBase<T> : IObjects where T : IObject
    {
        private readonly ISession session;

        protected ObjectsBase(ISession session)
        {
            this.session = session;
        }

        public abstract Composite ObjectType { get; }

        public ISession Session
        {
            get { return this.session; }
        }

        public Extent<T> Extent()
        {
            return this.Session.Extent<T>();
        }

        public T FindBy(RoleType roleType, object parameter)
        {
            if (parameter == null)
            {
                return default(T);
            }

            var extent = this.Session.Extent(this.ObjectType);
            extent.Filter.AddEquals(roleType, parameter);
            return (T)extent.First;
        }

        protected virtual void CorePrepare(Setup setup)
        {
            setup.Add(this);
        }

        protected virtual void CoreSetup(Setup setup)
        {
        }

        protected virtual void CoreSecure(Security config)
        {
        }
    }
}
