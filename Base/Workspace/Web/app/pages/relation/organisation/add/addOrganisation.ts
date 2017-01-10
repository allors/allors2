namespace App.Pages.Organisation.Add {

    class AddOrganisationController extends Page {
     
        organisation: Organisation;
     
        static $inject = ["appService", "$scope", "$state"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService) {
            super("AddOrganisation", app, $scope);

            this.refresh()
                .then(() => {
                    this.organisation = this.session.create("Organisation") as Organisation;
                });
        }

        cancel(): void {
            this.$state.go("relation.organisations");
        }
        
        protected refresh(): ng.IPromise<any> {
            return this.load();
        }
    }
    angular
        .module("app")
        .controller("relationAddOrganisationController",
            AddOrganisationController);

}
