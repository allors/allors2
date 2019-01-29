/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledRadioButtonTemplate {
        static templateName = "allors/bootstrap/radio-button-group";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + RadioButtonTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledRadioButtonTemplate.createDefaultView()) {
            templateCache.put(LabeledRadioButtonTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledRadioButton", {
            controller: RadioButtonController,
            templateUrl: ["$element", "$attrs", () => LabeledRadioButtonTemplate.templateName],
            require: FormController.require,
            bindings: RadioButtonController.bindings
        });
}
