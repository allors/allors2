// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeleteObjectFactory.cs" company="Allors bvba">
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

namespace Allors.Adapters.Database.Npgsql.Commands.Text
{
    using System.Collections.Generic;

    using Allors.Adapters.Database.Sql;
    using Allors.Meta;

    using global::Npgsql;

    using Database = Database;
    using DatabaseSession = DatabaseSession;

    public class DeleteObjectFactory
    {
        public readonly Database Database;
        private readonly Dictionary<IObjectType, string> sqlByMetaType;

        public DeleteObjectFactory(Database database)
        {
            this.Database = database;
            this.sqlByMetaType = new Dictionary<IObjectType, string>();
        }

        public DeleteObject Create(DatabaseSession session)
        {
            return new DeleteObject(this, session);
        }

        public class DeleteObject 
        {
            private readonly DeleteObjectFactory factory;

            private readonly DatabaseSession session;

            private readonly Dictionary<IObjectType, NpgsqlCommand> commandByObjectType;

            public DeleteObject(DeleteObjectFactory factory, DatabaseSession session)
            {
                this.factory = factory;
                this.session = session;
                this.commandByObjectType = new Dictionary<IObjectType, NpgsqlCommand>();
            }

            public void Execute(Strategy strategy)
            {
                var objectType = strategy.Class;

                NpgsqlCommand command;
                if (!this.commandByObjectType.TryGetValue(objectType, out command))
                {
                    if (!this.factory.sqlByMetaType.ContainsKey(objectType))
                    {
                        Schema schema = this.factory.Database.Schema;

                        string sql = string.Empty;

                        sql += "DELETE FROM " + schema.Objects + "\n";
                        sql += "WHERE " + schema.ObjectId + "=" + schema.ObjectId.Param.InvocationName + ";\n";

                        sql += "DELETE FROM " + schema.Table(objectType.ExclusiveClass) + "\n";
                        sql += "WHERE " + schema.ObjectId + "=" + schema.ObjectId.Param.InvocationName + ";\n";

                        this.factory.sqlByMetaType[objectType] = sql;
                    }

                    command = this.session.CreateNpgsqlCommand(this.factory.sqlByMetaType[objectType]);
                    Commands.NpgsqlCommandExtensions.AddInObject(command, this.session.Schema.ObjectId.Param, strategy.ObjectId);

                    this.commandByObjectType[objectType] = command;
                }
                else
                {
                    Commands.NpgsqlCommandExtensions.SetInObject(command, this.session.Schema.ObjectId.Param, strategy.ObjectId);
                }
                
                command.ExecuteNonQuery();
            }
        }
    }
}