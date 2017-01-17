var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledSelectTemplate = (function () {
            function LabeledSelectTemplate() {
            }
            LabeledSelectTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/> \n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.SelectTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledSelectTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledSelectTemplate.createDefaultView(); }
                templateCache.put(LabeledSelectTemplate.name, view);
            };
            return LabeledSelectTemplate;
        }());
        LabeledSelectTemplate.name = "allors/bootstrap/labeled-select";
        Bootstrap.LabeledSelectTemplate = LabeledSelectTemplate;
        angular
            .module("allors")
            .component("bLabeledSelect", {
            controller: Bootstrap.SelectController,
            templateUrl: LabeledSelectTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.SelectController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledSelect.js.map