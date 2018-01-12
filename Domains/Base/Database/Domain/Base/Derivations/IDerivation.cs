// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IDerivation.cs" company="Allors bvba">
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

namespace Allors.Domain
{
    using System;
    using System.Collections.Generic;

    using Allors;
    using Allors.Meta;

    public interface IDerivation
    {
        Guid Id { get; }

        DateTime TimeStamp { get; }

        ISession Session { get; }

        IValidation Validation { get; }

        IChangeSet ChangeSet { get; }

        int Generation { get; }

        ISet<Object> DerivedObjects { get; }

        object this[string name] { get; set; }

        IValidation Derive();

        void AddDerivable(Object derivable);

        void AddDerivables(IEnumerable<Object> derivables);

        /// <summary>
        /// The dependee is derived before the dependent object;
        /// </summary>
        /// <param name="dependent"></param>
        /// <param name="dependee"></param>
        void AddDependency(Object dependent, Object dependee);

        /// <summary>
        /// Gets or sets a value indicating if this derivable object is modified. 
        /// The object is considered modified if
        /// <ul>
        /// <li>it has been created</li>
        /// <li>it has changed roles</li>
        /// <li>it is marked as modified</li>
        /// </ul>
        /// </summary>
        /// <param name="derivable">The derivable object.</param>
        /// <returns>a value indicating if this derivable object is modified</returns>
        bool IsModified(Object derivable);

        bool IsModified(Object derivable, RelationKind kind);

        bool IsCreated(Object derivable);

        bool IsMarkedAsModified(Object derivable);

        bool HasChangedRole(Object derivable, RoleType roleType);

        bool HasChangedRoles(Object derivable, params RoleType[] roleTypes);

        bool HasChangedRoles(Object derivable, RelationKind relationKind);

        void MarkAsModified(Object derivable);
    }
}