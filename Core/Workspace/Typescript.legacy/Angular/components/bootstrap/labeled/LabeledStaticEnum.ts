/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledStaticEnumTemplate {
        static templateName = "allors/bootstrap/labeled-static-enum";

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
            templateCache.put(LabeledStaticEnumTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledStaticEnum", {
            controller: StaticEnumController,
            templateUrl: LabeledStaticEnumTemplate.templateName,
            require: FormController.require,
            bindings: StaticEnumController.bindings
        });
}
