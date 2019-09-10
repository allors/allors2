// <copyright file="DefaultConnection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.SqlClient
{
    using System.Data.SqlClient;

    public sealed class DefaultConnection : Connection
    {
        public DefaultConnection(Database database)
            : base(database)
        {
        }

        protected override Command CreateCommand(Mapping mapping, SqlCommand sqlCommand) => new DefaultCommand(mapping, sqlCommand);

        #region Events

        protected override void OnCreatingSqlConnection()
        {
        }

        protected override void OnCreatedSqlConnection()
        {
        }

        protected override void OnOpeningSqlConnection()
        {
        }

        protected override void OnOpenedSqlConnection()
        {
        }

        protected override void OnClosingSqlConnection()
        {
        }

        protected override void OnClosedSqlConnection()
        {
        }

        protected override void OnCreatingSqlTransaction()
        {
        }

        protected override void OnCreatedSqlTransaction()
        {
        }

        protected override void OnCommitting()
        {
        }

        protected override void OnCommitted()
        {
        }

        protected override void OnRollingBack()
        {
        }

        protected override void OnRolledBack()
        {
        }

        protected override void OnCreatingSqlCommand()
        {
        }

        protected override void OnCreatedSqlCommand()
        {
        }

        #endregion Events
    }
}
