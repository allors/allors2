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
    using Allors.Adapters.Database.Npgsql.Commands.Procedure;
    using Allors.Adapters.Database.Npgsql.Commands.Text;

    public abstract class SessionCommands
    {
        public abstract GetObjectTypeFactory.GetObjectType GetObjectType { get; }

        public abstract CreateObjectFactory.CreateObject CreateObjectCommand { get; }

        public abstract CreateObjectsFactory.CreateObjects CreateObjectsCommand { get; }

        public abstract InsertObjectFactory.InsertObject InsertObjectCommand { get; }

        public abstract DeleteObjectFactory.DeleteObject DeleteObjectCommand { get; }

        public abstract InstantiateObjectFactory.InstantiateObject InstantiateObjectCommand { get; }

        public abstract InstantiateObjectsFactory.InstantiateObjects InstantiateObjectsCommand { get; }

        public abstract GetUnitRolesFactory.GetUnitRoles GetUnitRolesCommand { get; }

        public abstract SetUnitRoleFactory.SetUnitRole SetUnitRoleCommand { get; }

        public abstract SetUnitRolesFactory.SetUnitRoles SetUnitRolesCommand { get; }

        public abstract GetCompositeRoleFactory.GetCompositeRole GetCompositeRoleCommand { get; }

        public abstract SetCompositeRoleFactory.SetCompositeRole SetCompositeRoleCommand { get; }

        public abstract ClearCompositeAndCompositesRoleFactory.ClearCompositeAndCompositesRole ClearCompositeAndCompositesRoleCommand { get; }

        public abstract GetCompositeAssociationFactory.GetCompositeAssociation GetCompositeAssociationCommand { get; }

        public abstract GetCompositeRolesFactory.GetCompositeRoles GetCompositeRolesCommand { get; }

        public abstract AddCompositeRoleFactory.AddCompositeRole AddCompositeRoleCommand { get; }

        public abstract RemoveCompositeRoleFactory.RemoveCompositeRole RemoveCompositeRoleCommand { get; }

        public abstract GetCompositeAssociationsFactory.GetCompositeAssociations GetCompositeAssociationsCommand { get; }

        public abstract UpdateCacheIdsFactory.UpdateCacheIds UpdateCacheIdsCommand { get; }

        public abstract GetCacheIdsFactory.GetCacheIds GetCacheIdsCommand { get; }
    }
}