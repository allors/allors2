/// <reference path="../../allors.module.ts" />
namespace Allors.Bootstrap {

    export class SelectOneTemplate {
        static templateName = "allors/bootstrap/select-one";

        static createDefaultView() {
            return `
<ui-select ng-if="$ctrl.options !== undefined" ng-model="$ctrl.model" append-to-body="$ctrl.appendToBody" >
    <ui-select-match placeholder="{{$ctrl.placeholder}}" allow-clear="{{$ctrl.allowClear}}">{{$ctrl.displayValue}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.options === undefined" ng-model="$ctrl.model" append-to-body="$ctrl.appendToBody" >
    <ui-select-match placeholder="{{$ctrl.placeholder}}" allow-clear="{{$ctrl.allowClear}}">{{$ctrl.displayValue}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()" refresh="$ctrl.refresh($select.search)" refresh-delay="$ctrl.refreshDelay">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = SelectOneTemplate.createDefaultView()) {
            templateCache.put(SelectOneTemplate.templateName, view);
        }
    }

    export class SelectOneController {
        static bindings = {
            model: "=",
            display: "@",
            options: "<",
            refreshDelay: "<",
            lookup: "&lookup",
            order: "<",
            allowClear: "<",
            appendToBody: "<",
            placeholder: "@"
        } as { [binding: string]: string }

        allowClear = true;
        placeholder = "Select a value";

        model: SessionObject;
        display: string;
        options: SessionObject[];
        asyncOptions: SessionObject[];
        order: any;

        lookup: (any) => angular.IPromise<any>;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService) {
        }

        get displayValue(): any {
            return this.model && this.model[this.display];
        }

        filterFunction(criteria: string): (object) => boolean {

            return (object) => {

                // If no criteria then all is a match
                if (criteria === undefined || criteria.length === 0) {
                    return true;
                }

                const value = object[this.display];
                if (value) {

                    if (value.toLowerCase) {
                        const lowerCaseValue = value.toLowerCase();
                        const lowerCaseCriteria = criteria.toLowerCase();
                        return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
                    } else {
                        return value.toString().indexOf(criteria) >= 0;
                    }
                }

                return false;
            }
        }

        orderBy() {
            if (this.order) {
                return this.order;
            } else {
                return this.display;
            }
        }

        refresh(criteria) {
            this.lookup({ criteria: criteria })
                .then((results) => {
                    this.asyncOptions = results;
                })
                .catch(()=>{});;
        }
    }

    angular
        .module("allors")
        .component("bSelectOne", {
            controller: SelectOneController,
            templateUrl: SelectOneTemplate.templateName,
            require: FormController.require,
            bindings: SelectOneController.bindings
        });
}