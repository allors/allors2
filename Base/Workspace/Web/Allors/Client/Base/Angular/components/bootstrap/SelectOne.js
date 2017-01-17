var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectOneTemplate = (function () {
            function SelectOneTemplate() {
            }
            SelectOneTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.options !== undefined\" ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\" >\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.options === undefined\" ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\" >\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectOneTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectOneTemplate.createDefaultView(); }
                templateCache.put(SelectOneTemplate.name, view);
            };
            return SelectOneTemplate;
        }());
        SelectOneTemplate.name = "allors/bootstrap/select-one";
        Bootstrap.SelectOneTemplate = SelectOneTemplate;
        var SelectOneController = (function () {
            function SelectOneController($log) {
                this.allowClear = true;
                this.placeholder = "Select a value";
            }
            Object.defineProperty(SelectOneController.prototype, "displayValue", {
                get: function () {
                    return this.model && this.model[this.display];
                },
                enumerable: true,
                configurable: true
            });
            SelectOneController.prototype.filterFunction = function (criteria) {
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
            SelectOneController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectOneController.prototype.refresh = function (criteria) {
                var _this = this;
                this
                    .lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                });
            };
            return SelectOneController;
        }());
        SelectOneController.bindings = {
            model: "=",
            display: "@",
            options: "<",
            refreshDelay: "<",
            lookup: "&lookup",
            order: "<",
            allowClear: "<",
            appendToBody: "<",
            placeholder: "@"
        };
        SelectOneController.$inject = ["$log", "$translate"];
        Bootstrap.SelectOneController = SelectOneController;
        angular
            .module("allors")
            .component("bSelectOne", {
            controller: SelectOneController,
            templateUrl: SelectOneTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: SelectOneController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=SelectOne.js.map