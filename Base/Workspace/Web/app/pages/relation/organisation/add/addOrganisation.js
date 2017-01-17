var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Organisation;
        (function (Organisation) {
            var Add;
            (function (Add) {
                var AddOrganisationController = (function (_super) {
                    __extends(AddOrganisationController, _super);
                    function AddOrganisationController(app, $scope, $state) {
                        var _this = _super.call(this, "AddOrganisation", app, $scope) || this;
                        _this.$state = $state;
                        _this.refresh()
                            .then(function () {
                            _this.organisation = _this.session.create("Organisation");
                        });
                        return _this;
                    }
                    AddOrganisationController.prototype.cancel = function () {
                        this.$state.go("relation.organisations");
                    };
                    AddOrganisationController.prototype.refresh = function () {
                        return this.load();
                    };
                    return AddOrganisationController;
                }(Pages.Page));
                AddOrganisationController.$inject = ["appService", "$scope", "$state"];
                angular
                    .module("app")
                    .controller("relationAddOrganisationController", AddOrganisationController);
            })(Add = Organisation.Add || (Organisation.Add = {}));
        })(Organisation = Pages.Organisation || (Pages.Organisation = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=addOrganisation.js.map