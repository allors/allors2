/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {

    export class LabeledTypeaheadTemplate {
        static templateName = "allors/bootstrap/labeled-typeahead";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/> 
    <b-labeled-input field="$ctrl">
` + TypeaheadTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledTypeaheadTemplate.createDefaultView()) {
            templateCache.put(LabeledTypeaheadTemplate.templateName, view);
        }
    }
    
    angular
        .module("allors")
        .component("bLabeledTypeahead", {
            controller: TypeaheadController,
            templateUrl: LabeledTypeaheadTemplate.templateName,
            require: FormController.require,
            bindings: TypeaheadController.bindings
        });
}
