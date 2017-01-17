var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextAngularTemplate = (function () {
            function LabeledTextAngularTemplate() {
            }
            LabeledTextAngularTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextAngularTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTextAngularTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextAngularTemplate.createDefaultView(); }
                templateCache.put(LabeledTextAngularTemplate.name, view);
            };
            return LabeledTextAngularTemplate;
        }());
        LabeledTextAngularTemplate.name = "allors/bootstrap/labeled-text-angular";
        Bootstrap.LabeledTextAngularTemplate = LabeledTextAngularTemplate;
        angular
            .module("allors")
            .component("bLabeledTextAngular", {
            controller: Bootstrap.TextAngularController,
            templateUrl: LabeledTextAngularTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextAngularController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledTextAngular.js.map