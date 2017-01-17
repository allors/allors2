var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var RadioButtonTemplate = (function () {
            function RadioButtonTemplate() {
            }
            RadioButtonTemplate.createDefaultView = function () {
                return "\n<div class=\"btn-group\">\n    <label class=\"btn btn-info\" \n           uib-btn-radio=\"true\" \n           ng-model=\"$ctrl.role\" \n           ng-disabled=\"!$ctrl.canWrite\"\n           ng-required=\"$ctrl.roleType.isRequired\">{{$ctrl.trueLabel}}</label>\n    <label class=\"btn btn-info\" \n           uib-btn-radio=\"false\"\n           ng-model=\"$ctrl.role\"\n           ng-disabled=\"!$ctrl.canWrite\"\n           ng-required=\"$ctrl.roleType.isRequired\">{{$ctrl.falseLabel}}</label>\n</div>\n";
            };
            RadioButtonTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = RadioButtonTemplate.createDefaultView(); }
                templateCache.put(RadioButtonTemplate.name, view);
            };
            return RadioButtonTemplate;
        }());
        RadioButtonTemplate.name = "allors/bootstrap/radio-button";
        Bootstrap.RadioButtonTemplate = RadioButtonTemplate;
        var RadioButtonController = (function (_super) {
            __extends(RadioButtonController, _super);
            function RadioButtonController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return RadioButtonController;
        }(Bootstrap.Field));
        RadioButtonController.bindings = {
            object: "<",
            relation: "@",
            trueLabel: "@true",
            falseLabel: "@false"
        };
        RadioButtonController.$inject = ["$log", "$translate"];
        Bootstrap.RadioButtonController = RadioButtonController;
        angular
            .module("allors")
            .component("bRadioButton", {
            controller: RadioButtonController,
            templateUrl: RadioButtonTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: RadioButtonController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=RadioButton.js.map