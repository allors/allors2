var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$provide"];
    function config($provide) {
        $provide.decorator("taOptions", ["$delegate", function (taOptions) {
                taOptions.toolbar = [
                    ["h1", "h2", "h3"],
                    ["bold", "italics", "underline", "ul"]
                ];
                return taOptions;
            }]);
    }
})(App || (App = {}));
//# sourceMappingURL=app.textangular.js.map