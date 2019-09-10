// <copyright file="DefaultConnection.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Npgsql
{
    using global::Npgsql;

    public sealed class DefaultConnection : Connection
    {
        public DefaultConnection(Database database)
            : base(database)
        {
        }

        protected override Command CreateCommand(Mapping mapping, NpgsqlCommand sqlCommand) => new DefaultCommand(mapping, sqlCommand);

        #region Events

        protected override void OnCreatingNpgsqlConnection()
        {
        }

        protected override void OnCreatedNpgsqlConnection()
        {
        }

        protected override void OnOpeningNpgsqlConnection()
        {
        }

        protected override void OnOpenedNpgsqlConnection()
        {
        }

        protected override void OnClosingNpgsqlConnection()
        {
        }

        protected override void OnClosedNpgsqlConnection()
        {
        }

        protected override void OnCreatingNpgsqlTransaction()
        {
        }

        protected override void OnCreatedNpgsqlTransaction()
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

        protected override void OnCreatingNpgsqlCommand()
        {
        }

        protected override void OnCreatedNpgsqlCommand()
        {
        }

        #endregion Events
    }
}
