var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TypeaheadTemplate = (function () {
            function TypeaheadTemplate() {
            }
            TypeaheadTemplate.createDefaultView = function () {
                return "\n<input  type=\"text\"\n        ng-disabled=\"!$ctrl.canWrite\" \n        ng-required=\"$ctrl.roleType.isRequired\" \n        ng-model=\"$ctrl.role\"\n        uib-typeahead=\"item for item in $ctrl.options | filter:$viewValue | limitTo:10\"\n        class=\"form-control\" />\n";
            };
            TypeaheadTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TypeaheadTemplate.createDefaultView(); }
                templateCache.put(TypeaheadTemplate.name, view);
            };
            return TypeaheadTemplate;
        }());
        TypeaheadTemplate.name = "allors/bootstrap/typeahead";
        Bootstrap.TypeaheadTemplate = TypeaheadTemplate;
        var TypeaheadController = (function (_super) {
            __extends(TypeaheadController, _super);
            function TypeaheadController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return TypeaheadController;
        }(Bootstrap.Field));
        TypeaheadController.bindings = {
            object: "<",
            relation: "@",
            options: "<"
        };
        TypeaheadController.$inject = ["$log", "$translate"];
        Bootstrap.TypeaheadController = TypeaheadController;
        angular
            .module("allors")
            .component("bTypeahead", {
            controller: TypeaheadController,
            templateUrl: TypeaheadTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: TypeaheadController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Typeahead.js.map