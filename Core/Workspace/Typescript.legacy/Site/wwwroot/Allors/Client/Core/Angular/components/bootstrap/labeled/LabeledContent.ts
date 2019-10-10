/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledContentTemplate {
        static templateName = "allors/bootstrap/labeled-Content";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + ContentTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        };

        static register(templateCache: angular.ITemplateCacheService, view = LabeledContentTemplate.createDefaultView()) {
            templateCache.put(LabeledContentTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledContent", {
            controller: ContentController,
            templateUrl: LabeledContentTemplate.templateName,
            require: FormController.require,
            bindings: ContentController.bindings
        });
}
