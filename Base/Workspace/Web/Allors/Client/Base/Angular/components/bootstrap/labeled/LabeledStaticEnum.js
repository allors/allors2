var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var StaticEnumGroupTemplate = (function () {
            function StaticEnumGroupTemplate() {
            }
            StaticEnumGroupTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.StaticEnumTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-group>\n";
            };
            StaticEnumGroupTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = StaticEnumGroupTemplate.createDefaultView(); }
                templateCache.put(StaticEnumGroupTemplate.name, view);
            };
            return StaticEnumGroupTemplate;
        }());
        StaticEnumGroupTemplate.name = "allors/bootstrap/labeled-static-enum";
        Bootstrap.StaticEnumGroupTemplate = StaticEnumGroupTemplate;
        angular
            .module("allors")
            .component("bLabeledStaticEnum", {
            controller: Bootstrap.StaticEnumController,
            templateUrl: StaticEnumGroupTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.StaticEnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledStaticEnum.js.map