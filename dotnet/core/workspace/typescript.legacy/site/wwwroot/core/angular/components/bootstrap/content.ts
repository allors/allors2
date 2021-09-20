/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {
    export class ContentTemplate {
        static templateName = "allors/bootstrap/content";

        static createDefaultView() {
            return `
<div ng-attr-contenteditable="{{$ctrl.canWrite}}"
        ng-model="$ctrl.role"
        ng-disabled="!$ctrl.canWrite"
        ng-required="$ctrl.roleType.isRequired">
</div>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = ContentTemplate.createDefaultView()) {
            templateCache.put(ContentTemplate.templateName, view);
        }
    }

    export class ContentController extends Field {
        static bindings = {
            object: "<",
            relation: "@"
        } as { [binding: string]: string }

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }
    }

    angular
        .module("allors")
        .component("bContent", {
            controller: ContentController,
            templateUrl: ContentTemplate.templateName,
            require: FormController.require,
            bindings: ContentController.bindings
        });
}
