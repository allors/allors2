angular.module("allors").directive("focus", function ($timeout) {
    return {
        restrict: "A",
        link: function ($scope, $element) {
            $timeout(function () {
                $element[0].focus();
            }, 0);
        }
    };
});
//# sourceMappingURL=focus.js.map