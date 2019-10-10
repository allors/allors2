module App.Pages.Person {
    class ListController {

        static $inject = ["$log", "$state"];
        constructor(private readonly $log: ng.ILogService, public $state: ng.ui.IStateService) {
        }
    }
    angular
        .module("app")
        .controller("personListController",
          ListController);

}
