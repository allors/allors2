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
    using Allors.Adapters.Database.Sql.Commands;

    public abstract class CommandFactories 
    {
        public abstract IGetObjectTypeFactory GetObjectTypeFactory { get; }

        public abstract ICreateObjectFactory CreateObjectFactory { get; }

        public abstract IInsertObjectFactory InsertObjectFactory { get; }

        public abstract ICreateObjectsFactory CreateObjectsFactory { get; }

        public abstract IInstantiateObjectFactory InstantiateObjectFactory { get; }

        public abstract IInstantiateObjectsFactory InstantiateObjectsFactory { get; }

        public abstract IAddCompositeRoleFactory AddCompositeRoleFactory { get; }

        public abstract IRemoveCompositeRoleFactory RemoveCompositeRoleFactory { get; }

        public abstract IDeleteObjectFactory DeleteObjectFactory { get; }

        public abstract IGetCompositeAssociationFactory GetCompositeAssociationFactory { get; }

        public abstract IGetCompositeAssociationsFactory GetCompositeAssociationsFactory { get; }

        public abstract IGetCompositeRoleFactory GetCompositeRoleFactory { get; }

        public abstract IGetCompositeRolesFactory GetCompositeRolesFactory { get; }

        public abstract IGetUnitRolesFactory GetUnitRolesFactory { get; }

        public abstract IClearCompositeAndCompositesRoleFactory ClearCompositeAndCompositesRoleFactory { get; }

        public abstract ISetCompositeRoleFactory SetCompositeRoleFactory { get; }

        public abstract ISetUnitRoleFactory SetUnitRoleFactory { get; }

        public abstract ISetUnitRolesFactory SetUnitRolesFactory { get; }

        public abstract IGetCacheIdsFactory GetCacheIdsFactory { get; }

        public abstract IUpdateCacheIdsFactory UpdateCacheIdsFactory { get; }

        protected abstract Database Database { get; }
    }
}