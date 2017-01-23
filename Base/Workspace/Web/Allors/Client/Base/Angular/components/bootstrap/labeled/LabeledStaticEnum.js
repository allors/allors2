var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledStaticEnumTemplate = (function () {
            function LabeledStaticEnumTemplate() {
            }
            LabeledStaticEnumTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.StaticEnumTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-group>\n";
            };
            LabeledStaticEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledStaticEnumTemplate.createDefaultView(); }
                templateCache.put(LabeledStaticEnumTemplate.name, view);
            };
            return LabeledStaticEnumTemplate;
        }());
        LabeledStaticEnumTemplate.name = "allors/bootstrap/labeled-static-enum";
        Bootstrap.LabeledStaticEnumTemplate = LabeledStaticEnumTemplate;
        angular
            .module("allors")
            .component("bLabeledStaticEnum", {
            controller: Bootstrap.StaticEnumController,
            templateUrl: LabeledStaticEnumTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.StaticEnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledStaticEnum.js.map