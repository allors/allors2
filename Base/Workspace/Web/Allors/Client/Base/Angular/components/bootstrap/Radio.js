var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var RadioTemplate = (function () {
            function RadioTemplate() {
            }
            RadioTemplate.createDefaultView = function () {
                return "\n<label>\n<input type=\"radio\" \n        ng-model=\"$ctrl.role\" \n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        ng-value=\"true\">\n{{$ctrl.trueLabel}}\n</label>\n\n<br/>\n\n<label>\n    <input type=\"radio\" \n        ng-model=\"$ctrl.role\" \n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        ng-value=\"false\">\n    {{$ctrl.falseLabel}}\n</label>\n\n<br/>\n\n\n";
            };
            RadioTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = RadioTemplate.createDefaultView(); }
                templateCache.put(RadioTemplate.name, view);
            };
            return RadioTemplate;
        }());
        RadioTemplate.name = "allors/bootstrap/radio";
        Bootstrap.RadioTemplate = RadioTemplate;
        var RadioController = (function (_super) {
            __extends(RadioController, _super);
            function RadioController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            RadioController.prototype.$onInit = function () {
            };
            return RadioController;
        }(Bootstrap.Field));
        RadioController.bindings = {
            object: "<",
            relation: "@",
            trueLabel: "@true",
            falseLabel: "@false"
        };
        RadioController.$inject = ["$log", "$translate"];
        Bootstrap.RadioController = RadioController;
        angular
            .module("allors")
            .component("bRadio", {
            controller: RadioController,
            templateUrl: RadioTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: RadioController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Radio.js.map