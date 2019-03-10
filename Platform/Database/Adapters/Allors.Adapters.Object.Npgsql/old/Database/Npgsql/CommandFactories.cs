// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommandFactories.cs" company="Allors bvba">
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

    using AddCompositeRoleFactory = Allors.Adapters.Database.Npgsql.Commands.Procedure.AddCompositeRoleFactory;

    public sealed class CommandFactories : Sql.CommandFactories
    {
        private readonly Database database;

        // Session
        private GetObjectTypeFactory getObjectTypeFactory;
        private InstantiateObjectsFactory instantiateObjectsFactory;
        private AddCompositeRoleFactory addCompositeRoleFactory;
        private RemoveCompositeRoleFactory removeCompositeRoleFactory;
        private CreateObjectFactory createObjectFactory;
        private CreateObjectsFactory createObjectsFactory;
        private InsertObjectFactory insertObjectFactory;
        private InstantiateObjectFactory instantiateObjectFactory;
        private DeleteObjectFactory deleteObjectFactory;
        private GetCompositeAssociationFactory getCompositeAssociationFactory;
        private GetCompositeAssociationsFactory getCompositeAssociationsFactory;
        private GetCompositeRoleFactory getCompositeRoleFactory;
        private GetCompositeRolesFactory getCompositeRolesFactory;
        private GetUnitRolesFactory getUnitRolesFactory;
        private ClearCompositeAndCompositesRoleFactory clearCompositeAndCompositesRoleFactory;
        private SetCompositeRoleFactory setCompositeRoleFactory;
        private SetUnitRoleFactory setUnitRoleFactory;
        private SetUnitRolesFactory setUnitRolesFactory;
        private GetCacheIdsFactory getCacheIdsFactory;
        private UpdateCacheIdsFactory updateCacheIdsFactory;
        
        public CommandFactories(Database database)
        {
            this.database = database;
        }

        public override GetObjectTypeFactory GetObjectTypeFactory
        {
            get
            {
                return this.getObjectTypeFactory ?? (this.getObjectTypeFactory = new GetObjectTypeFactory(this.database));
            }
        }

        public override CreateObjectFactory CreateObjectFactory
        {
            get
            {
                return this.createObjectFactory ?? (this.createObjectFactory = new CreateObjectFactory());
            }
        }

        public override CreateObjectsFactory CreateObjectsFactory
        {
            get
            {
                return this.createObjectsFactory ?? (this.createObjectsFactory = new CreateObjectsFactory(this.NpgsqlDatabase));
            }
        }

        public override InsertObjectFactory InsertObjectFactory
        {
            get
            {
                return this.insertObjectFactory ?? (this.insertObjectFactory = new InsertObjectFactory());
            }
        }

        public override InstantiateObjectsFactory InstantiateObjectsFactory
        {
            get
            {
                return this.instantiateObjectsFactory ?? (this.instantiateObjectsFactory = new InstantiateObjectsFactory(this.NpgsqlDatabase));
            }
        }

        public override AddCompositeRoleFactory AddCompositeRoleFactory
        {
            get
            {
                return this.addCompositeRoleFactory ?? (this.addCompositeRoleFactory = new AddCompositeRoleFactory(this.NpgsqlDatabase));
            }
        }

        public override RemoveCompositeRoleFactory RemoveCompositeRoleFactory
        {
            get
            {
                return this.removeCompositeRoleFactory ?? (this.removeCompositeRoleFactory = new RemoveCompositeRoleFactory(this.NpgsqlDatabase));
            }
        }

        public override InstantiateObjectFactory InstantiateObjectFactory
        {
            get
            {
                return this.instantiateObjectFactory ?? (this.instantiateObjectFactory = new InstantiateObjectFactory(this.database));
            }
        }

        public override DeleteObjectFactory DeleteObjectFactory
        {
            get
            {
                return this.deleteObjectFactory ?? (this.deleteObjectFactory = new DeleteObjectFactory(this.database));
            }
        }
        
        public override GetCompositeAssociationFactory GetCompositeAssociationFactory
        {
            get
            {
                return this.getCompositeAssociationFactory ?? (this.getCompositeAssociationFactory = new GetCompositeAssociationFactory(this.database));
            }
        }

        public override GetCompositeAssociationsFactory GetCompositeAssociationsFactory
        {
            get
            {
                return this.getCompositeAssociationsFactory ?? (this.getCompositeAssociationsFactory = new GetCompositeAssociationsFactory(this.database));
            }
        }

        public override GetCompositeRoleFactory GetCompositeRoleFactory
        {
            get
            {
                return this.getCompositeRoleFactory ?? (this.getCompositeRoleFactory = new GetCompositeRoleFactory(this.database));
            }
        }

        public override GetCompositeRolesFactory GetCompositeRolesFactory
        {
            get
            {
                return this.getCompositeRolesFactory ?? (this.getCompositeRolesFactory = new GetCompositeRolesFactory(this.database));
            }
        }

        public override GetUnitRolesFactory GetUnitRolesFactory
        {
            get
            {
                return this.getUnitRolesFactory ?? (this.getUnitRolesFactory = new GetUnitRolesFactory(this.database));
            }
        }

        public override ClearCompositeAndCompositesRoleFactory ClearCompositeAndCompositesRoleFactory
        {
            get
            {
                return this.clearCompositeAndCompositesRoleFactory ?? (this.clearCompositeAndCompositesRoleFactory = new ClearCompositeAndCompositesRoleFactory(this.database));
            }
        }

        public override SetCompositeRoleFactory SetCompositeRoleFactory
        {
            get
            {
                return this.setCompositeRoleFactory ?? (this.setCompositeRoleFactory = new SetCompositeRoleFactory(this.database));
            }
        }

        public override SetUnitRoleFactory SetUnitRoleFactory
        {
            get
            {
                return this.setUnitRoleFactory ?? (this.setUnitRoleFactory = new SetUnitRoleFactory(this.database));
            }
        }

        public override SetUnitRolesFactory SetUnitRolesFactory
        {
            get
            {
                return this.setUnitRolesFactory ?? (this.setUnitRolesFactory = new SetUnitRolesFactory(this.database));
            }
        }

        public override GetCacheIdsFactory GetCacheIdsFactory
        {
            get
            {
                return this.getCacheIdsFactory ?? (this.getCacheIdsFactory = new GetCacheIdsFactory(this.database));
            }
        }

        public override UpdateCacheIdsFactory UpdateCacheIdsFactory
        {
            get
            {
                return this.updateCacheIdsFactory ?? (this.updateCacheIdsFactory = new UpdateCacheIdsFactory(this.NpgsqlDatabase));
            }
        }

        protected override Sql.Database Database
        {
            get { return this.NpgsqlDatabase; }
        }

        private Database NpgsqlDatabase
        {
            get { return this.database; }
        }
    }
}