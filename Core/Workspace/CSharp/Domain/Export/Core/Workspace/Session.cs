// <copyright file="Session.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
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

    public class Session
    {
        private static long idCounter = 0;

        private readonly Workspace workspace;
        private readonly Dictionary<long, SessionObject> sessionObjectById = new Dictionary<long, SessionObject>();
        private readonly Dictionary<long, SessionObject> newSessionObjectById = new Dictionary<long, SessionObject>();

        public Session(Workspace workspace) => this.workspace = workspace;

        public bool HasChanges => this.newSessionObjectById.Count > 0 || this.sessionObjectById.Values.Any(v => v.HasChanges);

        public SessionObject Get(long id)
        {
            if (!this.sessionObjectById.TryGetValue(id, out var sessionObject))
            {
                if (!this.newSessionObjectById.TryGetValue(id, out sessionObject))
                {
                    var workspaceObject = this.workspace.Get(id);

                    sessionObject = this.workspace.ObjectFactory.Create(this, workspaceObject.ObjectType);

                    sessionObject.WorkspaceObject = workspaceObject;
                    sessionObject.ObjectType = workspaceObject.ObjectType;

                    this.sessionObjectById[workspaceObject.Id] = sessionObject;
                }
            }

            return sessionObject;
        }

        public SessionObject Create(Class @class)
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

        public PushRequest PushRequest()
        {
            var data = new PushRequest
            {
                NewObjects = this.newSessionObjectById.Select(v => v.Value.SaveNew()).ToArray(),
                Objects = this.sessionObjectById.Select(v => v.Value.Save()).Where(v => v != null).ToArray(),
            };
            return data;
        }

        public void PushResponse(PushResponse pushResponse)
        {
            if (pushResponse.NewObjects != null && pushResponse.NewObjects.Length > 0)
            {
                foreach (var pushResponseNewObject in pushResponse.NewObjects)
                {
                    var newId = long.Parse(pushResponseNewObject.NI);
                    var id = long.Parse(pushResponseNewObject.I);

                    var newSessionObject = this.newSessionObjectById[newId];

                    var loadResponse = new SyncResponse
                    {
                        UserSecurityHash = "#",
                        // This should trigger a load on next check
                        Objects =
                                                   new[]
                                                       {
                                                           new SyncResponseObject
                                                               {
                                                                   I = id.ToString(),
                                                                   V = string.Empty,
                                                                   T =
                                                                       newSessionObject
                                                                       .ObjectType.Name,
                                                                   Roles = new object[0][],
                                                                   Methods = new string[0][]
                                                               },
                                                       },
                    };

                    this.newSessionObjectById.Remove(newId);
                    newSessionObject.NewId = null;

                    this.workspace.Sync(loadResponse);
                    var workspaceObject = this.workspace.Get(id);
                    newSessionObject.WorkspaceObject = workspaceObject;

                    this.sessionObjectById[id] = newSessionObject;
                }
            }

            if (this.newSessionObjectById != null && this.newSessionObjectById.Count != 0)
            {
                throw new Exception("Not all new objects received ids");
            }
        }
    }
}
