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
            var OrganisationsController = (function (_super) {
                __extends(OrganisationsController, _super);
                function OrganisationsController(app, $scope, $state, params) {
                    var _this = _super.call(this, "Organisations", app, $scope) || this;
                    _this.$state = $state;
                    _this.params = params;
                    _this.refresh();
                    return _this;
                }
                OrganisationsController.prototype.delete = function (organisation) {
                    this.invoke(organisation.Delete);
                };
                OrganisationsController.prototype.refresh = function () {
                    var _this = this;
                    return this.load()
                        .then(function () {
                        _this.organisations = _this.collections["organisations"];
                    });
                };
                return OrganisationsController;
            }(Pages.Page));
            OrganisationsController.$inject = ["appService", "$scope", "$state", "$stateParams"];
            angular
                .module("app")
                .controller("relationOrganisationsController", OrganisationsController);
        })(Organisation = Pages.Organisation || (Pages.Organisation = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=organisations.js.map