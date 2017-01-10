namespace Allors.Bootstrap {
    export class LabeledImageTemplate {
        static name = "allors/bootstrap/labeled-image";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
 ` + ImageTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledImageTemplate.createDefaultView()) {
            templateCache.put(LabeledImageTemplate.name, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledImage", {
            controller: ImageController,
            templateUrl: LabeledImageTemplate.name,
            require: FormController.require,
            bindings: ImageController.bindings
        });
}
