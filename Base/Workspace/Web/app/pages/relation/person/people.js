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
            var PeopleController = (function (_super) {
                __extends(PeopleController, _super);
                function PeopleController(app, $scope, $state, params) {
                    var _this = _super.call(this, "People", app, $scope) || this;
                    _this.$state = $state;
                    _this.params = params;
                    _this.refresh();
                    return _this;
                }
                PeopleController.prototype.delete = function (person) {
                    this.invoke(person.Delete);
                };
                PeopleController.prototype.refresh = function () {
                    var _this = this;
                    return this.load().then(function () {
                        _this.people = _this.collections["people"];
                    });
                    ;
                };
                return PeopleController;
            }(Pages.Page));
            PeopleController.$inject = ["appService", "$scope", "$state", "$stateParams"];
            angular
                .module("app")
                .controller("relationPeopleController", PeopleController);
        })(Person = Pages.Person || (Pages.Person = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=people.js.map