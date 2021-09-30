module App {
    var app = angular.module("app");

    app.config(config);

    config.$inject = ["$stateProvider", "$urlRouterProvider", "$locationProvider"];
    function config($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider): void {

        $urlRouterProvider
            .otherwise("/");

        $stateProvider
            // Login
            .state("login",
                {
                    url: "/login",
                    templateUrl: "/app/login/login.html",
                    controller: "loginController",
                    controllerAs: "vm"
                })

            // General
            .state("home",
                {
                    url: "/",
                    templateUrl: "/app/pages/home/index.html",
                    controller: "homeController",
                    controllerAs: "vm"
                })
            .state("person",
                {
                    url: "/person",
                    templateUrl: "/app/pages/person/list.html",
                    controller: "personListController",
                    controllerAs: "vm"
                });
    }
}
