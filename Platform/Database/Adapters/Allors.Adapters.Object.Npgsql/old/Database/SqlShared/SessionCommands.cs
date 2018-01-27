// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionCommands.cs" company="Allors bvba">
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
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Sql
{
    using Allors.Adapters.Database.Sql.Commands;

    public abstract class SessionCommands
    {
        public abstract IGetObjectType GetObjectType { get; }

        public abstract ICreateObject CreateObjectCommand { get; }

        public abstract ICreateObjects CreateObjectsCommand { get; }

        public abstract IInsertObject InsertObjectCommand { get; }

        public abstract IDeleteObject DeleteObjectCommand { get; }

        public abstract IInstantiateObject InstantiateObjectCommand { get; }

        public abstract IInstantiateObjects InstantiateObjectsCommand { get; }

        public abstract IGetUnitRoles GetUnitRolesCommand { get; }

        public abstract ISetUnitRole SetUnitRoleCommand { get; }

        public abstract ISetUnitRoles SetUnitRolesCommand { get; }

        public abstract IGetCompositeRole GetCompositeRoleCommand { get; }

        public abstract ISetCompositeRole SetCompositeRoleCommand { get; }

        public abstract IClearCompositeAndCompositesRole ClearCompositeAndCompositesRoleCommand { get; }

        public abstract IGetCompositeAssociation GetCompositeAssociationCommand { get; }

        public abstract IGetCompositeRoles GetCompositeRolesCommand { get; }

        public abstract IAddCompositeRole AddCompositeRoleCommand { get; }

        public abstract IRemoveCompositeRole RemoveCompositeRoleCommand { get; }

        public abstract IGetCompositeAssociations GetCompositeAssociationsCommand { get; }

        public abstract IUpdateCacheIds UpdateCacheIdsCommand { get; }

        public abstract IGetCacheIds GetCacheIdsCommand { get; }
    }
}