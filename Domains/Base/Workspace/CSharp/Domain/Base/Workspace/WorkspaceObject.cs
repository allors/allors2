namespace Allors.Workspace
{
    using System.Collections.Generic;
    using Meta;

    public class WorkspaceObject {
        public Workspace Workspace { get; }

        public long Id { get; }

        public long Version { get; }

        public string UserSecurityHash { get; }

        public Class ObjectType { get; }

        public Dictionary<string, object> Roles { get; }

        public Dictionary<string, object> Methods { get; }

        public WorkspaceObject(Workspace workspace, Data.SyncResponse loadResponse, Data.SyncResponseObject loadObject) {
            this.Workspace = workspace;
            this.Id = long.Parse(loadObject.i);
            this.Version = !string.IsNullOrEmpty(loadObject.v) ? long.Parse(loadObject.v) : 0;
            this.UserSecurityHash = loadResponse.userSecurityHash;
            this.ObjectType = (Class)this.Workspace.ObjectFactory.GetObjectTypeForTypeName(loadObject.t);

            this.Roles = new Dictionary<string, object>();
            this.Methods =new Dictionary<string, object>();

            if (loadObject.roles != null)
            {
                foreach (var role in loadObject.roles)
                {
                    var name = (string)role[0];
                    var access = (string)role[1];
                    var canRead = access.Contains("r");
                    var canWrite = access.Contains("w");

                    this.Roles[$"CanRead{name}"] = canRead;
                    this.Roles[$"CanWrite{name}"] = canWrite;

                    if (canRead)
                    {
                        var value = role.Length > 2 ? role[2] : null;
                        this.Roles[name] = value;
                    }
                }
            }

            if (loadObject.methods != null)
            {
                foreach (var method in loadObject.methods)
                {
                    var name = method[0];
                    var access = method[1];
                    var canExecute = access.Contains("x");

                    this.Methods[$"CanExecute{name}"] = canExecute;
                }
            }
        }

        public bool CanRead(string roleTypeName) {
            return (bool)this.Roles[$"CanRead{roleTypeName}"];
        }

        public bool CanWrite(string roleTypeName) {
            return (bool)this.Roles[$"CanWrite{roleTypeName}"];
        }

        public bool CanExecute(string methodTypeName)
        {
            return (bool)this.Methods[$"CanExecute{methodTypeName}"];
        }
    }
}