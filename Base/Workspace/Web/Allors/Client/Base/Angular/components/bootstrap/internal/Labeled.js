var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTemplate = (function () {
            function LabeledTemplate() {
            }
            LabeledTemplate.register = function (templateCache) {
                templateCache.put(LabeledTemplate.name, LabeledTemplate.view);
            };
            return LabeledTemplate;
        }());
        LabeledTemplate.name = "allors/bootstrap/labeled";
        LabeledTemplate.view = "<div ng-class=\"{true: 'form-group required', false :'form-group'}[{{$ctrl.field.roleType.isRequired}}]\" ng-if=\"$ctrl.field.canRead\">\n<ng-transclude/>\n</div>";
        Bootstrap.LabeledTemplate = LabeledTemplate;
        var LabeledComponent = (function () {
            function LabeledComponent($log) {
            }
            return LabeledComponent;
        }());
        LabeledComponent.$inject = ["$log"];
        angular
            .module("allors")
            .component("bLabeled", {
            controller: LabeledComponent,
            transclude: true,
            templateUrl: LabeledTemplate.name,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Labeled.js.map