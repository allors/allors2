namespace Allors.Bootstrap {
    export class LabeledStaticEnumTemplate {
        static name = "allors/bootstrap/labeled-static-enum";

        private static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + StaticEnumTemplate.createDefaultView() + `
    </b-labeled-input>
</b-group>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledStaticEnumTemplate.createDefaultView()) {
            templateCache.put(LabeledStaticEnumTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledStaticEnum", {
            controller: StaticEnumController,
            templateUrl: LabeledStaticEnumTemplate.name,
            require: FormController.require,
            bindings: StaticEnumController.bindings
        });
}
