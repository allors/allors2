var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledDatepickerPopupTemplate = (function () {
            function LabeledDatepickerPopupTemplate() {
            }
            LabeledDatepickerPopupTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.DatepickerPopupTemplate.createDefaultView() + "\n    </b-input-group>\n</b-labeled>\n";
            };
            LabeledDatepickerPopupTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledDatepickerPopupTemplate.createDefaultView(); }
                templateCache.put(LabeledDatepickerPopupTemplate.name, view);
            };
            return LabeledDatepickerPopupTemplate;
        }());
        LabeledDatepickerPopupTemplate.name = "allors/bootstrap/labeled-datepicker-popup";
        Bootstrap.LabeledDatepickerPopupTemplate = LabeledDatepickerPopupTemplate;
        angular
            .module("allors")
            .component("bLabeledDatepickerPopup", {
            controller: Bootstrap.DatepickerPopupController,
            templateUrl: LabeledDatepickerPopupTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.DatepickerPopupController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledDatepickerPopup.js.map