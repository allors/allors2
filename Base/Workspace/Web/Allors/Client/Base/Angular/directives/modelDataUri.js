var Allors;
(function (Allors) {
    var Directives;
    (function (Directives) {
        modelDataUri.$inject = ["$parse"];
        function modelDataUri($parse) {
            function link(scope, element, attrs) {
                var model = $parse(attrs.modelDataUri);
                function onChange(event) {
                    scope.$apply(function () {
                        var file = event.currentTarget.files[0];
                        var reader = new FileReader();
                        reader.onload = function (readEvent) {
                            scope.$apply(function () {
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
                scope.$on("$destroy", function () {
                    element.off("change", onChange);
                });
            }
            return {
                restrict: "A",
                link: link
            };
        }
        angular
            .module("allors")
            .directive("modelDataUri", modelDataUri);
    })(Directives = Allors.Directives || (Allors.Directives = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=modelDataUri.js.map