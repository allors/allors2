var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledContentTemplate = (function () {
            function LabeledContentTemplate() {
            }
            LabeledContentTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.ContentTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledContentTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledContentTemplate.createDefaultView(); }
                templateCache.put(LabeledContentTemplate.name, view);
            };
            return LabeledContentTemplate;
        }());
        LabeledContentTemplate.name = "allors/bootstrap/labeled-Content";
        Bootstrap.LabeledContentTemplate = LabeledContentTemplate;
        angular
            .module("allors")
            .component("bLabeledContent", {
            controller: Bootstrap.ContentController,
            templateUrl: LabeledContentTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.ContentController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledContent.js.map