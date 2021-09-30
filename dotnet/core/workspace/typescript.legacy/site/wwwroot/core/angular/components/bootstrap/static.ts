/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />

namespace Allors.Bootstrap {

    export class StaticTemplate {
        static templateName = "allors/bootstrap/static";

        static createDefaultView() {
            return `
<p class="form-control-static" ng-bind-html="$ctrl.role"></p>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = StaticTemplate.createDefaultView()) {
            templateCache.put(StaticTemplate.templateName, view);
        }
    }

    export class StaticController extends Field {
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
        .component("bStatic", {
            controller: StaticController,
            templateUrl: StaticTemplate.templateName,
            require: FormController.require,
            bindings: StaticController.bindings
        });
}
