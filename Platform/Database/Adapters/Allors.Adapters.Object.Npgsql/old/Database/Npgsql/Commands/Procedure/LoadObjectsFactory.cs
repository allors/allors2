// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoadObjectsFactory.cs" company="Allors bvba">
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
// <summary>
//   
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Database.Npgsql.Commands.Procedure
{
    using System.Collections.Generic;
    using System.Data;

    using Allors.Adapters.Database.Sql;
    using Allors.Adapters.Database.Sql.Commands;
    using Allors.Meta;

    internal class LoadObjectsFactory : ILoadObjectsFactory
    {
        internal readonly Npgsql.ManagementSession ManagementSession;

        public LoadObjectsFactory(Npgsql.ManagementSession session)
        {
            this.ManagementSession = session;
        }

        public ILoadObjects Create(IObjectType objectType)
        {
            return new LoadObjects(this);
        }

        private class LoadObjects : Commands.Command, ILoadObjects
        {
            private readonly LoadObjectsFactory factory;

            public LoadObjects(LoadObjectsFactory factory)
            {
                this.factory = factory;
            }

            public void Execute(IClass objectType, IEnumerable<long> objectIds)
            {
                var database = this.factory.ManagementSession.NpgsqlDatabase;

                var exclusiveLeafClass = objectType.ExclusiveClass;
                var schema = database.NpgsqlSchema;

                lock (database)
                {
                    using (var command = this.factory.ManagementSession.CreateNpgsqlCommand(Schema.AllorsPrefix + "L_" + exclusiveLeafClass.Name))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        this.AddInObject(command, schema.TypeId.Param, objectType.Id);
                        this.AddInTable(command, schema.ObjectArrayParam, database.CreateObjectTable(objectIds));
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}