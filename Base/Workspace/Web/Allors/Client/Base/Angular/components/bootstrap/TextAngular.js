var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextAngularTemplate = (function () {
            function TextAngularTemplate() {
            }
            TextAngularTemplate.createDefaultView = function () {
                return "\n<div ng-if=\"$ctrl.canWrite\">\n<text-angular ng-model=\"$ctrl.role\" '/>\n</div>\n<div ng-if=\"!$ctrl.canWrite\">\n<text-angular ta-bind=\"text\" ng-model=\"$ctrl.role\" '/>\n</div>";
            };
            TextAngularTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextAngularTemplate.createDefaultView(); }
                templateCache.put(TextAngularTemplate.name, view);
            };
            return TextAngularTemplate;
        }());
        TextAngularTemplate.name = "allors/bootstrap/text-angular";
        Bootstrap.TextAngularTemplate = TextAngularTemplate;
        var TextAngularController = (function (_super) {
            __extends(TextAngularController, _super);
            function TextAngularController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return TextAngularController;
        }(Bootstrap.Field));
        TextAngularController.bindings = {
            object: "<",
            relation: "@"
        };
        TextAngularController.$inject = ["$log", "$translate"];
        Bootstrap.TextAngularController = TextAngularController;
        angular
            .module("allors")
            .component("bTextAngular", {
            controller: TextAngularController,
            templateUrl: TextAngularTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: TextAngularController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=TextAngular.js.map