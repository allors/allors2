var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Person;
        (function (Person) {
            var Add;
            (function (Add) {
                var AddPersonController = (function (_super) {
                    __extends(AddPersonController, _super);
                    function AddPersonController(app, $scope, $state) {
                        var _this = _super.call(this, "AddPerson", app, $scope) || this;
                        _this.$state = $state;
                        _this.refresh()
                            .then(function () {
                            _this.person = _this.session.create("Person");
                        });
                        return _this;
                    }
                    AddPersonController.prototype.cancel = function () {
                        this.$state.go("relation.people");
                    };
                    AddPersonController.prototype.refresh = function () {
                        return this.load();
                    };
                    return AddPersonController;
                }(Pages.Page));
                AddPersonController.$inject = ["appService", "$scope", "$state"];
                angular
                    .module("app")
                    .controller("relationAddPersonController", AddPersonController);
            })(Add = Person.Add || (Person.Add = {}));
        })(Person = Pages.Person || (Pages.Person = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=addPerson.js.map