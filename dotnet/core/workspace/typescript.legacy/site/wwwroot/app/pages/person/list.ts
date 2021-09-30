module App.Pages.Person {
  class ListController extends Page {

    people: Allors.Domain.Person[];

    static $inject = ["allorsService", "$scope", "$state", "$stateParams"];
    constructor(public allors: Services.AllorsService, $scope: ng.IScope, private $state: ng.ui.IStateService) {
      super("People", allors, $scope);

      this.refresh();
    }

    protected refresh(): ng.IPromise<any> {
      return this.load()
        .then(() => {
          this.people = this.collections["people"] as Allors.Domain.Person[];
        }
        ).catch((error) => {
          this.errorResponse(error);
        });;
    }
  }
  angular
    .module("app")
    .controller("personListController",
      ListController);

}
