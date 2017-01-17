var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$stateProvider", "$urlRouterProvider"];
    function config($stateProvider, $urlRouterProvider) {
        /////////////////////////////
        // Redirects and Otherwise //
        /////////////////////////////
        // Use $urlRouterProvider to configure any redirects (when) and invalid urls (otherwise).
        $urlRouterProvider
            .otherwise("/");
        //////////////////////////
        // State Configurations //
        //////////////////////////
        $stateProvider
            .state("home", {
            url: "/",
            templateUrl: "/app/pages/general/home/home.html",
            controller: "homeController",
            controllerAs: "vm"
        })
            .state("profile", {
            url: "/profile",
            templateUrl: "/app/pages/general/profile/profile.html",
            controller: "profileController",
            controllerAs: "vm"
        })
            .state("relation", {
            url: "/relation",
            templateUrl: "/app/pages/relation/relation.html",
            controller: "relationController",
            controllerAs: "vm"
        })
            .state("relation.people", {
            url: "/people",
            templateUrl: "/app/pages/relation/person/people.html",
            controller: "relationPeopleController",
            controllerAs: "vm"
        })
            .state("relation.people.add", {
            url: "/add",
            templateUrl: "/app/pages/relation/person/add/addPerson.html",
            controller: "relationAddPersonController",
            controllerAs: "vm"
        })
            .state("relation.people.edit", {
            url: "/edit/:id",
            templateUrl: "/app/pages/relation/person/edit/editPerson.html",
            controller: "relationEditPersonController",
            controllerAs: "vm"
        })
            .state("relation.organisations", {
            url: "/organisations",
            templateUrl: "/app/pages/relation/organisation/organisations.html",
            controller: "relationOrganisationsController",
            controllerAs: "vm"
        })
            .state("relation.organisations.add", {
            url: "/add",
            templateUrl: "/app/pages/relation/organisation/add/addOrganisation.html",
            controller: "relationAddOrganisationController",
            controllerAs: "vm"
        })
            .state("relation.organisations.edit", {
            url: "/editOrganisation/:id",
            templateUrl: "/app/pages/relation/organisation/edit/editOrganisation.html",
            controller: "relationEditOrganisationController",
            controllerAs: "vm"
        });
        ;
    }
})(App || (App = {}));
//# sourceMappingURL=app.state.js.map