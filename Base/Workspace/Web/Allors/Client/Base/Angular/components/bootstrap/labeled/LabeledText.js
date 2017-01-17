var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextTemplate = (function () {
            function LabeledTextTemplate() {
            }
            LabeledTextTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledTextTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextTemplate.createDefaultView(); }
                templateCache.put(LabeledTextTemplate.name, view);
            };
            return LabeledTextTemplate;
        }());
        LabeledTextTemplate.name = "allors/bootstrap/labeled-text";
        Bootstrap.LabeledTextTemplate = LabeledTextTemplate;
        angular
            .module("allors")
            .component("bLabeledText", {
            controller: Bootstrap.TextController,
            templateUrl: LabeledTextTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledText.js.map