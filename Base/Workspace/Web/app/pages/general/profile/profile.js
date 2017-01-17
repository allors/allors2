var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Profile;
        (function (Profile) {
            var ProfileController = (function (_super) {
                __extends(ProfileController, _super);
                function ProfileController(app, $scope, $state, params) {
                    var _this = _super.call(this, "Profile", app, $scope) || this;
                    _this.$state = $state;
                    _this.params = params;
                    _this.refresh();
                    return _this;
                }
                ProfileController.prototype.reset = function () {
                    this.refresh();
                };
                ProfileController.prototype.refresh = function () {
                    var _this = this;
                    return this.load()
                        .then(function () {
                        _this.person = _this.objects["person"];
                    });
                };
                return ProfileController;
            }(Pages.Page));
            ProfileController.$inject = ["appService", "$scope", "$state", "$stateParams"];
            angular
                .module("app")
                .controller("profileController", ProfileController);
        })(Profile = Pages.Profile || (Pages.Profile = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=profile.js.map