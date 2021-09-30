/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {
    export class LabeledCroppedImageTemplate {
        static templateName = "allors/bootstrap/labeled-cropped-image";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
 ` + CroppedImageTemplate.createDefaultView() + `
    </b-labeled-input>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledCroppedImageTemplate.createDefaultView()) {
            templateCache.put(LabeledCroppedImageTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledCroppedImage", {
            controller: CroppedImageController,
            templateUrl: LabeledCroppedImageTemplate.templateName,
            require: FormController.require,
            bindings: CroppedImageController.bindings
        });
}
