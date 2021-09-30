/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />

namespace Allors.Bootstrap {

    export class SelectTemplate {
        static templateName = "allors/bootstrap/select";

        static createDefaultView() {
            return `
<ui-select ng-if="$ctrl.roleType.isOne && $ctrl.options !== undefined" ng-model="$ctrl.role" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select a value" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$ctrl.displayValue}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.roleType.isOne && $ctrl.options === undefined" ng-model="$ctrl.role" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select a value" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$ctrl.displayValue}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()" refresh="$ctrl.refresh($select.search)" refresh-delay="$ctrl.refreshDelay">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.roleType.isMany && $ctrl.options !== undefined" multiple ng-model="$ctrl.sortedRole" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select values" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.roleType.isMany && $ctrl.options === undefined" multiple ng-model="$ctrl.sortedRole" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select values" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()" refresh="$ctrl.refresh($select.search)" refresh-delay="$ctrl.refreshDelay">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = SelectTemplate.createDefaultView()) {
            templateCache.put(SelectTemplate.templateName, view);
        }
    }

    export class SelectController extends Field {
        static bindings = {
            object: "<",
            relation: "@",
            display: "@",
            options: "<",
            order: "<",
            refreshDelay: "<",
            lookup: "&lookup"
        } as { [binding: string]: string }

        options: SessionObject[];
        asyncOptions: SessionObject[];
        order: any;

        remove: () => void;

        static $inject = ["$log", "$translate"];
        constructor($log: angular.ILogService, $translate: angular.translate.ITranslateService) {
            super($log, $translate);
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

        private _sortedRole: Array<{}>;

        get sortedRole() {

            if (!this.role) {
                this._sortedRole = null;
            } else {
                const arraysAreEqual = (a, b) => {
                    if (!b || a.length !== b.length ) {
                        return false;
                    }

                    for (let i = 0; i < a.length; i++) {
                        if (a[i] !== b[i]) {
                            return false;
                        }
                    }

                    return true;
                }

                const newSortedRole = this.role.slice(0).sort((a, b) => a[this.orderBy()] < b[this.orderBy()] ? -1 : 1);

                if (!arraysAreEqual(newSortedRole, this._sortedRole)) {
                    this._sortedRole = newSortedRole;
                }
            }

            return this._sortedRole;
        }

        set sortedRole(value) {
            this.role = value;
        }
    }

    angular
        .module("allors")
        .component("bSelect", {
            controller: SelectController,
            templateUrl: SelectTemplate.templateName,
            require: FormController.require,
            bindings: SelectController.bindings
        });
}
