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
            var Edit;
            (function (Edit) {
                var EditOrganisationController = (function (_super) {
                    __extends(EditOrganisationController, _super);
                    function EditOrganisationController(app, $scope, $state, params) {
                        var _this = _super.call(this, "EditOrganisation", app, $scope) || this;
                        _this.$state = $state;
                        _this.params = params;
                        _this.refresh();
                        return _this;
                    }
                    EditOrganisationController.prototype.personTypeAhead = function (criteria) {
                        return this.queryResults("PersonTypeAhead", { criteria: criteria });
                    };
                    EditOrganisationController.prototype.cancel = function () {
                        this.$state.go("relation.organisations");
                    };
                    EditOrganisationController.prototype.refresh = function () {
                        var _this = this;
                        return this.load({
                            id: this.params.id
                        })
                            .then(function () {
                            _this.organisation = _this.objects["organisation"];
                            _this.people = _this.collections["people"];
                        });
                    };
                    return EditOrganisationController;
                }(Pages.Page));
                EditOrganisationController.$inject = ["appService", "$scope", "$state", "$stateParams"];
                angular
                    .module("app")
                    .controller("relationEditOrganisationController", EditOrganisationController);
            })(Edit = Organisation.Edit || (Organisation.Edit = {}));
        })(Organisation = Pages.Organisation || (Pages.Organisation = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=editOrganisation.js.map