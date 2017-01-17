var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTypeaheadTemplate = (function () {
            function LabeledTypeaheadTemplate() {
            }
            LabeledTypeaheadTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/> \n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TypeaheadTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTypeaheadTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTypeaheadTemplate.createDefaultView(); }
                templateCache.put(LabeledTypeaheadTemplate.name, view);
            };
            return LabeledTypeaheadTemplate;
        }());
        LabeledTypeaheadTemplate.name = "allors/bootstrap/labeled-typeahead";
        Bootstrap.LabeledTypeaheadTemplate = LabeledTypeaheadTemplate;
        angular
            .module("allors")
            .component("bLabeledTypeahead", {
            controller: Bootstrap.TypeaheadController,
            templateUrl: LabeledTypeaheadTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TypeaheadController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledTypeahead.js.map