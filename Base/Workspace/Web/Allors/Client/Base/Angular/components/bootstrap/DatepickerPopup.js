var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var DatepickerPopupTemplate = (function () {
            function DatepickerPopupTemplate() {
            }
            DatepickerPopupTemplate.createDefaultView = function () {
                return "\n<p class=\"input-group\">\n    <input type=\"date\" placeholder=\"{{$ctrl.placeholder}}\" class=\"form-control\" datepicker-append-to-body=\"$ctrl.appendToBody\"\n            uib-datepicker-popup \n            is-open=\"$ctrl.opened\" \n            ng-model=\"$ctrl.role\"\n            ng-model-options=\"$ctrl.modelOptions\"\n            ng-disabled=\"!$ctrl.canWrite\"\n            ng-required=\"$ctrl.roleType.isRequired\">\n    <span class=\"input-group-btn\">\n        <button type=\"button\" class=\"btn btn-default\" ng-click=\"$ctrl.opened = true\"><i class=\"glyphicon glyphicon-calendar\"></i></button>\n    </span>\n</p>\n";
            };
            DatepickerPopupTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = DatepickerPopupTemplate.createDefaultView(); }
                templateCache.put(DatepickerPopupTemplate.name, view);
            };
            return DatepickerPopupTemplate;
        }());
        DatepickerPopupTemplate.name = "allors/bootstrap/datepicker-popup";
        Bootstrap.DatepickerPopupTemplate = DatepickerPopupTemplate;
        var DatepickerPopupController = (function (_super) {
            __extends(DatepickerPopupController, _super);
            function DatepickerPopupController($log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                _this.timezone = "UTC";
                _this.modelOptions = {};
                return _this;
            }
            DatepickerPopupController.prototype.$onInit = function () {
                this.modelOptions.timezone = this.timezone;
            };
            return DatepickerPopupController;
        }(Bootstrap.Field));
        DatepickerPopupController.bindings = {
            object: "<",
            relation: "@",
            timezone: "@",
            appendToBody: "<",
        };
        DatepickerPopupController.$inject = ["$log", "$translate"];
        Bootstrap.DatepickerPopupController = DatepickerPopupController;
        angular
            .module("allors")
            .component("bDatepickerPopup", {
            controller: DatepickerPopupController,
            templateUrl: DatepickerPopupTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: DatepickerPopupController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=DatepickerPopup.js.map