namespace Allors.Bootstrap {

    export class RadioButtonTemplate {
        static name = "allors/bootstrap/radio-button";

        static createDefaultView() {
            return `
<div class="btn-group">
    <label class="btn btn-info" 
           uib-btn-radio="true" 
           ng-model="$ctrl.role" 
           ng-disabled="!$ctrl.canWrite"
           ng-required="$ctrl.roleType.isRequired">{{$ctrl.trueLabel}}</label>
    <label class="btn btn-info" 
           uib-btn-radio="false"
           ng-model="$ctrl.role"
           ng-disabled="!$ctrl.canWrite"
           ng-required="$ctrl.roleType.isRequired">{{$ctrl.falseLabel}}</label>
</div>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = RadioButtonTemplate.createDefaultView()) {
            templateCache.put(RadioButtonTemplate.name, view);
        }
    }

    export class RadioButtonController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            trueLabel: "@true",
            falseLabel: "@false"
        } as { [binding: string]: string }

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }
    }

    angular
        .module("allors")
        .component("bRadioButton", {
            controller: RadioButtonController,
            templateUrl: RadioButtonTemplate.name,
            require: FormController.require,
            bindings: RadioButtonController.bindings
        });
}
