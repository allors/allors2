namespace Allors.Bootstrap {
    export class LabeledContentTemplate {
        static name = "allors/bootstrap/labeled-Content";

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
            templateCache.put(LabeledContentTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledContent", {
            controller: ContentController,
            templateUrl: LabeledContentTemplate.name,
            require: FormController.require,
            bindings: ContentController.bindings
        });
}
