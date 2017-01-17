var Allors;
(function (Allors) {
    var Result = (function () {
        function Result(session, response) {
            var _this = this;
            this.objects = {};
            this.collections = {};
            this.values = {};
            _.map(response.namedObjects, function (v, k) {
                _this.objects[k] = session.get(v);
            });
            _.map(response.namedCollections, function (v, k) {
                _this.collections[k] = _.map(v, function (obj) { return session.get(obj); });
            });
            _.map(response.namedValues, function (v, k) {
                _this.values[k] = v;
            });
        }
        return Result;
    }());
    Allors.Result = Result;
})(Allors || (Allors = {}));
//# sourceMappingURL=Result.js.map