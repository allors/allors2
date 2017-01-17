var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextTemplate = (function () {
            function TextTemplate() {
            }
            TextTemplate.createDefaultView = function () {
                return "\n<input placeholder=\"{{$ctrl.placeholder}}\" class=\"form-control\"\n        ng-model=\"$ctrl.role\"\n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\">\n";
            };
            ;
            TextTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextTemplate.createDefaultView(); }
                templateCache.put(TextTemplate.name, view);
            };
            return TextTemplate;
        }());
        TextTemplate.name = "allors/bootstrap/text";
        Bootstrap.TextTemplate = TextTemplate;
        var TextController = (function (_super) {
            __extends(TextController, _super);
            function TextController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return TextController;
        }(Bootstrap.Field));
        TextController.bindings = {
            object: "<",
            relation: "@"
        };
        TextController.$inject = ["$log", "$translate"];
        Bootstrap.TextController = TextController;
        angular
            .module("allors")
            .component("bText", {
            controller: TextController,
            templateUrl: TextTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: TextController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Text.js.map