module App.General.Home {
    class IndexController {

        static $inject = ["$log", "$state"];
        constructor(private $log: ng.ILogService, public $state: ng.ui.IStateService) {
        }
    }
    angular
        .module("app")
        .controller("homeController",
            IndexController);

}
