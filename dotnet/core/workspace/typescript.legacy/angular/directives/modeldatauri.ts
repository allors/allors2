/// <reference path="../allors.module.ts" />
module Allors.Directives {

    modelDataUri.$inject = ["$parse"];
    function modelDataUri($parse: angular.IParseService): angular.IDirective {

        function link(scope, element, attrs): void {
            var model = $parse(attrs.modelDataUri);

            function onChange(event: any) {
                scope.$apply(() => {
                    const file = event.currentTarget.files[0];
                    const reader = new FileReader();
                    reader.onload = (readEvent: any) => {
                        scope.$apply(() => {
                            var image = readEvent.target.result;
                            model.assign(scope, image);
                        });
                    };

                    reader.readAsDataURL(file);
                });
            }

            // Register
            element.on("change", onChange);

            // Unregister
            scope.$on("$destroy", () => {
                element.off("change", onChange);
            });
        }

        return {
            restrict: "A",
            link: link
        } as angular.IDirective;
    }

    angular
        .module("allors")
        .directive("modelDataUri", modelDataUri);
}