namespace Allors.Workspace
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Data;
    using Allors.Meta;
    
    public class Session
    {
        private static long idCounter = 0;

        private readonly Workspace workspace;
        private readonly Dictionary<long, SessionObject> sessionObjectById = new Dictionary<long, SessionObject>();
        private readonly Dictionary<long, SessionObject> newSessionObjectById = new Dictionary<long, SessionObject>();

        public Session(Workspace workspace)
        {
            this.workspace = workspace;
        }

        public bool HasChanges
        {
            get
            {
                return this.newSessionObjectById.Count > 0 || this.sessionObjectById.Values.Any(v => v.HasChanges);
            }
        }

        public SessionObject Get(long id)
        {
            SessionObject sessionObject;
            if (!this.sessionObjectById.TryGetValue(id, out sessionObject))
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
                               newObjects = new List<PushRequestNewObject>(),
                               objects = new List<PushRequestObject>()
                           };

            foreach (var newSessionObject in this.newSessionObjectById.Values)
            {
                var objectData = newSessionObject.SaveNew();
                if (objectData != null)
                {
                    data.newObjects.Add(objectData);
                }
            }

            foreach (var sessionObject in this.sessionObjectById.Values)
            {
                var objectData = sessionObject.Save();
                if (objectData != null)
                {
                    data.objects.Add(objectData);
                }
            }

            return data;
        }
        
        public void PushResponse(PushResponse pushResponse)
        {
            if (pushResponse.newObjects != null && pushResponse.newObjects.Length > 0)
            {
                foreach (var pushResponseNewObject in pushResponse.newObjects)
                {
                    var newId = long.Parse(pushResponseNewObject.ni);
                    var id = long.Parse(pushResponseNewObject.i);

                    var newSessionObject = this.newSessionObjectById[newId];

                    var loadResponse = new SyncResponse
                                           {
                                               userSecurityHash = "#",
                                               // This should trigger a load on next check
                                               objects =
                                                   new[]
                                                       {
                                                           new SyncResponseObject
                                                               {
                                                                   i = id.ToString(),
                                                                   v = "",
                                                                   t =
                                                                       newSessionObject
                                                                       .ObjectType.Name,
                                                                   roles = new object[0][],
                                                                   methods = new string[0][]
                                                               }
                                                       }
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