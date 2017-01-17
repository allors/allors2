var App;
(function (App) {
    templates.$inject = ["$templateCache"];
    function templates($templateCache) {
        Allors.Bootstrap.registerTemplates($templateCache);
    }
    angular
        .module("app")
        .run(templates);
})(App || (App = {}));
//# sourceMappingURL=app.templates.js.map