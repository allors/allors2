var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledCroppedImageTemplate = (function () {
            function LabeledCroppedImageTemplate() {
            }
            LabeledCroppedImageTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n " + Bootstrap.CroppedImageTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledCroppedImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledCroppedImageTemplate.createDefaultView(); }
                templateCache.put(LabeledCroppedImageTemplate.name, view);
            };
            return LabeledCroppedImageTemplate;
        }());
        LabeledCroppedImageTemplate.name = "allors/bootstrap/labeled-cropped-image";
        Bootstrap.LabeledCroppedImageTemplate = LabeledCroppedImageTemplate;
        angular
            .module("allors")
            .component("bLabeledCroppedImage", {
            controller: Bootstrap.CroppedImageController,
            templateUrl: LabeledCroppedImageTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.CroppedImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledCroppedImage.js.map