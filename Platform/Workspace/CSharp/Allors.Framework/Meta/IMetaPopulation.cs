//------------------------------------------------------------------------------------------------- 
// <copyright file="IMetaPopulation.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// <summary>Defines the Domain type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;

    public interface IMetaPopulation
    {
        IEnumerable<IUnit> Units { get; }

        IEnumerable<IComposite> Composites { get; }
        
        IEnumerable<IClass> Classes { get; }

        IEnumerable<IInterface> Interfaces { get; }

        IEnumerable<IRelationType> RelationTypes { get; }

        IMetaObject Find(Guid metaObjectId);

        IClass FindClassByName(string name);

        bool IsValid { get; }

        IValidationLog Validate();

        void Bind(Type[] types, MethodInfo[] methods);
    }
}