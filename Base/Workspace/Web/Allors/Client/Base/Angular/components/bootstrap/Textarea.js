var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextareaTemplate = (function () {
            function TextareaTemplate() {
            }
            TextareaTemplate.createDefaultView = function () {
                return "\n<textarea placeholder=\"{{$ctrl.placeholder}}\" class=\"form-control\"\n        ng-model=\"$ctrl.role\"\n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        rows=\"{{$ctrl.rows}}\">\n";
            };
            TextareaTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextareaTemplate.createDefaultView(); }
                templateCache.put(TextareaTemplate.name, view);
            };
            return TextareaTemplate;
        }());
        TextareaTemplate.name = "allors/bootstrap/textarea";
        Bootstrap.TextareaTemplate = TextareaTemplate;
        var TextareaController = (function (_super) {
            __extends(TextareaController, _super);
            function TextareaController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return TextareaController;
        }(Bootstrap.Field));
        TextareaController.bindings = {
            object: "<",
            relation: "@",
            rows: "<"
        };
        TextareaController.$inject = ["$log", "$translate"];
        Bootstrap.TextareaController = TextareaController;
        angular
            .module("allors")
            .component("bTextarea", {
            controller: TextareaController,
            templateUrl: TextareaTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: TextareaController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Textarea.js.map