namespace App.Pages.Relation
{
    class RelationController extends Page {

        person: Person;

        static $inject = ["appService", "$scope", "$state", "$stateParams"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService, private params: { id: string }) {
            super("Relation", app, $scope);

            this.refresh();
        }

        protected refresh(): ng.IPromise<any> {
            return this.load()
                .then(() => {
                    this.person = this.objects["person"] as Person;
                });
        }
    }
    angular
        .module("app")
        .controller("relationController",
			RelationController);

}