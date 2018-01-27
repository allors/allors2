// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ISetCompositeRole.cs" company="Allors bvba">
//   Copyright 2002-2013 Allors bvba.
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
// <summary>
//   Defines the SetCompositeRoleFactory type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql.Commands
{
    using System.Collections.Generic;

    using Allors.Meta;

    using CompositeRelation = CompositeRelation;

    public interface ISetCompositeRole
    {
        void Execute(IList<CompositeRelation> relation, IRoleType roleType);
    }

    public interface ISetCompositeRoleFactory
    {
        ISetCompositeRole Create(DatabaseSession session);
    }
}