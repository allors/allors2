var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledEnumTemplate = (function () {
            function LabeledEnumTemplate() {
            }
            LabeledEnumTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.EnumTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledEnumTemplate.createDefaultView(); }
                templateCache.put(LabeledEnumTemplate.name, view);
            };
            return LabeledEnumTemplate;
        }());
        LabeledEnumTemplate.name = "allors/bootstrap/labeled-enum";
        Bootstrap.LabeledEnumTemplate = LabeledEnumTemplate;
        angular
            .module("allors")
            .component("bLabeledEnum", {
            controller: Bootstrap.EnumController,
            templateUrl: LabeledEnumTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.EnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledEnum.js.map