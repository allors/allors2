/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
namespace Allors.Bootstrap {

    export class EnumTemplate {
        static templateName = "allors/bootstrap/enum";

        static createDefaultView() {
            return `
<select class="form-control" 
            ng-model="$ctrl.role" 
            ng-disabled="!$ctrl.canWrite" 
            ng-required="$ctrl.roleType.isRequired"
            ng-options="option.value as option.name for option in $ctrl.options">
    <option ng-if="!$ctrl.roleType.isRequired" value=""></option>     
</select>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = EnumTemplate.createDefaultView()) {
            templateCache.put(EnumTemplate.templateName, view);
        }
    }

    export class Enum {
        constructor(public value: number, public name: string) {}
    }

    export class EnumController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            fullTypeName: "@enum",
            options: "<" 
        } as { [binding: string]: string }

        fullTypeName: string;
        options: any[];

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);

            if (!this.options) {
                const type = eval(this.fullTypeName);
                if (type) {
                    const lastIndex = this.fullTypeName.lastIndexOf(".");
                    const typeName = this.fullTypeName.substr(lastIndex + 1);

                    this.options = [];
                    for (let k in type) {
                        if (type.hasOwnProperty(k)) {
                            const value = type[k];
                            if (typeof value === "number") {

                                const name = type[value];
                                const humanizedName = Filters.Humanize.filter(name);

                                const enumeration = new Enum(value, humanizedName);
                                this.options.push(enumeration);

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
    }

    angular
        .module("allors")
        .component("bEnum", {
            controller: EnumController,
            templateUrl: EnumTemplate.templateName,
            require: FormController.require,
            bindings: EnumController.bindings
        });
}
