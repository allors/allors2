/// <reference path="../Core/Angular/components/bootstrap/Form.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/ImageModal.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/CroppedImageModal.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Static.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/StaticEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Text.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/TextArea.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/TextAngular.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Enum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Select.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Typeahead.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Image.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/CroppedImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Radio.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/RadioButton.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/DatepickerPopup.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Content.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/Label.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/Labeled.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/LabeledInput.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledContent.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledStaticEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledStatic.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledText.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTextarea.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTextAngular.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledSelect.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTypeahead.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledCroppedImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledRadio.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledRadioButton.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledDatepickerPopup.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/SelectOne.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/SelectMany.ts"/>


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
