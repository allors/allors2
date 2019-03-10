//------------------------------------------------------------------------------------------------- 
// <copyright file="CommandFactories.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the CommandFactories type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors.Adapters.Database.Sql
{
    using Allors.Adapters.Database.Npgsql.Commands.Procedure;
    using Allors.Adapters.Database.Npgsql.Commands.Text;

    public abstract class CommandFactories 
    {
        public abstract GetObjectTypeFactory GetObjectTypeFactory { get; }

        public abstract CreateObjectFactory CreateObjectFactory { get; }

        public abstract InsertObjectFactory InsertObjectFactory { get; }

        public abstract CreateObjectsFactory CreateObjectsFactory { get; }

        public abstract InstantiateObjectFactory InstantiateObjectFactory { get; }

        public abstract InstantiateObjectsFactory InstantiateObjectsFactory { get; }

        public abstract AddCompositeRoleFactory AddCompositeRoleFactory { get; }

        public abstract RemoveCompositeRoleFactory RemoveCompositeRoleFactory { get; }

        public abstract DeleteObjectFactory DeleteObjectFactory { get; }

        public abstract GetCompositeAssociationFactory GetCompositeAssociationFactory { get; }

        public abstract GetCompositeAssociationsFactory GetCompositeAssociationsFactory { get; }

        public abstract GetCompositeRoleFactory GetCompositeRoleFactory { get; }

        public abstract GetCompositeRolesFactory GetCompositeRolesFactory { get; }

        public abstract GetUnitRolesFactory GetUnitRolesFactory { get; }

        public abstract ClearCompositeAndCompositesRoleFactory ClearCompositeAndCompositesRoleFactory { get; }

        public abstract SetCompositeRoleFactory SetCompositeRoleFactory { get; }

        public abstract SetUnitRoleFactory SetUnitRoleFactory { get; }

        public abstract SetUnitRolesFactory SetUnitRolesFactory { get; }

        public abstract GetCacheIdsFactory GetCacheIdsFactory { get; }

        public abstract UpdateCacheIdsFactory UpdateCacheIdsFactory { get; }

        protected abstract Database Database { get; }
    }
}