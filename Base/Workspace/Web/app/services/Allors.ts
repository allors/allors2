namespace App.Services {
    export class AppService {
        database: Allors.Database;
        workspace: Allors.Workspace;

        static $inject = ["$log", "$http", "$q", "$rootScope", "toastr"];
        constructor(public $log: ng.ILogService, public $http: ng.IHttpService, public $q: ng.IQService, public $rootScope: ng.IRootScopeService, public toastr: angular.toastr.IToastrService) {
            const prefix = "/Database/";
            const postfix = "/Pull";
            this.database = new Allors.Database(this.$http, this.$q, prefix, postfix);
            this.workspace = new Allors.Workspace(Allors.Data.metaPopulation);
        }
    }
    angular.module("app")
        .service("appService",
        AppService);
}