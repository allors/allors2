var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledRadioTemplate = (function () {
            function LabeledRadioTemplate() {
            }
            LabeledRadioTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.RadioTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledRadioTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledRadioTemplate.createDefaultView(); }
                templateCache.put(LabeledRadioTemplate.name, view);
            };
            return LabeledRadioTemplate;
        }());
        LabeledRadioTemplate.name = "allors/bootstrap/radio-group";
        Bootstrap.LabeledRadioTemplate = LabeledRadioTemplate;
        angular
            .module("allors")
            .component("bLabeledRadio", {
            controller: Bootstrap.RadioController,
            templateUrl: ["$element", "$attrs", function () { return LabeledRadioTemplate.name; }],
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.RadioController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledRadio.js.map