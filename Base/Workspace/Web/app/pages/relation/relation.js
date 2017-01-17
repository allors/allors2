var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Relation;
        (function (Relation) {
            var RelationController = (function (_super) {
                __extends(RelationController, _super);
                function RelationController(app, $scope, $state, params) {
                    var _this = _super.call(this, "Relation", app, $scope) || this;
                    _this.$state = $state;
                    _this.params = params;
                    _this.refresh();
                    return _this;
                }
                RelationController.prototype.refresh = function () {
                    var _this = this;
                    return this.load()
                        .then(function () {
                        _this.person = _this.objects["person"];
                    });
                };
                return RelationController;
            }(Pages.Page));
            RelationController.$inject = ["appService", "$scope", "$state", "$stateParams"];
            angular
                .module("app")
                .controller("relationController", RelationController);
        })(Relation = Pages.Relation || (Pages.Relation = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=relation.js.map