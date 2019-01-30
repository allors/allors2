/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />

namespace Allors.Bootstrap {

    export class TextareaTemplate {
        static templateName = "allors/bootstrap/textarea";

        static createDefaultView() {
            return `
<textarea placeholder="{{$ctrl.placeholder}}" class="form-control"
        ng-model="$ctrl.role"
        ng-disabled="!$ctrl.canWrite"
        ng-required="$ctrl.roleType.isRequired"
        rows="{{$ctrl.rows}}">
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = TextareaTemplate.createDefaultView()) {
            templateCache.put(TextareaTemplate.templateName, view);
        }
    }

    export class TextareaController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            rows: "<"
        } as { [binding: string]: string }

        rows: number;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }
    }

    angular
        .module("allors")
        .component("bTextarea", {
            controller: TextareaController,
            templateUrl: TextareaTemplate.templateName,
            require: FormController.require,
            bindings: TextareaController.bindings
        });
}
