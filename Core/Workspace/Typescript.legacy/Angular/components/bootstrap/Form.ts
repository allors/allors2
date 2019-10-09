/// <reference path="../../allors.module.ts" />
namespace Allors.Bootstrap {
    export class FormTemplate {

        static templateName = "allors/bootstrap/form";

        private static view =
`<form name="form" ng-class="$ctrl.horizontal ? 'form-horizontal' : ''">
<ng-transclude />
</form>`;

        static register(templateCache: angular.ITemplateCacheService) {
            templateCache.put(FormTemplate.templateName, FormTemplate.view);
        }
    }

    export class FormController  {

        static require = {
            form: "^bForm"
        } as {[controller: string]: string }

        static bindings = {
            horizontal: "<",
            onRegister: "&"
        } as { [binding: string]: string }

        horizontal: boolean;

        onRegister: (any) => void;

        static $inject = ["$scope", "$log"];

        constructor(private $scope: angular.IScope, private $log: angular.ILogService) {
            $scope.$watch(this.$scope["form"], () => {
                if (this.$scope["form"]) {
                    const form = this.$scope["form"];
                    this.onRegister({form: form});
                }
            });
        }
    }

    angular
        .module("allors")
        .component("bForm", {
            controller: FormController,
            templateUrl: FormTemplate.templateName,
            transclude: true,
            bindings: FormController.bindings
        });
}
