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
            var Edit;
            (function (Edit) {
                var EditPersonController = (function (_super) {
                    __extends(EditPersonController, _super);
                    function EditPersonController(app, $scope, $state, params) {
                        var _this = _super.call(this, "EditPerson", app, $scope) || this;
                        _this.$state = $state;
                        _this.params = params;
                        _this.refresh();
                        return _this;
                    }
                    EditPersonController.prototype.cancel = function () {
                        this.$state.go("relation.people");
                    };
                    EditPersonController.prototype.save = function () {
                        var _this = this;
                        return _super.prototype.save.call(this).then(function () { return _this.$state.go("relation.people"); });
                    };
                    EditPersonController.prototype.refresh = function () {
                        var _this = this;
                        return this.load({
                            id: this.params.id
                        })
                            .then(function () {
                            _this.person = _this.objects["person"];
                            var persons = _this.collections["persons"];
                            _this.lastNameOptions = _(persons).map(function (v) { return v.LastName; }).uniq().compact().value() || [];
                        });
                    };
                    return EditPersonController;
                }(Pages.Page));
                EditPersonController.$inject = ["appService", "$scope", "$state", "$stateParams"];
                angular
                    .module("app")
                    .controller("relationEditPersonController", EditPersonController);
            })(Edit = Person.Edit || (Person.Edit = {}));
        })(Person = Pages.Person || (Pages.Person = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=editPerson.js.map