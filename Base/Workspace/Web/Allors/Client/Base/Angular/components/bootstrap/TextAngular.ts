namespace Allors.Bootstrap {

    export class TextAngularTemplate {
        static name = "allors/bootstrap/text-angular";

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
            templateCache.put(TextAngularTemplate.name, view);
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
            templateUrl: TextAngularTemplate.name,
            require: FormController.require,
            bindings: TextAngularController.bindings
        });
}
