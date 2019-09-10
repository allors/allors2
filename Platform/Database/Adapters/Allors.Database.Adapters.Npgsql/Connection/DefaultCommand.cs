// <copyright file="DefaultCommand.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Adapters.Npgsql
{
    using global::Npgsql;

    public class DefaultCommand : Command
    {
        public DefaultCommand(Mapping mapping, NpgsqlCommand command)
            : base(mapping, command)
        {
        }

        protected override void OnExecuting()
        {
        }

        protected override void OnExecuted()
        {
        }
    }
}
