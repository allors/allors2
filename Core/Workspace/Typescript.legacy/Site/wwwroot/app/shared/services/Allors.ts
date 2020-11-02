namespace App.Services {
    import Data = Allors.Meta.Data;
    import MetaPopulation = Allors.Meta.MetaPopulation;
    import Workspace = Allors.Workspace;
    import Domain = Allors.Domain;

    export class AllorsService {

        baseUrl = "allors/";

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

            const metaPopulation = new MetaPopulation(Data.data);
            const workspace = new Workspace(metaPopulation);
            Domain.domain.apply(workspace);

            this.database = new Allors.Database(this.$http, this.$q, postfix, this.baseUrl);
            this.workspace = new Allors.Workspace(metaPopulation);
        }
    }
    angular.module("app")
        .service("allorsService",
            AllorsService);
}
