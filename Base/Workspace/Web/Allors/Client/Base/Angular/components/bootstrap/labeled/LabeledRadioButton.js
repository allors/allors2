var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledRadioButtonTemplate = (function () {
            function LabeledRadioButtonTemplate() {
            }
            LabeledRadioButtonTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.RadioButtonTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledRadioButtonTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledRadioButtonTemplate.createDefaultView(); }
                templateCache.put(LabeledRadioButtonTemplate.name, view);
            };
            return LabeledRadioButtonTemplate;
        }());
        LabeledRadioButtonTemplate.name = "allors/bootstrap/radio-button-group";
        Bootstrap.LabeledRadioButtonTemplate = LabeledRadioButtonTemplate;
        angular
            .module("allors")
            .component("bLabeledRadioButton", {
            controller: Bootstrap.RadioButtonController,
            templateUrl: ["$element", "$attrs", function () { return LabeledRadioButtonTemplate.name; }],
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.RadioButtonController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledRadioButton.js.map