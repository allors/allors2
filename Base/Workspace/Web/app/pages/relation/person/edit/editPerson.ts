namespace App.Pages.Person.Edit {

    class EditPersonController extends Page {

        person: Person;

        lastNameOptions: string[];

        static $inject = ["appService", "$scope", "$state", "$stateParams"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService, private params: { id: string }) {
            super("EditPerson", app, $scope);

            this.refresh();
        }

        cancel(): void {
            this.$state.go("relation.people");
        }

        save(): ng.IPromise<any> {
            return super.save().then(() => this.$state.go("relation.people"));
        }
        
        protected refresh(): ng.IPromise<any> {
            return this.load({
                    id: this.params.id
                })
                .then(() => {
                    this.person = this.objects["person"] as Person;

                    var persons = this.collections["persons"] as Person[];
                    this.lastNameOptions = _(persons).map(v => v.LastName).uniq().compact().value() || [];
                });
        }
    }
    angular
        .module("app")
        .controller("relationEditPersonController",
            EditPersonController);

}
