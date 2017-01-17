var Allors;
(function (Allors) {
    var Session = (function () {
        function Session(workspace) {
            this.sessionObjectById = {};
            this.newSessionObjectById = {};
            this.workspace = workspace;
            this.hasChanges = false;
        }
        Session.prototype.get = function (id) {
            if (id === undefined || id === null) {
                return undefined;
            }
            var sessionObject = this.sessionObjectById[id];
            if (sessionObject === undefined) {
                sessionObject = this.newSessionObjectById[id];
                if (sessionObject === undefined) {
                    var workspaceObject = this.workspace.get(id);
                    var type = Allors.Domain[workspaceObject.objectType.name];
                    sessionObject = new type();
                    sessionObject.session = this;
                    sessionObject.workspaceObject = workspaceObject;
                    sessionObject.objectType = workspaceObject.objectType;
                    this.sessionObjectById[sessionObject.id] = sessionObject;
                }
            }
            return sessionObject;
        };
        Session.prototype.create = function (objectTypeName) {
            var type = Allors.Domain[objectTypeName];
            var newSessionObject = new type();
            newSessionObject.session = this;
            newSessionObject.objectType = this.workspace.objectTypeByName[objectTypeName];
            newSessionObject.newId = (--Session.idCounter).toString();
            this.newSessionObjectById[newSessionObject.newId] = newSessionObject;
            this.hasChanges = true;
            return newSessionObject;
        };
        Session.prototype.reset = function () {
            _.forEach(this.newSessionObjectById, function (v) {
                v.reset();
            });
            this.newSessionObjectById = {};
            _.forEach(this.sessionObjectById, function (v) {
                v.reset();
            });
            this.hasChanges = false;
        };
        Session.prototype.pushRequest = function () {
            var data = new Allors.Data.PushRequest();
            data.newObjects = [];
            data.objects = [];
            if (this.newSessionObjectById) {
                _.forEach(this.newSessionObjectById, function (newSessionObject) {
                    var objectData = newSessionObject.saveNew();
                    if (objectData !== undefined) {
                        data.newObjects.push(objectData);
                    }
                });
            }
            _.forEach(this.sessionObjectById, function (sessionObject) {
                var objectData = sessionObject.save();
                if (objectData !== undefined) {
                    data.objects.push(objectData);
                }
            });
            return data;
        };
        Session.prototype.pushResponse = function (pushResponse) {
            var _this = this;
            if (pushResponse.newObjects) {
                _.forEach(pushResponse.newObjects, function (pushResponseNewObject) {
                    var newId = pushResponseNewObject.ni;
                    var id = pushResponseNewObject.i;
                    var newSessionObject = _this.newSessionObjectById[newId];
                    var syncResponse = {
                        responseType: Allors.Data.ResponseType.Sync,
                        userSecurityHash: "#",
                        objects: [
                            {
                                i: id,
                                v: "",
                                t: newSessionObject.objectType.name,
                                roles: [],
                                methods: []
                            }
                        ]
                    };
                    delete (_this.newSessionObjectById[newId]);
                    delete (newSessionObject.newId);
                    _this.workspace.sync(syncResponse);
                    var workspaceObject = _this.workspace.get(id);
                    newSessionObject.workspaceObject = workspaceObject;
                    _this.sessionObjectById[id] = newSessionObject;
                });
            }
            if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
                throw new Error("Not all new objects received ids");
            }
        };
        return Session;
    }());
    Session.idCounter = 0;
    Allors.Session = Session;
})(Allors || (Allors = {}));
//# sourceMappingURL=Session.js.map