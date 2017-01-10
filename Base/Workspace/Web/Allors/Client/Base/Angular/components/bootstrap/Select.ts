namespace Allors.Bootstrap {

    export class SelectTemplate {
        static name = "allors/bootstrap/select";

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

<ui-select ng-if="$ctrl.roleType.isMany && $ctrl.options !== undefined" multiple ng-model="$ctrl.role" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select values" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>

<ui-select ng-if="$ctrl.roleType.isMany && $ctrl.options === undefined" multiple ng-model="$ctrl.role" ng-disabled="!$ctrl.canWrite" ng-required="$ctrl.roleType.isRequired">
    <ui-select-match placeholder="Select values" allow-clear="{{!$ctrl.roleType.isRequired}}">{{$item[$ctrl.display]}}</ui-select-match>
    <ui-select-choices repeat="item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()" refresh="$ctrl.refresh($select.search)" refresh-delay="$ctrl.refreshDelay">
        <div ng-bind-html="item[$ctrl.display] | highlight: $select.search"></div>
    </ui-select-choices>
</ui-select>
`;
        }

        static register(templateCache: angular.ITemplateCacheService, view = SelectTemplate.createDefaultView()) {
            templateCache.put(SelectTemplate.name, view);
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
                const value = object[this.display] as string;
                if (value) {
                    const lowerCaseValue = value.toLowerCase();
                    const lowerCaseCriteria = criteria.toLowerCase();

                    return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
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
            this
                .lookup({ criteria: criteria })
                .then((results) => {
                    this.asyncOptions = results;
                });
        }
    }

    angular
        .module("allors")
        .component("bSelect", {
            controller: SelectController,
            templateUrl: SelectTemplate.name,
            require: FormController.require,
            bindings: SelectController.bindings
        });
}
