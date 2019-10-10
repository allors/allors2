/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />

namespace Allors.Bootstrap {

    export class TypeaheadTemplate {
        static templateName = "allors/bootstrap/typeahead";

        static createDefaultView() {
            return `
<input  type="text"
        ng-disabled="!$ctrl.canWrite" 
        ng-required="$ctrl.roleType.isRequired" 
        ng-model="$ctrl.role"
        uib-typeahead="item for item in $ctrl.options | filter:$viewValue | limitTo:10"
        class="form-control" />
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = TypeaheadTemplate.createDefaultView()) {
            templateCache.put(TypeaheadTemplate.templateName, view);
        }
    }

    export class TypeaheadController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            options: "<"
        } as { [binding: string]: string }

        options: string[];

        remove: () => void;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }
    }

    angular
        .module("allors")
        .component("bTypeahead", {
            controller: TypeaheadController,
            templateUrl: TypeaheadTemplate.templateName,
            require: FormController.require,
            bindings: TypeaheadController.bindings
        });
}
