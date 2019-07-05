// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ObjectsBase.cs" company="Allors bvba">
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
            if(parameter == null)
            {
                return default(T);
            }

            var extent = this.Session.Extent(this.ObjectType);
            extent.Filter.AddEquals(roleType, parameter);
            return (T)extent.First;
        }

        protected virtual void BasePrepare(Setup setup)
        {
            setup.Add(this);
        }

        protected virtual void BaseSetup(Setup setup)
        {
        }

        protected virtual void BaseSecure(Security config)
        {
        }
    }
}
