var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Custom;
        (function (Custom) {
            function nest(collection, iteratees) {
                if (!iteratees.length) {
                    return collection;
                }
                var first = iteratees[0];
                var rest = iteratees.slice(1);
                var sorted = _.sortBy(collection, first);
                var group = _.groupBy(sorted, first);
                return _.mapValues(group, function (value) { return nest(value, rest); });
            }
            Custom.nest = nest;
            ;
        })(Custom = Domain.Custom || (Domain.Custom = {}));
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Nest.js.map