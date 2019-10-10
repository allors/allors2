/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
namespace Allors.Bootstrap {

    export class LabeledDatepickerPopupTemplate {
        static templateName = "allors/bootstrap/labeled-datepicker-popup";

        static createDefaultView() {
            return `
<b-labeled field="$ctrl">
    <b-label field="$ctrl"/>
    <b-labeled-input field="$ctrl">
` + DatepickerPopupTemplate.createDefaultView() + `
    </b-input-group>
</b-labeled>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = LabeledDatepickerPopupTemplate.createDefaultView()) {
            templateCache.put(LabeledDatepickerPopupTemplate.templateName, view);
        }
    }

    angular
        .module("allors")
        .component("bLabeledDatepickerPopup", {
            controller: DatepickerPopupController,
            templateUrl: LabeledDatepickerPopupTemplate.templateName,
            require: FormController.require,
            bindings: DatepickerPopupController.bindings

        });
}
