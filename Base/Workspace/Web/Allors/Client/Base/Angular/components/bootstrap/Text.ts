namespace Allors.Bootstrap {
    export class TextTemplate {
        static name = "allors/bootstrap/text";

        static createDefaultView() {
            return `
<input placeholder="{{$ctrl.placeholder}}" class="form-control"
        ng-model="$ctrl.role"
        ng-disabled="!$ctrl.canWrite"
        ng-required="$ctrl.roleType.isRequired">
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = TextTemplate.createDefaultView()) {
            templateCache.put(TextTemplate.name, view);
        }
    }

    export class TextController extends Field {
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
        .component("bText", {
            controller: TextController,
            templateUrl: TextTemplate.name,
            require: FormController.require,
            bindings: TextController.bindings
        });
}
