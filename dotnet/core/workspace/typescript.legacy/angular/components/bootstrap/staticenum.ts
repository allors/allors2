/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {

    export class StaticEnumTemplate {
        static templateName = "allors/bootstrap/static-enum";

        static createDefaultView() {
            return `
<p class="form-control-static" ng-bind="$ctrl.enum.name"></p>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = StaticEnumTemplate.createDefaultView()) {
            templateCache.put(StaticEnumTemplate.templateName, view);
        }
    }

    export class StaticEnum {
        constructor(public value: number, public name: string) {}
    }

    export class StaticEnumController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            fullTypeName: "@enum"
        } as { [binding: string]: string }

        get enum(): StaticEnum {
            const filtered = this.enums.filter(v => v.value === this.role);
            return !!filtered ? filtered[0] : undefined;
        }

        fullTypeName: string;

        enums: StaticEnum[];
        
        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
            
            const type = eval(this.fullTypeName);
            if (type) {
                const lastIndex = this.fullTypeName.lastIndexOf(".");
                const typeName = this.fullTypeName.substr(lastIndex + 1);

                this.enums = [];
                for (let k in type) {
                    if (type.hasOwnProperty(k)) {
                        const value = type[k];
                        if (typeof value === "number") {

                            const name = type[value];
                            const humanizedName = Filters.Humanize.filter(name);

                            const enumeration = new StaticEnum(value, humanizedName);
                            this.enums.push(enumeration);

                            ((enumeration, key1, key2) => {
                                this.translate(key1, key2, (translatedName) => {
                                    if (translatedName) {
                                        enumeration.name = translatedName;
                                    }
                                });
                            })(enumeration, `enum_${typeName}_${value}`, `enum_${typeName}_${name}`);

                        }
                    }
                }
            } 
        }
    }

    angular
        .module("allors")
        .component("bStaticEnum", {
            controller: StaticEnumController,
            templateUrl: StaticEnumTemplate.templateName,
            require: FormController.require,
            bindings: StaticEnumController.bindings
        });
}
