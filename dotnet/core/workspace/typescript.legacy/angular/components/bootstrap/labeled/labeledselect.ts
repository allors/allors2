/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {

    export class LabeledSelectTemplate {
        static templateName = "allors/bootstrap/labeled-select";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/> 
    <b-labeled-input field="$ctrl">
` + SelectTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledSelectTemplate.createDefaultView()) {
            templateCache.put(LabeledSelectTemplate.templateName, view);
        }
    }
    
    angular
        .module("allors")
        .component("bLabeledSelect", {
            controller: SelectController,
            templateUrl: LabeledSelectTemplate.templateName,
            require: FormController.require,
            bindings: SelectController.bindings
        });
}
