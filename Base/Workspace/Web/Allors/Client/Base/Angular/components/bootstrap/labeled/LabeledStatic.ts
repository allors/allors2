namespace Allors.Bootstrap {
    export class LabeledStaticTemplate {
        static name = "allors/bootstrap/static-group";

        private static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + StaticTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledStaticTemplate.createDefaultView()) {
            templateCache.put(LabeledStaticTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledStatic", {
            controller: StaticController,
            templateUrl: LabeledStaticTemplate.name,
            require: FormController.require,
            bindings: StaticController.bindings
        });
}
