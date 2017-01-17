var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var StaticTemplate = (function () {
            function StaticTemplate() {
            }
            StaticTemplate.createDefaultView = function () {
                return "\n<p class=\"form-control-static\" ng-bind=\"$ctrl.role\"></p>\n";
            };
            StaticTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = StaticTemplate.createDefaultView(); }
                templateCache.put(StaticTemplate.name, view);
            };
            return StaticTemplate;
        }());
        StaticTemplate.name = "allors/bootstrap/static";
        Bootstrap.StaticTemplate = StaticTemplate;
        var StaticController = (function (_super) {
            __extends(StaticController, _super);
            function StaticController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return StaticController;
        }(Bootstrap.Field));
        StaticController.bindings = {
            object: "<",
            relation: "@"
        };
        StaticController.$inject = ["$log", "$translate"];
        Bootstrap.StaticController = StaticController;
        angular
            .module("allors")
            .component("bStatic", {
            controller: StaticController,
            templateUrl: StaticTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: StaticController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Static.js.map