namespace App.Pages.Organisation {

    class OrganisationsController extends Page {
      
        organisations: Organisation[];

        static $inject = ["appService", "$scope", "$state", "$stateParams"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService, private params: { id: string }) {
            super("Organisations", app, $scope);

            this.refresh();
        }

        delete(organisation: Organisation) {
            this.invoke(organisation.Delete);
        }
        
        refresh(): ng.IPromise<any> {
            return this.load()
                .then(() => {
                    this.organisations = this.collections["organisations"] as Organisation[];
                });
        }
    }
    angular
        .module("app")
        .controller("relationOrganisationsController",
            OrganisationsController);
}
