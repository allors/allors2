/// <reference path="../allors.module.ts" />
angular.module("allors").directive("focus", $timeout => {
    return {
        restrict: "A",
        link($scope, $element) {
            $timeout(() => {
                $element[0].focus();
            }, 0);
        }
    };
});