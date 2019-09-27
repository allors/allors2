// <copyright file="WorkspaceObject.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using Meta;
    using Protocol.Remote.Sync;

    internal class SyncResponseContext
    {
        private readonly IMetaPopulation metaPopulation;

        private readonly Dictionary<string, IMetaObject> metaObjectByKey;

        internal SyncResponseContext(IWorkspace workspace, string userSecurityHash)
        {
            this.Workspace = workspace;
            this.UserSecurityHash = userSecurityHash;
            this.metaPopulation = this.Workspace.ObjectFactory.MetaPopulation;
            this.metaObjectByKey = new Dictionary<string, IMetaObject>();
        }

        public IWorkspace Workspace { get; }

        public string UserSecurityHash { get; }

        public IClass ReadClass(SyncResponseObject syncResponseObject)
        {
            var t = syncResponseObject.T;
            if (t.StartsWith(":"))
            {
                var secondColonIndex = t.IndexOf(':', 1);
                var key = t.Substring(1, secondColonIndex - 1);
                var classIdAsString = t.Substring(secondColonIndex + 1);
                var classId = Guid.Parse(classIdAsString);
                var @class = (IClass)this.metaPopulation.Find(classId);
                this.metaObjectByKey.Add(key, @class);
                return @class;
            }

            return (IClass)this.metaObjectByKey[t];
        }

        public IRoleType ReadRoleType(SyncResponseRole syncResponseRole)
        {
            var r = syncResponseRole.T;
            if (r.StartsWith(":"))
            {
                var secondColonIndex = r.IndexOf(':', 1);
                var key = r.Substring(1, secondColonIndex - 1);
                var roleTypeIdAsString = r.Substring(secondColonIndex + 1);
                var roleTypeId = Guid.Parse(roleTypeIdAsString);
                var roleType = (IRoleType)this.metaPopulation.Find(roleTypeId);
                this.metaObjectByKey.Add(key, roleType);
                return roleType;
            }

            return (IRoleType)this.metaObjectByKey[r];
        }

        public IMethodType ReadMethodType(string m)
        {
            if (m.StartsWith(":"))
            {
                var secondColonIndex = m.IndexOf(':', 1);
                var key = m.Substring(1, secondColonIndex - 1);
                var methodTypeIdAsString = m.Substring(secondColonIndex + 1);
                var methodTypeId = Guid.Parse(methodTypeIdAsString);
                var methodType = (IMethodType)this.metaPopulation.Find(methodTypeId);
                this.metaObjectByKey.Add(key, methodType);
                return methodType;
            }

            return (IMethodType)this.metaObjectByKey[m];
        }
    }
}
