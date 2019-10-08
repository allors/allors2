/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {

    export class DatepickerPopupTemplate {
        static templateName = "allors/bootstrap/datepicker-popup";

        static createDefaultView() {
            return `
<p class="input-group">
    <input type="date" placeholder="{{$ctrl.placeholder}}" class="form-control" datepicker-append-to-body="$ctrl.appendToBody"
            uib-datepicker-popup 
            is-open="$ctrl.opened" 
            ng-model="$ctrl.role"
            ng-model-options="$ctrl.modelOptions"
            ng-disabled="!$ctrl.canWrite"
            ng-required="$ctrl.roleType.isRequired">
    <span class="input-group-btn">
        <button type="button" class="btn btn-default" ng-click="$ctrl.opened = true"><i class="glyphicon glyphicon-calendar"></i></button>
    </span>
</p>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = DatepickerPopupTemplate.createDefaultView()) {
            templateCache.put(DatepickerPopupTemplate.templateName, view);
        }
    }

    export class DatepickerPopupController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            timezone: "@",
            appendToBody: "<",
        } as { [binding: string]: string }

        timezone = "UTC";
        modelOptions: any = {};
        opened: boolean;
        appendToBody: boolean;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }

        $onInit() {
            this.modelOptions.timezone = this.timezone;
        }
    }

    angular
        .module("allors")
        .component("bDatepickerPopup", {
            controller: DatepickerPopupController,
            templateUrl: DatepickerPopupTemplate.templateName,
            require: FormController.require,
            bindings: DatepickerPopupController.bindings
        });
}
