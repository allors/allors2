namespace Allors.Bootstrap {
    export class StaticEnumGroupTemplate {
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

        static register(templateCache: angular.ITemplateCacheService, view = StaticEnumGroupTemplate.createDefaultView()) {
            templateCache.put(StaticEnumGroupTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledStaticEnum", {
            controller: StaticEnumController,
            templateUrl: StaticEnumGroupTemplate.name,
            require: FormController.require,
            bindings: StaticEnumController.bindings
        });
}
