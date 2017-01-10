namespace Allors.Bootstrap {
    export class LabeledTextAngularTemplate {
        static name = "allors/bootstrap/labeled-text-angular";

        private static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + TextAngularTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledTextAngularTemplate.createDefaultView()) {
            templateCache.put(LabeledTextAngularTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledTextAngular", {
            controller: TextAngularController,
            templateUrl: LabeledTextAngularTemplate.name,
            require: FormController.require,
            bindings: TextAngularController.bindings
        });
}
