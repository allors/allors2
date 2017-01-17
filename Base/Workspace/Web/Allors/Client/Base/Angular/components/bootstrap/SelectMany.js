var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectManyTemplate = (function () {
            function SelectManyTemplate() {
            }
            SelectManyTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.options !== undefined\" multiple ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\">\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.options === undefined\" multiple ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\">\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectManyTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectManyTemplate.createDefaultView(); }
                templateCache.put(SelectManyTemplate.name, view);
            };
            return SelectManyTemplate;
        }());
        SelectManyTemplate.name = "allors/bootstrap/select-many";
        Bootstrap.SelectManyTemplate = SelectManyTemplate;
        var SelectManyController = (function () {
            function SelectManyController($log) {
                this.allowClear = true;
                this.placeholder = "Select values";
            }
            SelectManyController.prototype.$onInit = function () {
                if (!this.model) {
                    this.model = new Array();
                }
            };
            Object.defineProperty(SelectManyController.prototype, "displayValue", {
                get: function () {
                    return this.model && this.model[this.display];
                },
                enumerable: true,
                configurable: true
            });
            SelectManyController.prototype.filterFunction = function (criteria) {
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
            SelectManyController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectManyController.prototype.refresh = function (criteria) {
                var _this = this;
                this
                    .lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                });
            };
            return SelectManyController;
        }());
        SelectManyController.bindings = {
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
        SelectManyController.$inject = ["$log", "$translate"];
        Bootstrap.SelectManyController = SelectManyController;
        angular
            .module("allors")
            .component("bSelectMany", {
            controller: SelectManyController,
            templateUrl: SelectManyTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: SelectManyController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=SelectMany.js.map