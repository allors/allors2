// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SessionCommands.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
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

namespace Allors.Adapters.Database.Npgsql
{
    using Allors.Adapters.Database.Npgsql.Commands.Procedure;
    using Allors.Adapters.Database.Npgsql.Commands.Text;

    public sealed class SessionCommands : Sql.SessionCommands
    {
        private readonly DatabaseSession session;
        private readonly Sql.CommandFactories commandFactories;

        private GetObjectTypeFactory.GetObjectType getObjectType;
        private CreateObjectFactory.CreateObject createObjectCommand;
        private CreateObjectsFactory.CreateObjects createObjects;
        private InsertObjectFactory.InsertObject insertObject;
        private DeleteObjectFactory.DeleteObject deleteObject;
        private InstantiateObjectFactory.InstantiateObject instantiateObject;
        private InstantiateObjectsFactory.InstantiateObjects instantiateObjects;
        private GetCompositeRoleFactory.GetCompositeRole getCompositeRole;
        private SetCompositeRoleFactory.SetCompositeRole setCompositeRole;
        private ClearCompositeAndCompositesRoleFactory.ClearCompositeAndCompositesRole clearCompositeAndCompoisitesRole;
        private GetCompositeAssociationFactory.GetCompositeAssociation getCompositeAssociation;
        private GetCompositeRolesFactory.GetCompositeRoles getCompositeRoles;
        private AddCompositeRoleFactory.AddCompositeRole addCompositeRole;
        private RemoveCompositeRoleFactory.RemoveCompositeRole removeCompositeRole;
        private GetCompositeAssociationsFactory.GetCompositeAssociations getCompositeAssociations;
        private UpdateCacheIdsFactory.UpdateCacheIds updateCacheIds;
        private GetUnitRolesFactory.GetUnitRoles getUnitRoles;
        private SetUnitRoleFactory.SetUnitRole setUnitRole;
        private SetUnitRolesFactory.SetUnitRoles setUnitRoles;
        private GetCacheIdsFactory.GetCacheIds getCacheIds;

        internal SessionCommands(DatabaseSession session)
        {
            this.session = session;
            this.commandFactories = this.session.NpgsqlDatabase.NpgsqlCommandFactories;
        }

        public override GetObjectTypeFactory.GetObjectType GetObjectType
        {
            get
            {
                return this.getObjectType ?? (this.getObjectType = this.commandFactories.GetObjectTypeFactory.Create(this.session));
            }
        }

        public override CreateObjectFactory.CreateObject CreateObjectCommand
        {
            get
            {
                return this.createObjectCommand ?? (this.createObjectCommand = this.commandFactories.CreateObjectFactory.Create(this.session));
            }
        }

        public override CreateObjectsFactory.CreateObjects CreateObjectsCommand
        {
            get
            {
                return this.createObjects ?? (this.createObjects = this.commandFactories.CreateObjectsFactory.Create(this.session));
            }
        }

        public override InsertObjectFactory.InsertObject InsertObjectCommand
        {
            get
            {
                return this.insertObject ?? (this.insertObject = this.commandFactories.InsertObjectFactory.Create(this.session));
            }
        }

        public override DeleteObjectFactory.DeleteObject DeleteObjectCommand
        {
            get
            {
                return this.deleteObject ?? (this.deleteObject = this.commandFactories.DeleteObjectFactory.Create(this.session));
            }
        }

        public override InstantiateObjectFactory.InstantiateObject InstantiateObjectCommand
        {
            get
            {
                return this.instantiateObject ?? (this.instantiateObject = this.commandFactories.InstantiateObjectFactory.Create(this.session));
            }
        }

        public override InstantiateObjectsFactory.InstantiateObjects InstantiateObjectsCommand
        {
            get
            {
                return this.instantiateObjects ?? (this.instantiateObjects = this.commandFactories.InstantiateObjectsFactory.Create(this.session));
            }
        }

        public override GetUnitRolesFactory.GetUnitRoles GetUnitRolesCommand
        {
            get
            {
                return this.getUnitRoles ?? (this.getUnitRoles = this.commandFactories.GetUnitRolesFactory.Create(this.session));
            }
        }

        public override SetUnitRoleFactory.SetUnitRole SetUnitRoleCommand
        {
            get
            {
                return this.setUnitRole ?? (this.setUnitRole = this.commandFactories.SetUnitRoleFactory.Create(this.session));
            }
        }

        public override SetUnitRolesFactory.SetUnitRoles SetUnitRolesCommand
        {
            get
            {
                return this.setUnitRoles ?? (this.setUnitRoles = this.commandFactories.SetUnitRolesFactory.Create(this.session));
            }
        }

        public override GetCompositeRoleFactory.GetCompositeRole GetCompositeRoleCommand
        {
            get
            {
                return this.getCompositeRole ?? (this.getCompositeRole = this.commandFactories.GetCompositeRoleFactory.Create(this.session));
            }
        }

        public override SetCompositeRoleFactory.SetCompositeRole SetCompositeRoleCommand
        {
            get
            {
                return this.setCompositeRole ?? (this.setCompositeRole = this.commandFactories.SetCompositeRoleFactory.Create(this.session));
            }
        }

        public override ClearCompositeAndCompositesRoleFactory.ClearCompositeAndCompositesRole ClearCompositeAndCompositesRoleCommand
        {
            get
            {
                return this.clearCompositeAndCompoisitesRole ?? (this.clearCompositeAndCompoisitesRole = this.commandFactories.ClearCompositeAndCompositesRoleFactory.Create(this.session));
            }
        }

        public override GetCompositeAssociationFactory.GetCompositeAssociation GetCompositeAssociationCommand
        {
            get
            {
                return this.getCompositeAssociation ?? (this.getCompositeAssociation = this.commandFactories.GetCompositeAssociationFactory.Create(this.session));
            }
        }

        public override GetCompositeRolesFactory.GetCompositeRoles GetCompositeRolesCommand
        {
            get
            {
                return this.getCompositeRoles ?? (this.getCompositeRoles = this.commandFactories.GetCompositeRolesFactory.Create(this.session));
            }
        }

        public override AddCompositeRoleFactory.AddCompositeRole AddCompositeRoleCommand
        {
            get
            {
                return this.addCompositeRole ?? (this.addCompositeRole = this.commandFactories.AddCompositeRoleFactory.Create(this.session));
            }
        }

        public override RemoveCompositeRoleFactory.RemoveCompositeRole RemoveCompositeRoleCommand
        {
            get
            {
                return this.removeCompositeRole ?? (this.removeCompositeRole = this.commandFactories.RemoveCompositeRoleFactory.Create(this.session));
            }
        }

        public override GetCompositeAssociationsFactory.GetCompositeAssociations GetCompositeAssociationsCommand
        {
            get
            {
                return this.getCompositeAssociations ?? (this.getCompositeAssociations = this.commandFactories.GetCompositeAssociationsFactory.Create(this.session));
            }
        }

        public override UpdateCacheIdsFactory.UpdateCacheIds UpdateCacheIdsCommand
        {
            get
            {
                return this.updateCacheIds ?? (this.updateCacheIds = this.commandFactories.UpdateCacheIdsFactory.Create(this.session));
            }
        }

        public override GetCacheIdsFactory.GetCacheIds GetCacheIdsCommand
        {
            get
            {
                return this.getCacheIds ?? (this.getCacheIds = this.commandFactories.GetCacheIdsFactory.Create(this.session));
            }
        }
    }
}