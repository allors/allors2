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
    using Allors.Adapters.Database.Sql.Commands;

    public sealed class SessionCommands : Sql.SessionCommands
    {
        private readonly DatabaseSession session;
        private readonly Sql.CommandFactories commandFactories;

        private IGetObjectType getObjectType;
        private ICreateObject createObjectCommand;
        private ICreateObjects createObjects;
        private IInsertObject insertObject;
        private IDeleteObject deleteObject;
        private IInstantiateObject instantiateObject;
        private IInstantiateObjects instantiateObjects;
        private IGetCompositeRole getCompositeRole;
        private ISetCompositeRole setCompositeRole;
        private IClearCompositeAndCompositesRole clearCompositeAndCompoisitesRole;
        private IGetCompositeAssociation getCompositeAssociation;
        private IGetCompositeRoles getCompositeRoles;
        private IAddCompositeRole addCompositeRole;
        private IRemoveCompositeRole removeCompositeRole;
        private IGetCompositeAssociations getCompositeAssociations;
        private IUpdateCacheIds updateCacheIds;
        private IGetUnitRoles getUnitRoles;
        private ISetUnitRole setUnitRole;
        private ISetUnitRoles setUnitRoles;
        private IGetCacheIds getCacheIds;

        internal SessionCommands(DatabaseSession session)
        {
            this.session = session;
            this.commandFactories = this.session.NpgsqlDatabase.NpgsqlCommandFactories;
        }

        public override IGetObjectType GetObjectType
        {
            get
            {
                return this.getObjectType ?? (this.getObjectType = this.commandFactories.GetObjectTypeFactory.Create(this.session));
            }
        }

        public override ICreateObject CreateObjectCommand
        {
            get
            {
                return this.createObjectCommand ?? (this.createObjectCommand = this.commandFactories.CreateObjectFactory.Create(this.session));
            }
        }

        public override ICreateObjects CreateObjectsCommand
        {
            get
            {
                return this.createObjects ?? (this.createObjects = this.commandFactories.CreateObjectsFactory.Create(this.session));
            }
        }

        public override IInsertObject InsertObjectCommand
        {
            get
            {
                return this.insertObject ?? (this.insertObject = this.commandFactories.InsertObjectFactory.Create(this.session));
            }
        }

        public override IDeleteObject DeleteObjectCommand
        {
            get
            {
                return this.deleteObject ?? (this.deleteObject = this.commandFactories.DeleteObjectFactory.Create(this.session));
            }
        }

        public override IInstantiateObject InstantiateObjectCommand
        {
            get
            {
                return this.instantiateObject ?? (this.instantiateObject = this.commandFactories.InstantiateObjectFactory.Create(this.session));
            }
        }

        public override IInstantiateObjects InstantiateObjectsCommand
        {
            get
            {
                return this.instantiateObjects ?? (this.instantiateObjects = this.commandFactories.InstantiateObjectsFactory.Create(this.session));
            }
        }

        public override IGetUnitRoles GetUnitRolesCommand
        {
            get
            {
                return this.getUnitRoles ?? (this.getUnitRoles = this.commandFactories.GetUnitRolesFactory.Create(this.session));
            }
        }

        public override ISetUnitRole SetUnitRoleCommand
        {
            get
            {
                return this.setUnitRole ?? (this.setUnitRole = this.commandFactories.SetUnitRoleFactory.Create(this.session));
            }
        }

        public override ISetUnitRoles SetUnitRolesCommand
        {
            get
            {
                return this.setUnitRoles ?? (this.setUnitRoles = this.commandFactories.SetUnitRolesFactory.Create(this.session));
            }
        }

        public override IGetCompositeRole GetCompositeRoleCommand
        {
            get
            {
                return this.getCompositeRole ?? (this.getCompositeRole = this.commandFactories.GetCompositeRoleFactory.Create(this.session));
            }
        }

        public override ISetCompositeRole SetCompositeRoleCommand
        {
            get
            {
                return this.setCompositeRole ?? (this.setCompositeRole = this.commandFactories.SetCompositeRoleFactory.Create(this.session));
            }
        }

        public override IClearCompositeAndCompositesRole ClearCompositeAndCompositesRoleCommand
        {
            get
            {
                return this.clearCompositeAndCompoisitesRole ?? (this.clearCompositeAndCompoisitesRole = this.commandFactories.ClearCompositeAndCompositesRoleFactory.Create(this.session));
            }
        }

        public override IGetCompositeAssociation GetCompositeAssociationCommand
        {
            get
            {
                return this.getCompositeAssociation ?? (this.getCompositeAssociation = this.commandFactories.GetCompositeAssociationFactory.Create(this.session));
            }
        }

        public override IGetCompositeRoles GetCompositeRolesCommand
        {
            get
            {
                return this.getCompositeRoles ?? (this.getCompositeRoles = this.commandFactories.GetCompositeRolesFactory.Create(this.session));
            }
        }

        public override IAddCompositeRole AddCompositeRoleCommand
        {
            get
            {
                return this.addCompositeRole ?? (this.addCompositeRole = this.commandFactories.AddCompositeRoleFactory.Create(this.session));
            }
        }

        public override IRemoveCompositeRole RemoveCompositeRoleCommand
        {
            get
            {
                return this.removeCompositeRole ?? (this.removeCompositeRole = this.commandFactories.RemoveCompositeRoleFactory.Create(this.session));
            }
        }

        public override IGetCompositeAssociations GetCompositeAssociationsCommand
        {
            get
            {
                return this.getCompositeAssociations ?? (this.getCompositeAssociations = this.commandFactories.GetCompositeAssociationsFactory.Create(this.session));
            }
        }

        public override IUpdateCacheIds UpdateCacheIdsCommand
        {
            get
            {
                return this.updateCacheIds ?? (this.updateCacheIds = this.commandFactories.UpdateCacheIdsFactory.Create(this.session));
            }
        }

        public override IGetCacheIds GetCacheIdsCommand
        {
            get
            {
                return this.getCacheIds ?? (this.getCacheIds = this.commandFactories.GetCacheIdsFactory.Create(this.session));
            }
        }
    }
}