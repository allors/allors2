namespace App
{
    var app = angular.module("app");
    
    app.config(config);

    config.$inject = ["$stateProvider", "$urlRouterProvider"];
    function config($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider): void {

        /////////////////////////////
        // Redirects and Otherwise //
        /////////////////////////////

        // Use $urlRouterProvider to configure any redirects (when) and invalid urls (otherwise).
        $urlRouterProvider

            // The `when` method says if the url is ever the 1st param, then redirect to the 2nd param
            // Here we are just setting up some convenience urls.
            //.when("/user/:id', '/contacts/:id')

            // If the url is ever invalid, e.g. '/asdf', then redirect to '/' aka the home state
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

            // Relation
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
}
