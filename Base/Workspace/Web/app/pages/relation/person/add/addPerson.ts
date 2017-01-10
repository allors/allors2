namespace App.Pages.Person.Add {
    class AddPersonController extends Page {

        person: Person;
     
        static $inject = ["appService", "$scope", "$state"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService) {
            super("AddPerson", app, $scope);

            this.refresh()
                .then(() => {
                    this.person = this.session.create("Person") as Person;
                });
        }
       
        cancel(): void {
            this.$state.go("relation.people");
        }
        
        protected refresh(): ng.IPromise<any> {
            return this.load();
        }
    }
    angular
        .module("app")
        .controller("relationAddPersonController",
            AddPersonController);
}
