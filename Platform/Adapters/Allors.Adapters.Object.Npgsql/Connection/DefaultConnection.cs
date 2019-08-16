// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultConnection.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters.Object.Npgsql
{
    using global::Npgsql;

    public sealed class DefaultConnection : Connection
    {
        public DefaultConnection(Database database)
            : base(database)
        {
        }

        protected override Command CreateCommand(Mapping mapping, NpgsqlCommand sqlCommand)
        {
            return new DefaultCommand(mapping, sqlCommand);
        }

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
        #endregion
    }
}
