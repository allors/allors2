/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledEnumTemplate {
        static templateName = "allors/bootstrap/labeled-enum";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + EnumTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = LabeledEnumTemplate.createDefaultView()) {
            templateCache.put(LabeledEnumTemplate.templateName, view);
        }
    }
 
    angular
        .module("allors")
        .component("bLabeledEnum", {
            controller: EnumController,
            templateUrl: LabeledEnumTemplate.templateName,
            require: FormController.require,
            bindings: EnumController.bindings
        });
}
