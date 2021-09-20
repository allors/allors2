/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledRadioTemplate {
        static templateName = "allors/bootstrap/radio-group";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + RadioTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledRadioTemplate.createDefaultView()) {
            templateCache.put(LabeledRadioTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledRadio", {
            controller: RadioController,
            templateUrl: ["$element", "$attrs", () => LabeledRadioTemplate.templateName],
            require: FormController.require,
            bindings: RadioController.bindings
        });
}
