var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledImageTemplate = (function () {
            function LabeledImageTemplate() {
            }
            LabeledImageTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n " + Bootstrap.ImageTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledImageTemplate.createDefaultView(); }
                templateCache.put(LabeledImageTemplate.name, view);
            };
            return LabeledImageTemplate;
        }());
        LabeledImageTemplate.name = "allors/bootstrap/labeled-image";
        Bootstrap.LabeledImageTemplate = LabeledImageTemplate;
        angular
            .module("allors")
            .component("bLabeledImage", {
            controller: Bootstrap.ImageController,
            templateUrl: LabeledImageTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.ImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledImage.js.map