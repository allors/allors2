var Allors;
(function (Allors) {
    var Workspace = (function () {
        function Workspace(metaPopulationData) {
            var _this = this;
            this.objectTypeByName = {};
            this.workspaceObjectById = {};
            _.forEach(metaPopulationData.classes, function (classData) {
                var objectType = new Allors.Meta.ObjectType(classData);
                _this.objectTypeByName[objectType.name] = objectType;
            });
        }
        Workspace.prototype.diff = function (response) {
            var _this = this;
            var userSecurityHash = response.userSecurityHash;
            var requireLoadIdsWithVersion = _.filter(response.objects, function (idAndVersion) {
                var id = idAndVersion[0], version = idAndVersion[1];
                var workspaceObject = _this.workspaceObjectById[id];
                return (workspaceObject === undefined) || (workspaceObject === null) || (workspaceObject.version !== version) || (workspaceObject.userSecurityHash !== userSecurityHash);
            });
            var requireLoadIds = new Allors.Data.SyncRequest();
            requireLoadIds.objects = _.map(requireLoadIdsWithVersion, function (idWithVersion) {
                return idWithVersion[0];
            });
            return requireLoadIds;
        };
        Workspace.prototype.sync = function (syncResponse) {
            var _this = this;
            _.forEach(syncResponse.objects, function (objectData) {
                var workspaceObject = new Allors.WorkspaceObject(_this, syncResponse, objectData);
                _this.workspaceObjectById[workspaceObject.id] = workspaceObject;
            });
        };
        Workspace.prototype.get = function (id) {
            var workspaceObject = this.workspaceObjectById[id];
            if (workspaceObject === undefined) {
                throw new Error("Object with id " + id + " is not present.");
            }
            return workspaceObject;
        };
        return Workspace;
    }());
    Allors.Workspace = Workspace;
})(Allors || (Allors = {}));
//# sourceMappingURL=Workspace.js.map