var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Tests;
(function (Tests) {
    var WorkspaceTests = (function (_super) {
        __extends(WorkspaceTests, _super);
        function WorkspaceTests() {
            return _super.apply(this, arguments) || this;
        }
        WorkspaceTests.prototype.load = function () {
            var workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            workspace.sync(Tests.Fixture.loadData);
            var martien = workspace.get("3");
            this.areIdentical("3", martien.id);
            this.areIdentical("1003", martien.version);
            this.areIdentical("Person", martien.objectType.name);
            this.areIdentical("Martien", martien.roles.FirstName);
            this.areIdentical("van", martien.roles.MiddleName);
            this.areIdentical("Knippenberg", martien.roles.LastName);
            this.areIdentical(undefined, martien.roles.IsStudent);
            this.areIdentical(undefined, martien.roles.BirthDate);
        };
        WorkspaceTests.prototype.checkVersions = function () {
            var workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            workspace.userSecurityHash = "#";
            workspace.sync(Tests.Fixture.loadData);
            var pullResponse = {
                responseType: Allors.Data.ResponseType.Pull,
                userSecurityHash: "#",
                objects: [
                    ["1", "1001"],
                    ["2", "1002"],
                    ["3", "1004"]
                ]
            };
            var requireLoad = workspace.diff(pullResponse);
            this.areIdentical(1, requireLoad.objects.length);
        };
        WorkspaceTests.prototype.checkVersionsUserSecurityHash = function () {
            var workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            workspace.userSecurityHash = "abc";
            workspace.sync(Tests.Fixture.loadData);
            var pullResponse = {
                responseType: Allors.Data.ResponseType.Pull,
                userSecurityHash: "def",
                objects: [
                    ["1", "1001"],
                    ["2", "1002"],
                    ["3", "1004"]
                ]
            };
            var requireLoad = workspace.diff(pullResponse);
            this.areIdentical(3, requireLoad.objects.length);
        };
        return WorkspaceTests;
    }(tsUnit.TestClass));
    Tests.WorkspaceTests = WorkspaceTests;
})(Tests || (Tests = {}));
//# sourceMappingURL=WorkspaceTests.js.map