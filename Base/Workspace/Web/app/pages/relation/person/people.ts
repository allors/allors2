namespace App.Pages.Person {
    class PeopleController extends Page {
      
        people: Person[];

        static $inject = ["appService", "$scope", "$state", "$stateParams"];
        constructor(app: Services.AppService, $scope: ng.IScope, private $state: ng.ui.IStateService, private params: { id: string }) {
            super("People", app, $scope);

            this.refresh();
        }

        delete(person: Person) {
            this.invoke(person.Delete);
        }
        
        refresh(): ng.IPromise<any> {
            return this.load().then(() => {
                this.people = this.collections["people"] as Person[];
            });;
        }
    }
    angular
        .module("app")
        .controller("relationPeopleController",
            PeopleController);
}
