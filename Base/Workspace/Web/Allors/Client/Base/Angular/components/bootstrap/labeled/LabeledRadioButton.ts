namespace Allors.Bootstrap {
    export class LabeledRadioButtonTemplate {
        static name = "allors/bootstrap/radio-button-group";

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
            templateCache.put(LabeledRadioButtonTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledRadioButton", {
            controller: RadioButtonController,
            templateUrl: ["$element", "$attrs", () => LabeledRadioButtonTemplate.name],
            require: FormController.require,
            bindings: RadioButtonController.bindings
        });
}
