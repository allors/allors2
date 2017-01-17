var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Layout;
        (function (Layout) {
            var MainController = (function (_super) {
                __extends(MainController, _super);
                function MainController(app, $scope, $state, params) {
                    var _this = _super.call(this, "Main", app, $scope) || this;
                    _this.$state = $state;
                    _this.params = params;
                    _this.refresh();
                    return _this;
                }
                MainController.prototype.refresh = function () {
                    var _this = this;
                    return this.load()
                        .then(function () {
                        _this.person = _this.objects["person"];
                    });
                };
                return MainController;
            }(Pages.Page));
            MainController.$inject = ["appService", "$scope", "$state", "$stateParams"];
            angular
                .module("app")
                .controller("mainController", MainController);
        })(Layout = Pages.Layout || (Pages.Layout = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=main.js.map