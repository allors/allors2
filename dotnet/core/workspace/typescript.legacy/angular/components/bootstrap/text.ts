/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {
    export class TextTemplate {
        static templateName = "allors/bootstrap/text";

        static createDefaultView() {
            return `
<div class="input-group">
    <input placeholder="{{$ctrl.placeholder}}" 
            class="form-control"
            name="{{$ctrl.name || $ctrl.relation}}"
            ng-model="$ctrl.role"
            ng-disabled="!$ctrl.canWrite"
            ng-required="$ctrl.roleType.isRequired"
            ng-attr-type="{{$ctrl.htmlType}}">
    <span ng-if="$ctrl.addon" class="input-group-addon">{{$ctrl.addon}}</span>
</div>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = TextTemplate.createDefaultView()) {
            templateCache.put(TextTemplate.templateName, view);
        }
    }

    export class TextController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            addon: "<"
        } as { [binding: string]: string }

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
        }

        get htmlType(): string {
            if (this.roleType) {
                if (this.roleType.objectType === "Integer" ||
                    this.roleType.objectType === "Decimal" ||
                    this.roleType.objectType === "Float") {
                    return "number";
                }
            }
            return "text";
        }   
    }

    angular
        .module("allors")
        .component("bText", {
            controller: TextController,
            templateUrl: TextTemplate.templateName,
            require: FormController.require,
            bindings: TextController.bindings
        });
}
