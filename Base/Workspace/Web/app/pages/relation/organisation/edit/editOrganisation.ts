namespace App.Pages.Organisation.Edit {
    class EditOrganisationController extends Page {

        organisation: Organisation;
        people: Person[];

        static $inject = ["appService", "$scope", "$state", "$stateParams"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService, private params: { id: string }) {
            super("EditOrganisation", app, $scope);

            this.refresh();
        }

        personTypeAhead(criteria: string): angular.IPromise<any> {
            return this.queryResults("PersonTypeAhead", {criteria: criteria});
        }

        cancel(): void {
            this.$state.go("relation.organisations");
        }

        protected refresh(): ng.IPromise<any> {
            return this.load({
                    id: this.params.id
                })
                .then(() => {
                    this.organisation = this.objects["organisation"] as Organisation;
                    this.people = this.collections["people"] as Person[];
                });
        }
    }
    angular
        .module("app")
        .controller("relationEditOrganisationController",
            EditOrganisationController);

}
