/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledTextareaTemplate {
        static templateName = "allors/bootstrap/labeled-textarea";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + TextareaTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledTextareaTemplate.createDefaultView()) {
            templateCache.put(LabeledTextareaTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledTextarea", {
            controller: TextareaController,
            templateUrl: LabeledTextareaTemplate.templateName,
            require: FormController.require,
            bindings: TextareaController.bindings
        });
}
