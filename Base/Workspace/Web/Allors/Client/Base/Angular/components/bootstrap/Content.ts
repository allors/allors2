namespace Allors.Bootstrap {
    export class ContentTemplate {
        static name = "allors/bootstrap/content";

        static createDefaultView() {
            return `
<div contenteditable
        ng-model="$ctrl.role"
        ng-disabled="!$ctrl.canWrite"
        ng-required="$ctrl.roleType.isRequired">
</div>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = ContentTemplate.createDefaultView()) {
            templateCache.put(ContentTemplate.name, view);
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
            templateUrl: ContentTemplate.name,
            require: FormController.require,
            bindings: ContentController.bindings
        });
}
