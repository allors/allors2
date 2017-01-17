var App;
(function (App) {
    var Services;
    (function (Services) {
        var AppService = (function () {
            function AppService($log, $http, $q, $rootScope, toastr) {
                this.$log = $log;
                this.$http = $http;
                this.$q = $q;
                this.$rootScope = $rootScope;
                this.toastr = toastr;
                var prefix = "/Database/";
                var postfix = "/Pull";
                this.database = new Allors.Database(this.$http, this.$q, prefix, postfix);
                this.workspace = new Allors.Workspace(Allors.Data.metaPopulation);
            }
            return AppService;
        }());
        AppService.$inject = ["$log", "$http", "$q", "$rootScope", "toastr"];
        Services.AppService = AppService;
        angular.module("app")
            .service("appService", AppService);
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
//# sourceMappingURL=Allors.js.map