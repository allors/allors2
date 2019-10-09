/// <reference path="../../allors.module.ts" />
namespace Allors.Bootstrap {

    export class SelectManyTemplate {
        static templateName = "allors/bootstrap/select-many";

        static createDefaultView() {
            return `
<ui-select ng-if="$ctrl.options !== undefined" multiple ng-model="$ctrl.model" append-to-body="$ctrl.appendToBody">
    <ui-select-match placeholder="{{$ctrl.placeholder}}" allow-clear="{{$ctrl.allowClear}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.options === undefined" multiple ng-model="$ctrl.model" append-to-body="$ctrl.appendToBody">
    <ui-select-match placeholder="{{$ctrl.placeholder}}" allow-clear="{{$ctrl.allowClear}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()" refresh="$ctrl.refresh($select.search)" refresh-delay="$ctrl.refreshDelay">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = SelectManyTemplate.createDefaultView()) {
            templateCache.put(SelectManyTemplate.templateName, view);
        }
    }

    export class SelectManyController {
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
        placeholder = "Select values";

        model: SessionObject[];
        display: string;
        options: SessionObject[];
        asyncOptions: SessionObject[];
        order: any;

        lookup: (any) => angular.IPromise<any>;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService) {
        }

        $onInit() {
            if (!this.model) {
                this.model = new Array<SessionObject>();
            }
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
        .component("bSelectMany", {
            controller: SelectManyController,
            templateUrl: SelectManyTemplate.templateName,
            require: FormController.require,
            bindings: SelectManyController.bindings
        });
}