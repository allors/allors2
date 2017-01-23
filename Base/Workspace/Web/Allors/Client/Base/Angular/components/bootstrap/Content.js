var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var ContentTemplate = (function () {
            function ContentTemplate() {
            }
            ContentTemplate.createDefaultView = function () {
                return "\n<div contenteditable\n        ng-model=\"$ctrl.role\"\n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\">\n</div>\n";
            };
            ;
            ContentTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = ContentTemplate.createDefaultView(); }
                templateCache.put(ContentTemplate.name, view);
            };
            return ContentTemplate;
        }());
        ContentTemplate.name = "allors/bootstrap/content";
        Bootstrap.ContentTemplate = ContentTemplate;
        var ContentController = (function (_super) {
            __extends(ContentController, _super);
            function ContentController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            return ContentController;
        }(Bootstrap.Field));
        ContentController.bindings = {
            object: "<",
            relation: "@"
        };
        ContentController.$inject = ["$log", "$translate"];
        Bootstrap.ContentController = ContentController;
        angular
            .module("allors")
            .component("bContent", {
            controller: ContentController,
            templateUrl: ContentTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: ContentController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Content.js.map