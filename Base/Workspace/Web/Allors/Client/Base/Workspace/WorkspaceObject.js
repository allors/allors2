var Allors;
(function (Allors) {
    var WorkspaceObject = (function () {
        function WorkspaceObject(workspace, loadResponse, loadObject) {
            var _this = this;
            this.workspace = workspace;
            this.i = loadObject.i;
            this.v = loadObject.v;
            this.u = loadResponse.userSecurityHash;
            this.t = loadObject.t;
            this.roles = {};
            this.methods = {};
            var objectType = this.workspace.objectTypeByName[this.t];
            _.forEach(loadObject.roles, function (role) {
                var name = role[0], access = role[1];
                var canRead = access.indexOf("r") !== -1;
                var canWrite = access.indexOf("w") !== -1;
                _this.roles["CanRead" + name] = canRead;
                _this.roles["CanWrite" + name] = canWrite;
                if (canRead) {
                    var roleType = objectType.roleTypeByName[name];
                    var value = role[2];
                    if (value && roleType.isUnit && roleType.objectType === "DateTime") {
                        value = new Date(value);
                    }
                    _this.roles[name] = value;
                }
            });
            _.forEach(loadObject.methods, function (method) {
                var name = method[0], access = method[1];
                var canExecute = access.indexOf("x") !== -1;
                _this.methods["CanExecute" + name] = canExecute;
            });
        }
        Object.defineProperty(WorkspaceObject.prototype, "id", {
            get: function () {
                return this.i;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "version", {
            get: function () {
                return this.v;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "userSecurityHash", {
            get: function () {
                return this.u;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "objectType", {
            get: function () {
                return this.workspace.objectTypeByName[this.t];
            },
            enumerable: true,
            configurable: true
        });
        WorkspaceObject.prototype.canRead = function (roleTypeName) {
            return this.roles["CanRead" + roleTypeName];
        };
        WorkspaceObject.prototype.canWrite = function (roleTypeName) {
            return this.roles["CanWrite" + roleTypeName];
        };
        WorkspaceObject.prototype.canExecute = function (methodName) {
            return this.methods["CanExecute" + methodName];
        };
        return WorkspaceObject;
    }());
    Allors.WorkspaceObject = WorkspaceObject;
})(Allors || (Allors = {}));
//# sourceMappingURL=WorkspaceObject.js.map