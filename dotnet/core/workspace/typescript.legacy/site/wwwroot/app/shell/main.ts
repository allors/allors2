/// <reference path="../pages/Page.ts" />
module App.Shell {

    class MainController extends Page {

        user: Allors.Domain.Person;

        static $inject = ["allorsService", "$scope", "$state"];
        constructor(allors: Services.AllorsService, $scope: ng.IScope, private $state: angular.ui.IStateService) {
            super("Main", allors, $scope);

            this.refresh();

            // Login
            $scope["controller"] = this;
        }

        goto(route: string) {
            this.$state.go(route);
        }


        public refresh(): ng.IPromise<any> {
            return this.load()
                .then(() => {
                    this.user = this.objects["user"] as Allors.Domain.Person;
                })
                .catch((err) => {
                    if (err.status && err.status === 401) {
                        this.$state.go("login");
                    }
                });
        }
    }
    angular
        .module("app")
        .controller("mainController",
        MainController);

}
