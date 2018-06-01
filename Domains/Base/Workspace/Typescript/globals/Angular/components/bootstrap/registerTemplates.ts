/// <reference path="Form.ts"/>
/// <reference path="ImageModal.ts"/>
/// <reference path="CroppedImageModal.ts"/>
/// <reference path="Static.ts"/>
/// <reference path="StaticEnum.ts"/>
/// <reference path="Text.ts"/>
/// <reference path="TextArea.ts"/>
/// <reference path="TextAngular.ts"/>
/// <reference path="Enum.ts"/>
/// <reference path="Select.ts"/>
/// <reference path="Typeahead.ts"/>
/// <reference path="Image.ts"/>
/// <reference path="CroppedImage.ts"/>
/// <reference path="Radio.ts"/>
/// <reference path="RadioButton.ts"/>
/// <reference path="DatepickerPopup.ts"/>
/// <reference path="Content.ts"/>
/// <reference path="internal/Label.ts"/>
/// <reference path="internal/Labeled.ts"/>
/// <reference path="internal/LabeledInput.ts"/>
/// <reference path="labeled/LabeledContent.ts"/>
/// <reference path="labeled/LabeledStaticEnum.ts"/>
/// <reference path="labeled/LabeledStatic.ts"/>
/// <reference path="labeled/LabeledText.ts"/>
/// <reference path="labeled/LabeledTextarea.ts"/>
/// <reference path="labeled/LabeledTextAngular.ts"/>
/// <reference path="labeled/LabeledEnum.ts"/>
/// <reference path="labeled/LabeledSelect.ts"/>
/// <reference path="labeled/LabeledTypeahead.ts"/>
/// <reference path="labeled/LabeledImage.ts"/>
/// <reference path="labeled/LabeledCroppedImage.ts"/>
/// <reference path="labeled/LabeledRadio.ts"/>
/// <reference path="labeled/LabeledRadioButton.ts"/>
/// <reference path="labeled/LabeledDatepickerPopup.ts"/>
/// <reference path="SelectOne.ts"/>
/// <reference path="SelectMany.ts"/>

namespace Allors.Bootstrap {

    export function registerTemplates($templateCache: angular.ITemplateCacheService): void {

        // Form
        // ----
        FormTemplate.register($templateCache);

        // Object
        // ------
        // Shared
        // ------
        ImageModalTemplate.register($templateCache);
        CroppedImageModalTemplate.register($templateCache);

        // Fields
        // ------
        StaticTemplate.register($templateCache);
        StaticEnumTemplate.register($templateCache);
        TextTemplate.register($templateCache);
        TextareaTemplate.register($templateCache);
        TextAngularTemplate.register($templateCache);
        EnumTemplate.register($templateCache);
        SelectTemplate.register($templateCache);
        TypeaheadTemplate.register($templateCache);
        ImageTemplate.register($templateCache);
        CroppedImageTemplate.register($templateCache);
        RadioTemplate.register($templateCache);
        RadioButtonTemplate.register($templateCache);
        DatepickerPopupTemplate.register($templateCache);
        ContentTemplate.register($templateCache);

        // Field Groups
        // ------------
        // Internals
        LabeledTemplate.register($templateCache);
        LabelTemplate.register($templateCache);
        LabeledInputTemplate.register($templateCache);
        // Controls
        LabeledStaticEnumTemplate.register($templateCache);
        LabeledStaticTemplate.register($templateCache);
        LabeledTextTemplate.register($templateCache);
        LabeledTextareaTemplate.register($templateCache);
        LabeledTextAngularTemplate.register($templateCache);
        LabeledEnumTemplate.register($templateCache);
        LabeledSelectTemplate.register($templateCache);
        LabeledTypeaheadTemplate.register($templateCache);
        LabeledImageTemplate.register($templateCache);
        LabeledCroppedImageTemplate.register($templateCache);
        LabeledRadioTemplate.register($templateCache);
        LabeledRadioButtonTemplate.register($templateCache);
        LabeledDatepickerPopupTemplate.register($templateCache);
        LabeledContentTemplate.register($templateCache);

        // Model
        // -----
        SelectOneTemplate.register($templateCache);
        SelectManyTemplate.register($templateCache);
    }
}