var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectTemplate = (function () {
            function SelectTemplate() {
            }
            SelectTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.roleType.isOne && $ctrl.options !== undefined\" ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select a value\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isOne && $ctrl.options === undefined\" ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select a value\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isMany && $ctrl.options !== undefined\" multiple ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select values\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isMany && $ctrl.options === undefined\" multiple ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select values\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectTemplate.createDefaultView(); }
                templateCache.put(SelectTemplate.name, view);
            };
            return SelectTemplate;
        }());
        SelectTemplate.name = "allors/bootstrap/select";
        Bootstrap.SelectTemplate = SelectTemplate;
        var SelectController = (function (_super) {
            __extends(SelectController, _super);
            function SelectController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            SelectController.prototype.filterFunction = function (criteria) {
                var _this = this;
                return function (object) {
                    var value = object[_this.display];
                    if (value) {
                        var lowerCaseValue = value.toLowerCase();
                        var lowerCaseCriteria = criteria.toLowerCase();
                        return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
                    }
                    return false;
                };
            };
            SelectController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectController.prototype.refresh = function (criteria) {
                var _this = this;
                this
                    .lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                });
            };
            return SelectController;
        }(Bootstrap.Field));
        SelectController.bindings = {
            object: "<",
            relation: "@",
            display: "@",
            options: "<",
            order: "<",
            refreshDelay: "<",
            lookup: "&lookup"
        };
        SelectController.$inject = ["$log", "$translate"];
        Bootstrap.SelectController = SelectController;
        angular
            .module("allors")
            .component("bSelect", {
            controller: SelectController,
            templateUrl: SelectTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: SelectController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Select.js.map