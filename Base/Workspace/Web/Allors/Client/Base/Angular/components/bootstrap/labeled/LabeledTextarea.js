var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextareaTemplate = (function () {
            function LabeledTextareaTemplate() {
            }
            LabeledTextareaTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextareaTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTextareaTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextareaTemplate.createDefaultView(); }
                templateCache.put(LabeledTextareaTemplate.name, view);
            };
            return LabeledTextareaTemplate;
        }());
        LabeledTextareaTemplate.name = "allors/bootstrap/labeled-textarea";
        Bootstrap.LabeledTextareaTemplate = LabeledTextareaTemplate;
        angular
            .module("allors")
            .component("bLabeledTextarea", {
            controller: Bootstrap.TextareaController,
            templateUrl: LabeledTextareaTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextareaController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledTextarea.js.map