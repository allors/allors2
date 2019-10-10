namespace App.Services {
    export class AllorsService {

        baseUrl = "";

        database: Allors.Database;
        workspace: Allors.Workspace;

        static $inject = ["$log", "$http", "$q", "$rootScope", "$uibModal", "toastr"];
        constructor(public $log: ng.ILogService,
            public $http: ng.IHttpService,
            public $q: ng.IQService,
            public $rootScope: ng.IRootScopeService,
            public $uibModal: angular.ui.bootstrap.IModalService,
            public toastr: angular.toastr.IToastrService) {

            const postfix = "/Pull";

            this.database = new Allors.Database(this.$http, this.$q, postfix, this.baseUrl);
            this.workspace = new Allors.Workspace(Allors.Data.metaPopulation);
        }
    }
    angular.module("app")
        .service("allorsService",
            AllorsService);
}
