var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var FormTemplate = (function () {
            function FormTemplate() {
            }
            FormTemplate.register = function (templateCache) {
                templateCache.put(FormTemplate.name, FormTemplate.view);
            };
            return FormTemplate;
        }());
        FormTemplate.name = "allors/bootstrap/form";
        FormTemplate.view = "<form ng-class=\"$ctrl.horizontal ? 'form-horizontal' : ''\">\n<ng-transclude />\n</form>";
        Bootstrap.FormTemplate = FormTemplate;
        var FormController = (function () {
            function FormController($log) {
                this.$log = $log;
            }
            return FormController;
        }());
        FormController.require = {
            form: "^bForm"
        };
        FormController.bindings = {
            horizontal: "<"
        };
        FormController.$inject = ["$log"];
        Bootstrap.FormController = FormController;
        angular
            .module("allors")
            .component("bForm", {
            controller: FormController,
            templateUrl: FormTemplate.name,
            transclude: true,
            bindings: FormController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Form.js.map