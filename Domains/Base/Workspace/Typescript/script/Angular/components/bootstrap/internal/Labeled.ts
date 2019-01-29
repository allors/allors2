/// <reference path="../../../allors.module.ts" />
namespace Allors.Bootstrap {
    export class LabeledTemplate {
        static templateName = "allors/bootstrap/labeled";

        private static view = 
`<div ng-class="{true: 'form-group required', false :'form-group'}[{{$ctrl.field.roleType.isRequired}}]" ng-if="$ctrl.field.canRead">
<ng-transclude/>
</div>`;

        static register(templateCache: angular.ITemplateCacheService) {
            templateCache.put(LabeledTemplate.templateName, LabeledTemplate.view);
        }
    }

    class LabeledComponent {
        field: Field;

        static $inject = ["$log"];
        constructor($log: angular.ILogService) {
        }
    }

    angular
        .module("allors")
        .component("bLabeled", {
            controller: LabeledComponent,
            transclude: true,
            templateUrl: LabeledTemplate.templateName,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
}
