/// <reference path="../../../allors.module.ts" />
namespace Allors.Bootstrap {
    export class LabeledInputTemplate {
        static templatename = "allors/bootstrap/labeled-input";

        private static view = 
`
<div ng-class="$ctrl.form.horizontal ? 'col-sm-8' : ''">
<ng-transclude/>
</div>
`;

        static register(templateCache: angular.ITemplateCacheService) {
            templateCache.put(LabeledInputTemplate.templatename, LabeledInputTemplate.view);
        }
    }

    class LabeledInputComponent {
        field: Field;

        static $inject = ["$log"];
        constructor($log: angular.ILogService) {
        }
    }

    angular
        .module("allors")
        .component("bLabeledInput", {
            controller: LabeledInputComponent,
            transclude: true,
            templateUrl: LabeledInputTemplate.templatename,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
}
