// <copyright file="v.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public abstract class DatabaseConnection
    {
        private readonly IdGenerator idGenerator;

        protected DatabaseConnection(Configuration configuration, IdGenerator idGenerator)
        {
            this.Configuration = configuration;
            this.idGenerator = idGenerator;
        }

        public Configuration Configuration { get; }

        public abstract IWorkspace CreateWorkspace();

        public abstract DatabaseRecord GetRecord(long id);

        public abstract long GetPermission(IClass @class, IOperandType operandType, Operations operation);

        public long NextId() => this.idGenerator.Next();
    }
}
