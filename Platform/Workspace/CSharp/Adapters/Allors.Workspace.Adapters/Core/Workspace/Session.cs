// <copyright file="Session.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Allors.Protocol.Remote.Push;
    using Allors.Protocol.Remote.Sync;
    using Allors.Workspace.Meta;

    public class Session : ISession
    {
        private static long idCounter = 0;

        private readonly Workspace workspace;
        private readonly Dictionary<long, ISessionObject> sessionObjectById = new Dictionary<long, ISessionObject>();
        private readonly Dictionary<long, ISessionObject> newSessionObjectById = new Dictionary<long, ISessionObject>();

        public Session(Workspace workspace) => this.workspace = workspace;

        public bool HasChanges => this.newSessionObjectById.Count > 0 || this.sessionObjectById.Values.Any(v => v.HasChanges);

        public IWorkspace Workspace => this.workspace;

        public ISessionObject Get(long id)
        {
            if (!this.sessionObjectById.TryGetValue(id, out var sessionObject))
            {
                if (!this.newSessionObjectById.TryGetValue(id, out sessionObject))
                {
                    var workspaceObject = this.workspace.Get(id);

                    var newSessionObject = this.workspace.ObjectFactory.Create(this, workspaceObject.Class);

                    newSessionObject.WorkspaceObject = workspaceObject;
                    newSessionObject.ObjectType = workspaceObject.Class;

                    this.sessionObjectById[workspaceObject.Id] = newSessionObject;

                    sessionObject = newSessionObject;
                }
            }

            return sessionObject;
        }

        public ISessionObject Create(IClass @class)
        {
            var newSessionObject = this.workspace.ObjectFactory.Create(this, @class);

            var newId = --Session.idCounter;
            newSessionObject.NewId = newId;
            newSessionObject.ObjectType = @class;

            this.newSessionObjectById[newId] = newSessionObject;

            return newSessionObject;
        }

        public void Reset()
        {
            foreach (var newSessionObject in this.newSessionObjectById.Values)
            {
                newSessionObject.Reset();
            }

            foreach (var sessionObject in this.sessionObjectById.Values)
            {
                sessionObject.Reset();
            }
        }

        public void Refresh()
        {
            foreach (var sessionObject in this.sessionObjectById.Values)
            {
                sessionObject.Refresh();
            }
        }


        public PushRequest PushRequest() =>
            new PushRequest
            {
                newObjects = this.newSessionObjectById.Select(v => v.Value.SaveNew()).ToArray(),
                objects = this.sessionObjectById.Select(v => v.Value.Save()).Where(v => v != null).ToArray(),
            };

        public void PushResponse(PushResponse pushResponse)
        {
            if (pushResponse.newObjects != null && pushResponse.newObjects.Length > 0)
            {
                foreach (var pushResponseNewObject in pushResponse.newObjects)
                {
                    var newId = long.Parse(pushResponseNewObject.ni);
                    var id = long.Parse(pushResponseNewObject.i);

                    var sessionObject = this.newSessionObjectById[newId];
                    sessionObject.NewId = null;
                    sessionObject.WorkspaceObject = this.workspace.New(id, sessionObject.ObjectType);

                    this.newSessionObjectById.Remove(newId);
                    this.sessionObjectById[id] = sessionObject;
                }
            }

            if (this.newSessionObjectById != null && this.newSessionObjectById.Count != 0)
            {
                throw new Exception("Not all new objects received ids");
            }
        }

        public IEnumerable<ISessionObject> GetAssociation(ISessionObject @object, IAssociationType associationType)
        {
            var roleType = associationType.RoleType;

            var associations = this.workspace.Get((IComposite)associationType.ObjectType).Select(v => this.Get(v.Id));
            foreach (var association in associations)
            {
                if (association.CanRead(roleType))
                {
                    if (roleType.IsOne)
                    {
                        var role = (SessionObject)((SessionObject)association).GetForAssociation(roleType);
                        if (role != null && role.Id == @object.Id)
                        {
                            yield return association;
                        }
                    }
                    else
                    {
                        var roles = (ISessionObject[])((SessionObject)association).GetForAssociation(roleType);
                        if (roles != null && roles.Contains(@object))
                        {
                            yield return association;
                        }
                    }
                }
            }
        }

        internal ISessionObject GetForAssociation(long id)
        {
            if (!this.sessionObjectById.TryGetValue(id, out var sessionObject))
            {
                this.newSessionObjectById.TryGetValue(id, out sessionObject);
            }

            return sessionObject;
        }
    }
}
