var App;
(function (App) {
    var Home;
    (function (Home) {
        var HomeController = (function () {
            function HomeController() {
            }
            return HomeController;
        }());
        angular
            .module("app")
            .controller("homeController", HomeController);
    })(Home = App.Home || (App.Home = {}));
})(App || (App = {}));
//# sourceMappingURL=home.js.map