namespace Allors.Bootstrap {
    export class LabeledRadioTemplate {
        static name = "allors/bootstrap/radio-group";

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
            templateCache.put(LabeledRadioTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledRadio", {
            controller: RadioController,
            templateUrl: ["$element", "$attrs", () => LabeledRadioTemplate.name],
            require: FormController.require,
            bindings: RadioController.bindings
        });
}
