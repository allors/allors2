// <copyright file="DatabaseRecord.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace.Adapters
{
    using Meta;

    public abstract class DatabaseRecord : IRecord
    {
        protected DatabaseRecord(IClass @class, long id, long version)
        {
            this.Class = @class;
            this.Id = id;
            this.Version = version;
        }

        public IClass Class { get; }

        public long Id { get; }

        public long Version { get; }

        public abstract object GetRole(IRoleType roleType);

        public abstract bool IsPermitted(long permission);
    }
}
