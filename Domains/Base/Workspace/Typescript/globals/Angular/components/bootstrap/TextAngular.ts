/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />

namespace Allors.Bootstrap {

    export class TextAngularTemplate {
        static templateName = "allors/bootstrap/text-angular";

        static createDefaultView() {
            return `
<div ng-if="$ctrl.canWrite">
<text-angular ng-model="$ctrl.role" '/>
</div>
<div ng-if="!$ctrl.canWrite">
<text-angular ta-bind="text" ng-model="$ctrl.role" '/>
</div>`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = TextAngularTemplate.createDefaultView()) {
            templateCache.put(TextAngularTemplate.templateName, view);
        }
    }

    export class TextAngularController extends Field {
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
        .component("bTextAngular", {
            controller: TextAngularController,
            templateUrl: TextAngularTemplate.templateName,
            require: FormController.require,
            bindings: TextAngularController.bindings
        });
}
