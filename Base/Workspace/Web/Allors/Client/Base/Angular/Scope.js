var Allors;
(function (Allors) {
    var Scope = (function () {
        function Scope(name, database, workspace, $rootScope, $scope, $q, $log) {
            var _this = this;
            this.$scope = $scope;
            this.$q = $q;
            this.$log = $log;
            this.context = new Allors.Context(name, database, workspace);
            this.events = new Allors.Events(this.context, $rootScope, $scope);
            this.session = this.context.session;
            this.events.onRefresh(function () { return _this.refresh(); });
        }
        Object.defineProperty(Scope.prototype, "objects", {
            // Context
            get: function () {
                return this.context.objects;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Scope.prototype, "collections", {
            get: function () {
                return this.context.collections;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Scope.prototype, "values", {
            get: function () {
                return this.context.values;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(Scope.prototype, "hasChanges", {
            get: function () {
                return this.context.session.hasChanges;
            },
            enumerable: true,
            configurable: true
        });
        // Commands
        Scope.prototype.load = function (params) {
            return this.context.load(params);
        };
        Scope.prototype.save = function () {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _this.context
                    .save()
                    .then(function (saveResponse) {
                    _this.events.broadcastRefresh();
                    resolve(saveResponse);
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Scope.prototype.invoke = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                if (methodOrService instanceof Allors.Method) {
                    return _this.context
                        .invoke(methodOrService)
                        .then(function (invokeResponse) {
                        resolve(invokeResponse);
                    })
                        .catch(function (e) {
                        reject(e);
                    })
                        .finally(function () { return _this.events.broadcastRefresh(); });
                }
                else {
                    return _this.context
                        .invoke(methodOrService, args)
                        .then(function (invokeResponse) {
                        resolve(invokeResponse);
                    })
                        .catch(function (e) {
                        reject(e);
                    })
                        .finally(function () { return _this.events.broadcastRefresh(); });
                }
            });
        };
        Scope.prototype.saveAndInvoke = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                return _this.context
                    .save()
                    .then(function () {
                    _this.refresh()
                        .then(function () {
                        if (methodOrService instanceof Allors.Method) {
                            _this.context.invoke(methodOrService)
                                .then(function (invokeResponse) {
                                resolve(invokeResponse);
                            })
                                .catch(function (e) { return reject(e); })
                                .finally(function () { return _this.events.broadcastRefresh(); });
                        }
                        else {
                            _this.context.invoke(methodOrService, args)
                                .then(function (invokeResponse) {
                                resolve(invokeResponse);
                            })
                                .catch(function (e) { return reject(e); })
                                .finally(function () { return _this.events.broadcastRefresh(); });
                        }
                    })
                        .catch(function (e) { return reject(e); });
                })
                    .catch(function (e) { return reject(e); });
            });
        };
        Scope.prototype.query = function (query, args) {
            return this.context.query(query, args);
        };
        Scope.prototype.queryResults = function (query, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _this.context
                    .query(query, args)
                    .then(function (result) {
                    var results = result.collections["results"];
                    resolve(results);
                })
                    .catch(function (e) { return reject(e); });
            });
        };
        return Scope;
    }());
    Allors.Scope = Scope;
})(Allors || (Allors = {}));
//# sourceMappingURL=Scope.js.map