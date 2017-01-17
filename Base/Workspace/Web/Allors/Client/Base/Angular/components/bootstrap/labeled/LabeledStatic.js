var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledStaticTemplate = (function () {
            function LabeledStaticTemplate() {
            }
            LabeledStaticTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.StaticTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledStaticTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledStaticTemplate.createDefaultView(); }
                templateCache.put(LabeledStaticTemplate.name, view);
            };
            return LabeledStaticTemplate;
        }());
        LabeledStaticTemplate.name = "allors/bootstrap/static-group";
        Bootstrap.LabeledStaticTemplate = LabeledStaticTemplate;
        angular
            .module("allors")
            .component("bLabeledStatic", {
            controller: Bootstrap.StaticController,
            templateUrl: LabeledStaticTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.StaticController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledStatic.js.map