var Allors;
(function (Allors) {
    var Database = (function () {
        function Database($http, $q, prefix, postfix) {
            this.$http = $http;
            this.$q = $q;
            this.prefix = prefix;
            this.postfix = postfix;
        }
        Database.prototype.pull = function (name, params) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var serviceName = name + _this.postfix;
                _this.$http.post(serviceName, params)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Data.ResponseType.Pull;
                    resolve(response);
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Database.prototype.sync = function (syncRequest) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _this.$http.post(_this.prefix + "Sync", syncRequest)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Data.ResponseType.Sync;
                    resolve(response);
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Database.prototype.push = function (pushRequest) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _this.$http.post(_this.prefix + "Push", pushRequest)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Data.ResponseType.Sync;
                    if (response.hasErrors) {
                        reject(response);
                    }
                    else {
                        resolve(response);
                    }
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Database.prototype.invoke = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                if (methodOrService instanceof Allors.Method) {
                    var method = methodOrService;
                    var invokeRequest = {
                        i: method.object.id,
                        v: method.object.version,
                        m: method.name
                    };
                    _this.$http.post(_this.prefix + "Invoke", invokeRequest)
                        .then(function (callbackArg) {
                        var response = callbackArg.data;
                        response.responseType = Allors.Data.ResponseType.Invoke;
                        if (response.hasErrors) {
                            reject(response);
                        }
                        else {
                            resolve(response);
                        }
                    })
                        .catch(function (e) {
                        reject(e);
                    });
                }
                else {
                    var service = methodOrService + _this.postfix;
                    _this.$http.post(service, args)
                        .then(function (callbackArg) {
                        var response = callbackArg.data;
                        response.responseType = Allors.Data.ResponseType.Invoke;
                        if (response.hasErrors) {
                            reject(response);
                        }
                        else {
                            resolve(response);
                        }
                    })
                        .catch(function (e) {
                        reject(e);
                    });
                }
            });
        };
        return Database;
    }());
    Allors.Database = Database;
})(Allors || (Allors = {}));
//# sourceMappingURL=Database.js.map