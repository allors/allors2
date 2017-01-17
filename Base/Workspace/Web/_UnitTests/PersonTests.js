var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Tests;
(function (Tests) {
    var PersonTests = (function (_super) {
        __extends(PersonTests, _super);
        function PersonTests() {
            return _super.apply(this, arguments) || this;
        }
        PersonTests.prototype.displayName = function () {
            var workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            var session = new Allors.Session(workspace);
            var person = session.create("Person");
            this.areIdentical("N/A", person.displayName);
            person.UserName = "john@doe.com";
            this.areIdentical("john@doe.com", person.displayName);
            person.LastName = "Doe";
            this.areIdentical("Doe", person.displayName);
            person.FirstName = "John";
            this.areIdentical("John Doe", person.displayName);
            this.areIdentical("John Doe", person.displayName);
        };
        PersonTests.prototype.hello = function () {
            var workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            var session = new Allors.Session(workspace);
            var person = session.create("Person");
            person.LastName = "Doe";
            person.FirstName = "John";
            this.areIdentical("Hello John Doe", person.hello());
        };
        return PersonTests;
    }(tsUnit.TestClass));
    Tests.PersonTests = PersonTests;
})(Tests || (Tests = {}));
//# sourceMappingURL=PersonTests.js.map