var Allors;
(function (Allors) {
    var Filters;
    (function (Filters) {
        var Humanize;
        (function (Humanize) {
            angular.module("allors").
                filter("humanize", function () { return Humanize.filter; });
            Humanize.filter = function (input) {
                return input
                    .replace(/([a-z\d])([A-Z])/g, "$1" + " " + "$2")
                    .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1" + " " + "$2");
            };
        })(Humanize = Filters.Humanize || (Filters.Humanize = {}));
    })(Filters = Allors.Filters || (Allors.Filters = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=humanize.js.map