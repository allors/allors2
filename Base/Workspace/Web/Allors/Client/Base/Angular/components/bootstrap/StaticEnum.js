var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var StaticEnumTemplate = (function () {
            function StaticEnumTemplate() {
            }
            StaticEnumTemplate.createDefaultView = function () {
                return "\n<p class=\"form-control-static\" ng-bind=\"$ctrl.enum.name\"></p>\n";
            };
            StaticEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = StaticEnumTemplate.createDefaultView(); }
                templateCache.put(StaticEnumTemplate.name, view);
            };
            return StaticEnumTemplate;
        }());
        StaticEnumTemplate.name = "allors/bootstrap/static-enum";
        Bootstrap.StaticEnumTemplate = StaticEnumTemplate;
        var StaticEnum = (function () {
            function StaticEnum(value, name) {
                this.value = value;
                this.name = name;
            }
            return StaticEnum;
        }());
        Bootstrap.StaticEnum = StaticEnum;
        var StaticEnumController = (function (_super) {
            __extends(StaticEnumController, _super);
            function StaticEnumController($log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                var type = eval(_this.fullTypeName);
                if (type) {
                    var lastIndex = _this.fullTypeName.lastIndexOf(".");
                    var typeName = _this.fullTypeName.substr(lastIndex + 1);
                    _this.enums = [];
                    for (var k in type) {
                        if (type.hasOwnProperty(k)) {
                            var value = type[k];
                            if (typeof value === "number") {
                                var name_1 = type[value];
                                var humanizedName = Allors.Filters.Humanize.filter(name_1);
                                var enumeration = new StaticEnum(value, humanizedName);
                                _this.enums.push(enumeration);
                                (function (enumeration, key1, key2) {
                                    _this.translate(key1, key2, function (translatedName) {
                                        if (translatedName) {
                                            enumeration.name = translatedName;
                                        }
                                    });
                                })(enumeration, "enum_" + typeName + "_" + value, "enum_" + typeName + "_" + name_1);
                            }
                        }
                    }
                }
                return _this;
            }
            Object.defineProperty(StaticEnumController.prototype, "enum", {
                get: function () {
                    var _this = this;
                    var filtered = this.enums.filter(function (v) { return v.value === _this.role; });
                    return !!filtered ? filtered[0] : undefined;
                },
                enumerable: true,
                configurable: true
            });
            return StaticEnumController;
        }(Bootstrap.Field));
        StaticEnumController.bindings = {
            object: "<",
            relation: "@",
            fullTypeName: "@enum"
        };
        StaticEnumController.$inject = ["$log", "$translate"];
        Bootstrap.StaticEnumController = StaticEnumController;
        angular
            .module("allors")
            .component("bStaticEnum", {
            controller: StaticEnumController,
            templateUrl: StaticEnumTemplate.name,
            require: Bootstrap.FormController.require,
            bindings: StaticEnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=StaticEnum.js.map