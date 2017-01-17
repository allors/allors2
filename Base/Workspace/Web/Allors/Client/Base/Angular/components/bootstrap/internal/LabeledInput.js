var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledInputTemplate = (function () {
            function LabeledInputTemplate() {
            }
            LabeledInputTemplate.register = function (templateCache) {
                templateCache.put(LabeledInputTemplate.name, LabeledInputTemplate.view);
            };
            return LabeledInputTemplate;
        }());
        LabeledInputTemplate.name = "allors/bootstrap/labeled-input";
        LabeledInputTemplate.view = "\n<div ng-class=\"$ctrl.form.horizontal ? 'col-sm-8' : ''\">\n<ng-transclude/>\n</div>\n";
        Bootstrap.LabeledInputTemplate = LabeledInputTemplate;
        var LabeledInputComponent = (function () {
            function LabeledInputComponent($log) {
            }
            return LabeledInputComponent;
        }());
        LabeledInputComponent.$inject = ["$log"];
        angular
            .module("allors")
            .component("bLabeledInput", {
            controller: LabeledInputComponent,
            transclude: true,
            templateUrl: LabeledInputTemplate.name,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=LabeledInput.js.map