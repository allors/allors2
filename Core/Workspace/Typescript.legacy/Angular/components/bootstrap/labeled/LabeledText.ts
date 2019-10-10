/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledTextTemplate {
        static templateName = "allors/bootstrap/labeled-text";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + TextTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = LabeledTextTemplate.createDefaultView()) {
            templateCache.put(LabeledTextTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledText", {
            controller: TextController,
            templateUrl: LabeledTextTemplate.templateName,
            require: FormController.require,
            bindings: TextController.bindings
        });
}
