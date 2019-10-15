var Allors;
(function (Allors) {
    angular.module("allors", []);
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Method = /** @class */ (function () {
        function Method(object, methodType) {
            this.object = object;
            this.methodType = methodType;
        }
        return Method;
    }());
    Allors.Method = Method;
})(Allors || (Allors = {}));
var __values = (this && this.__values) || function(o) {
    var s = typeof Symbol === "function" && Symbol.iterator, m = s && o[s], i = 0;
    if (m) return m.call(o);
    if (o && typeof o.length === "number") return {
        next: function () {
            if (o && i >= o.length) o = void 0;
            return { value: o && o[i++], done: !o };
        }
    };
    throw new TypeError(s ? "Object is not iterable." : "Symbol.iterator is not defined.");
};
/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
var Allors;
(function (Allors) {
    var Context = /** @class */ (function () {
        function Context(name, database, workspace) {
            this.name = name;
            this.database = database;
            this.workspace = workspace;
            this.objects = {};
            this.collections = {};
            this.values = {};
            this.$q = this.database.$q;
            this.session = new Allors.Session(this.workspace);
        }
        Context.prototype.load = function (params) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                return _this.database
                    .pull(_this.name, params)
                    .then(function (response) {
                    try {
                        var syncRequest = _this.workspace.diff(response);
                        if (syncRequest.objects.length > 0) {
                            _this.database.sync(syncRequest)
                                .then(function (syncResponse) {
                                var securityRequest = _this.workspace.sync(syncResponse);
                                _this.update(response);
                                _this.session.reset();
                                if (securityRequest) {
                                    _this.database
                                        .security(securityRequest)
                                        .then(function (v) {
                                        var securityRequest2 = _this.workspace.security(v);
                                        if (securityRequest2) {
                                            _this.database
                                                .security(securityRequest2)
                                                .then(function (v) {
                                                _this.workspace.security(v);
                                                resolve();
                                            })
                                                .catch(function (e) {
                                                reject(e);
                                            });
                                        }
                                        else {
                                            resolve();
                                        }
                                    })
                                        .catch(function (e) {
                                        reject(e);
                                    });
                                }
                                else {
                                    resolve();
                                }
                            })
                                .catch(function (e2) {
                                reject(e2);
                            });
                        }
                        else {
                            _this.update(response);
                            _this.session.reset();
                            resolve();
                        }
                    }
                    catch (e) {
                        reject(e);
                    }
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Context.prototype.query = function (service, params) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _this.database.pull(service, params)
                    .then(function (v) {
                    try {
                        var response_1 = v;
                        var requireLoadIds = _this.workspace.diff(response_1);
                        if (requireLoadIds.objects.length > 0) {
                            _this.database.sync(requireLoadIds)
                                .then(function (u) {
                                var loadResponse = u;
                                _this.workspace.sync(loadResponse);
                                var result = new Allors.Loaded(_this.session, response_1);
                                resolve(result);
                            })
                                .catch(function (e2) { return reject(e2); });
                        }
                        else {
                            var result = new Allors.Loaded(_this.session, response_1);
                            resolve(result);
                        }
                    }
                    catch (e) {
                        reject(e);
                    }
                })
                    .catch(function (e) { return reject(e); });
            });
        };
        Context.prototype.save = function () {
            var _this = this;
            return this.$q(function (resolve, reject) {
                try {
                    var pushRequest_1 = _this.session.pushRequest();
                    _this.database
                        .push(pushRequest_1)
                        .then(function (pushResponse) {
                        var e_1, _a;
                        try {
                            _this.session.pushResponse(pushResponse);
                            var syncRequest = new Allors.Protocol.SyncRequest();
                            syncRequest.objects = pushRequest_1.objects.map(function (v) { return v.i; });
                            if (pushResponse.newObjects) {
                                try {
                                    for (var _b = __values(pushResponse.newObjects), _c = _b.next(); !_c.done; _c = _b.next()) {
                                        var newObject = _c.value;
                                        syncRequest.objects.push(newObject.i);
                                    }
                                }
                                catch (e_1_1) { e_1 = { error: e_1_1 }; }
                                finally {
                                    try {
                                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                                    }
                                    finally { if (e_1) throw e_1.error; }
                                }
                            }
                            _this.database.sync(syncRequest)
                                .then(function (syncResponse) {
                                _this.workspace.sync(syncResponse);
                                _this.session.reset();
                                resolve(pushResponse);
                            })
                                .catch(function (reason) {
                                reject(reason);
                            });
                        }
                        catch (e3) {
                            reject(e3);
                        }
                    })
                        .catch(function (e2) {
                        reject(e2);
                    });
                }
                catch (e) {
                    reject(e);
                }
            });
        };
        Context.prototype.invoke = function (methodOrService, args) {
            if (methodOrService instanceof Allors.Method) {
                return this.database.invoke(methodOrService);
            }
            else {
                return this.database.invoke(methodOrService, args);
            }
        };
        Context.prototype.update = function (response) {
            var _this = this;
            this.objects = {};
            this.collections = {};
            this.values = {};
            _.map(response.namedObjects, function (v, k) {
                _this.objects[k] = _this.session.get(v);
            });
            _.map(response.namedCollections, function (v, k) {
                _this.collections[k] = _.map(v, function (obj) { return _this.session.get(obj); });
            });
            _.map(response.namedValues, function (v, k) {
                _this.values[k] = v;
            });
        };
        return Context;
    }());
    Allors.Context = Context;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var Compressor = /** @class */ (function () {
            function Compressor() {
                this.keyByValue = {};
                this.counter = 0;
            }
            Compressor.prototype.write = function (value) {
                if (value === undefined || value === null) {
                    return null;
                }
                if (this.keyByValue.hasOwnProperty(value)) {
                    return this.keyByValue[value];
                }
                var key = (++this.counter).toString();
                this.keyByValue[value] = key;
                return "" + Compressor.indexMarker + key + Compressor.indexMarker + value;
            };
            Compressor.indexMarker = '~';
            Compressor.itemSeparator = ',';
            return Compressor;
        }());
        Protocol.Compressor = Compressor;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
/// <reference path="../Workspace/Protocol/Compressor.ts" />
var Allors;
(function (Allors) {
    var Compressor = Allors.Protocol.Compressor;
    var Database = /** @class */ (function () {
        function Database($http, $q, postfix, baseUrl) {
            this.$http = $http;
            this.$q = $q;
            this.postfix = postfix;
            this.baseUrl = baseUrl;
        }
        Object.defineProperty(Database.prototype, "headers", {
            get: function () {
                return this.authorization ? {
                    headers: { 'Authorization': this.authorization }
                } : undefined;
            },
            enumerable: true,
            configurable: true
        });
        Database.prototype.pull = function (name, params) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var serviceName = _this.baseUrl + "/" + name + _this.postfix;
                _this.$http.post(serviceName, params || {}, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Pull;
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
                var serviceName = _this.baseUrl + "allors/sync";
                _this.$http.post(serviceName, syncRequest, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Sync;
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
                var serviceName = _this.baseUrl + "allors/push";
                _this.$http.post(serviceName, pushRequest, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Sync;
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
        Database.prototype.security = function (securityRequest) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var serviceName = _this.baseUrl + "allors/security";
                _this.$http.post(serviceName, securityRequest, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Security;
                    resolve(response);
                })
                    .catch(function (e) {
                    reject(e);
                });
            });
        };
        Database.prototype.invoke = function (methodOrService, args) {
            if (methodOrService instanceof Allors.Method) {
                return this.invokeMethods([methodOrService]);
            }
            else if (methodOrService instanceof Array) {
                return this.invokeMethods(methodOrService, args);
            }
            else {
                return this.invokeService(methodOrService, args);
            }
        };
        Database.prototype.invokeMethods = function (methods, options) {
            var _this = this;
            var compressor = new Compressor();
            return this.$q(function (resolve, reject) {
                var invokeRequest = {
                    i: methods.map(function (v) {
                        return {
                            i: v.object.id,
                            v: v.object.version,
                            m: compressor.write(v.methodType.id),
                        };
                    }),
                    o: options
                };
                var serviceName = _this.baseUrl + "allors/invoke";
                _this.$http.post(serviceName, invokeRequest, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Invoke;
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
        Database.prototype.invokeService = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var serviceName = _this.baseUrl + methodOrService + _this.postfix;
                _this.$http.post(serviceName, args, _this.headers)
                    .then(function (callbackArg) {
                    var response = callbackArg.data;
                    response.responseType = Allors.Protocol.ResponseType.Invoke;
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
        return Database;
    }());
    Allors.Database = Database;
})(Allors || (Allors = {}));
/// <reference path="allors.module.ts" />
var Allors;
(function (Allors) {
    var Events = /** @class */ (function () {
        function Events(context, $rootScope, scope) {
            this.context = context;
            this.$rootScope = $rootScope;
            this.scope = scope;
        }
        Events.prototype.on = function (eventName, handler) {
            this.scope.$on(eventName, handler);
        };
        Events.prototype.onRefresh = function (handler) {
            this.on(Events.refreshEventName, handler);
        };
        Events.prototype.broadcast = function (eventName) {
            this.$rootScope.$broadcast(eventName, this.context.name);
        };
        Events.prototype.broadcastRefresh = function () {
            this.broadcast(Events.refreshEventName);
        };
        Events.refreshEventName = "allors.refresh";
        return Events;
    }());
    Allors.Events = Events;
})(Allors || (Allors = {}));
/// <reference path="allors.module.ts" />
var Allors;
(function (Allors) {
    var Loaded = /** @class */ (function () {
        function Loaded(session, response) {
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
        return Loaded;
    }());
    Allors.Loaded = Loaded;
})(Allors || (Allors = {}));
/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
var Allors;
(function (Allors) {
    var Scope = /** @class */ (function () {
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
/// <reference path="../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var FormTemplate = /** @class */ (function () {
            function FormTemplate() {
            }
            FormTemplate.register = function (templateCache) {
                templateCache.put(FormTemplate.templateName, FormTemplate.view);
            };
            FormTemplate.templateName = "allors/bootstrap/form";
            FormTemplate.view = "<form name=\"form\" ng-class=\"$ctrl.horizontal ? 'form-horizontal' : ''\">\n<ng-transclude />\n</form>";
            return FormTemplate;
        }());
        Bootstrap.FormTemplate = FormTemplate;
        var FormController = /** @class */ (function () {
            function FormController($scope, $log) {
                var _this = this;
                this.$scope = $scope;
                this.$log = $log;
                $scope.$watch(this.$scope["form"], function () {
                    if (_this.$scope["form"]) {
                        var form = _this.$scope["form"];
                        _this.onRegister({ form: form });
                    }
                });
            }
            FormController.require = {
                form: "^bForm"
            };
            FormController.bindings = {
                horizontal: "<",
                onRegister: "&"
            };
            FormController.$inject = ["$scope", "$log"];
            return FormController;
        }());
        Bootstrap.FormController = FormController;
        angular
            .module("allors")
            .component("bForm", {
            controller: FormController,
            templateUrl: FormTemplate.templateName,
            transclude: true,
            bindings: FormController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var Field = /** @class */ (function () {
            function Field($log, $translate) {
                this.$log = $log;
                this.$translate = $translate;
            }
            Object.defineProperty(Field.prototype, "objectType", {
                get: function () {
                    try {
                        return this.object && this.object.objectType;
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Field.prototype, "roleType", {
                get: function () {
                    try {
                        var objectType = this.object.objectType;
                        return objectType.roleTypeByName[this.relation];
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Field.prototype, "canRead", {
                get: function () {
                    try {
                        var canRead = false;
                        if (this.object) {
                            canRead = this.object.canRead(this.relation);
                        }
                        return canRead;
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Field.prototype, "canWrite", {
                get: function () {
                    try {
                        var canWrite = false;
                        if (this.object) {
                            canWrite = this.object.canWrite(this.relation);
                        }
                        return canWrite;
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Field.prototype, "role", {
                get: function () {
                    try {
                        return this.object && this.object[this.relation];
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                set: function (value) {
                    try {
                        this.object[this.relation] = value;
                    }
                    catch (e) {
                    }
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Field.prototype, "displayValue", {
                get: function () {
                    try {
                        return this.role && this.role[this.display];
                    }
                    catch (e) {
                        return undefined;
                    }
                },
                enumerable: true,
                configurable: true
            });
            Field.prototype.$onInit = function () {
                this.derive();
            };
            Field.prototype.$onChanges = function () {
                this.derive();
            };
            ;
            Field.prototype.derive = function () {
                var _this = this;
                try {
                    if (this.roleType && this.$translate) {
                        if (this.label === undefined) {
                            this.label = null;
                            var key1 = "meta_" + this.objectType.name + "_" + this.roleType.name + "_Label";
                            var key2 = "meta_" + this.roleType.objectType + "_" + this.roleType.name + "_Label";
                            this.translate(key1, key2, function (value) { return _this.label = value; });
                            if (this.label === undefined || this.label === null) {
                                this.label = Allors.Filters.Humanize.filter(this.relation);
                                var suffix = "Enum";
                                if (this.label.indexOf(suffix, this.label.length - suffix.length) !== -1) {
                                    this.label = this.label.substring(0, this.label.length - suffix.length);
                                }
                            }
                        }
                        if (this.placeholder === undefined) {
                            this.placeholder = null;
                            var key1 = "meta_" + this.objectType.name + "_" + this.roleType.name + "_Placeholder";
                            var key2 = "meta_" + this.roleType.objectType + "_" + this.roleType.name + "_Placeholder";
                            this.translate(key1, key2, function (value) { return _this.placeholder = value; });
                        }
                        if (this.help === undefined) {
                            this.help = null;
                            var key1 = "meta_" + this.objectType.name + "_" + this.roleType.name + "_Help";
                            var key2 = "meta_" + this.roleType.objectType + "_" + this.roleType.name + "_Help";
                            this.translate(key1, key2, function (value) { return _this.help = value; });
                        }
                    }
                }
                catch (e) {
                    this.$log.error("Could not translate field");
                }
            };
            Field.prototype.translate = function (key1, key2, set, setDefault) {
                var _this = this;
                this.$translate(key1)
                    .then(function (translation) {
                    if (key1 !== translation) {
                        set(translation);
                    }
                    else {
                        _this.$translate(key2)
                            .then(function (translation) {
                            if (key2 !== translation) {
                                set(translation);
                            }
                            else {
                                if (setDefault) {
                                    setDefault();
                                }
                            }
                        })
                            .catch(function () { });
                        ;
                    }
                })
                    .catch(function () { });
                ;
            };
            return Field;
        }());
        Bootstrap.Field = Field;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var ContentTemplate = /** @class */ (function () {
            function ContentTemplate() {
            }
            ContentTemplate.createDefaultView = function () {
                return "\n<div ng-attr-contenteditable=\"{{$ctrl.canWrite}}\"\n        ng-model=\"$ctrl.role\"\n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\">\n</div>\n";
            };
            ;
            ContentTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = ContentTemplate.createDefaultView(); }
                templateCache.put(ContentTemplate.templateName, view);
            };
            ContentTemplate.templateName = "allors/bootstrap/content";
            return ContentTemplate;
        }());
        Bootstrap.ContentTemplate = ContentTemplate;
        var ContentController = /** @class */ (function (_super) {
            __extends(ContentController, _super);
            function ContentController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            ContentController.bindings = {
                object: "<",
                relation: "@"
            };
            ContentController.$inject = ["$log", "$translate"];
            return ContentController;
        }(Bootstrap.Field));
        Bootstrap.ContentController = ContentController;
        angular
            .module("allors")
            .component("bContent", {
            controller: ContentController,
            templateUrl: ContentTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: ContentController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var CroppedImageTemplate = /** @class */ (function () {
            function CroppedImageTemplate() {
            }
            CroppedImageTemplate.createDefaultView = function () {
                return "\n<div ng-if=\"!$ctrl.role\">\n    <button type=\"button\" class=\"btn btn-default\" ng-click=\"$ctrl.add()\">Add new image</button>\n</div>\n        \n<div ng-if=\"$ctrl.role.InDataUri\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"{{$ctrl.role.InDataUri}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n<div ng-if=\"!$ctrl.role.InDataUri && $ctrl.role\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"/media/display/{{$ctrl.role.UniqueId}}?revision={{$ctrl.role.Revision}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n";
            };
            CroppedImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = CroppedImageTemplate.createDefaultView(); }
                templateCache.put(CroppedImageTemplate.templateName, view);
            };
            CroppedImageTemplate.templateName = "allors/bootstrap/cropped-image";
            return CroppedImageTemplate;
        }());
        Bootstrap.CroppedImageTemplate = CroppedImageTemplate;
        var CroppedImageController = /** @class */ (function (_super) {
            __extends(CroppedImageController, _super);
            function CroppedImageController($scope, $uibModal, $log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                _this.$scope = $scope;
                _this.$uibModal = $uibModal;
                _this.imgClass = "img-responsive";
                return _this;
            }
            CroppedImageController.prototype.add = function () {
                var _this = this;
                var modalInstance = this.$uibModal.open({
                    templateUrl: Bootstrap.CroppedImageModalTemplate.templateName,
                    controller: Bootstrap.CroppedImageModalController,
                    controllerAs: "$ctrl",
                    resolve: {
                        size: function () { return _this.size; },
                        format: function () { return _this.format; },
                        quality: function () { return _this.quality; },
                        aspect: function () { return _this.aspect; }
                    }
                });
                modalInstance.result
                    .then(function (selectedItem) {
                    if (!_this.role) {
                        _this.role = _this.object.session.create("Media");
                    }
                    var media = _this.role;
                    media.InDataUri = selectedItem;
                })
                    .catch(function () { });
            };
            CroppedImageController.bindings = {
                object: "<",
                relation: "@",
                imgClass: "@",
                size: "<",
                format: "<",
                quality: "<",
                aspect: "<"
            };
            CroppedImageController.$inject = ["$scope", "$uibModal", "$log", "$translate"];
            return CroppedImageController;
        }(Bootstrap.Field));
        Bootstrap.CroppedImageController = CroppedImageController;
        angular
            .module("allors")
            .component("bCroppedImage", {
            controller: CroppedImageController,
            templateUrl: CroppedImageTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: CroppedImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var CroppedImageModalTemplate = /** @class */ (function () {
            function CroppedImageModalTemplate() {
            }
            CroppedImageModalTemplate.register = function (templateCache) {
                templateCache.put(CroppedImageModalTemplate.templateName, CroppedImageModalTemplate.view);
            };
            CroppedImageModalTemplate.templateName = "allors/bootstrap/cropped-image/modal";
            CroppedImageModalTemplate.view = "\n<div class=\"modal-header\">\n    <h3 class=\"modal-title\">Image</h3>\n</div>\n\n<div class=\"modal-body\">\n    \n    <div class=\"row\" style=\"height:20vw;\">\n        <div class=\"col-sm-6\" style=\"height:100%;\">\n            <ui-cropper   image=\"$ctrl.image\" \n                        area-min-size=\"$ctrl.size\"\n                        result-image=\"$ctrl.croppedImage\" \n                        result-image-size=\"$ctrl.size\"\n                        result-image-format=\"$ctrl.format\"\n                        result-image-quality=\"$ctrl.quality\",\n                        aspect-ratio=\"$ctrl.aspect\">\n            </ui-cropper>\n        </div>\n\n        <div class=\"col-sm-6 center-block\" style=\"height:100%;\">\n            <img ng-if=\"$ctrl.croppedImage\" ng-src=\"{{$ctrl.croppedImage}}\" class=\"img-responsive img-thumbnail\" style=\"vertical-align: middle; height: 90%\"/>\n        </div>\n    </div>\n\n</div>\n\n<div class=\"modal-footer\">\n    <div class=\"pull-left\">\n        <label class=\"btn btn-default\" for=\"file-selector\">\n            <input id=\"file-selector\" type=\"file\" style=\"display:none;\" model-data-uri=\"$ctrl.image\">\n            Select file\n        </label>\n    </div>\n\n    <button class=\"btn btn-primary\" type=\"button\" ng-click=\"$ctrl.ok()\">OK</button>\n    <button class=\"btn btn-danger\" type=\"button\" ng-click=\"$ctrl.cancel()\">Cancel</button>\n</div>\n";
            return CroppedImageModalTemplate;
        }());
        Bootstrap.CroppedImageModalTemplate = CroppedImageModalTemplate;
        var CroppedImageModalController = /** @class */ (function () {
            function CroppedImageModalController($scope, $uibModalInstance, $log, $translate, size, format, quality, aspect) {
                this.$scope = $scope;
                this.$uibModalInstance = $uibModalInstance;
                this.size = size;
                this.format = format;
                this.quality = quality;
                this.aspect = aspect;
                this.image = "";
                this.croppedImage = "";
            }
            CroppedImageModalController.prototype.ok = function () {
                this.$uibModalInstance.close(this.croppedImage);
            };
            CroppedImageModalController.prototype.cancel = function () {
                this.$uibModalInstance.dismiss("cancel");
            };
            CroppedImageModalController.$inject = ["$scope", "$uibModalInstance", "$log", "$translate", "size", "format", "quality", "aspect"];
            return CroppedImageModalController;
        }());
        Bootstrap.CroppedImageModalController = CroppedImageModalController;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var DatepickerPopupTemplate = /** @class */ (function () {
            function DatepickerPopupTemplate() {
            }
            DatepickerPopupTemplate.createDefaultView = function () {
                return "\n<p class=\"input-group\">\n    <input type=\"date\" placeholder=\"{{$ctrl.placeholder}}\" class=\"form-control\" datepicker-append-to-body=\"$ctrl.appendToBody\"\n            uib-datepicker-popup \n            is-open=\"$ctrl.opened\" \n            ng-model=\"$ctrl.role\"\n            ng-model-options=\"$ctrl.modelOptions\"\n            ng-disabled=\"!$ctrl.canWrite\"\n            ng-required=\"$ctrl.roleType.isRequired\">\n    <span class=\"input-group-btn\">\n        <button type=\"button\" class=\"btn btn-default\" ng-click=\"$ctrl.opened = true\"><i class=\"glyphicon glyphicon-calendar\"></i></button>\n    </span>\n</p>\n";
            };
            DatepickerPopupTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = DatepickerPopupTemplate.createDefaultView(); }
                templateCache.put(DatepickerPopupTemplate.templateName, view);
            };
            DatepickerPopupTemplate.templateName = "allors/bootstrap/datepicker-popup";
            return DatepickerPopupTemplate;
        }());
        Bootstrap.DatepickerPopupTemplate = DatepickerPopupTemplate;
        var DatepickerPopupController = /** @class */ (function (_super) {
            __extends(DatepickerPopupController, _super);
            function DatepickerPopupController($log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                _this.timezone = "UTC";
                _this.modelOptions = {};
                return _this;
            }
            DatepickerPopupController.prototype.$onInit = function () {
                this.modelOptions.timezone = this.timezone;
            };
            DatepickerPopupController.bindings = {
                object: "<",
                relation: "@",
                timezone: "@",
                appendToBody: "<",
            };
            DatepickerPopupController.$inject = ["$log", "$translate"];
            return DatepickerPopupController;
        }(Bootstrap.Field));
        Bootstrap.DatepickerPopupController = DatepickerPopupController;
        angular
            .module("allors")
            .component("bDatepickerPopup", {
            controller: DatepickerPopupController,
            templateUrl: DatepickerPopupTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: DatepickerPopupController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var EnumTemplate = /** @class */ (function () {
            function EnumTemplate() {
            }
            EnumTemplate.createDefaultView = function () {
                return "\n<select class=\"form-control\" \n            ng-model=\"$ctrl.role\" \n            ng-disabled=\"!$ctrl.canWrite\" \n            ng-required=\"$ctrl.roleType.isRequired\"\n            ng-options=\"option.value as option.name for option in $ctrl.options\">\n    <option ng-if=\"!$ctrl.roleType.isRequired\" value=\"\"></option>     \n</select>\n";
            };
            EnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = EnumTemplate.createDefaultView(); }
                templateCache.put(EnumTemplate.templateName, view);
            };
            EnumTemplate.templateName = "allors/bootstrap/enum";
            return EnumTemplate;
        }());
        Bootstrap.EnumTemplate = EnumTemplate;
        var Enum = /** @class */ (function () {
            function Enum(value, name) {
                this.value = value;
                this.name = name;
            }
            return Enum;
        }());
        Bootstrap.Enum = Enum;
        var EnumController = /** @class */ (function (_super) {
            __extends(EnumController, _super);
            function EnumController($log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                if (!_this.options) {
                    var type = eval(_this.fullTypeName);
                    if (type) {
                        var lastIndex = _this.fullTypeName.lastIndexOf(".");
                        var typeName = _this.fullTypeName.substr(lastIndex + 1);
                        _this.options = [];
                        for (var k in type) {
                            if (type.hasOwnProperty(k)) {
                                var value = type[k];
                                if (typeof value === "number") {
                                    var name_1 = type[value];
                                    var humanizedName = Allors.Filters.Humanize.filter(name_1);
                                    var enumeration = new Enum(value, humanizedName);
                                    _this.options.push(enumeration);
                                    (function (enumeration, key1, key2) {
                                        _this.translate(key1, key2, function (translatedName) {
                                            if (translatedName) {
                                                enumeration.name = translatedName;
                                            }
                                        });
                                    })(enumeration, "enum_" + typeName + "_" + value, "enum_" + typeName + "_" + name_1);
                                }
                            }
                        }
                    }
                }
                return _this;
            }
            EnumController.bindings = {
                object: "<",
                relation: "@",
                fullTypeName: "@enum",
                options: "<"
            };
            EnumController.$inject = ["$log", "$translate"];
            return EnumController;
        }(Bootstrap.Field));
        Bootstrap.EnumController = EnumController;
        angular
            .module("allors")
            .component("bEnum", {
            controller: EnumController,
            templateUrl: EnumTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: EnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="Form.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var ImageTemplate = /** @class */ (function () {
            function ImageTemplate() {
            }
            ImageTemplate.createDefaultView = function () {
                return "\n<div ng-if=\"!$ctrl.role\">\n    <button type=\"button\" class=\"btn btn-default\" ng-click=\"$ctrl.add()\">Add new image</button>\n</div>\n        \n<div ng-if=\"$ctrl.role.InDataUri\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"{{$ctrl.role.InDataUri}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n\n<div ng-if=\"!$ctrl.role.InDataUri && $ctrl.role\">\n    <a ng-click=\"$ctrl.add()\">\n        <img ng-src=\"/media/display/{{$ctrl.role.UniqueId}}?revision={{$ctrl.role.Revision}}\" ng-class=\"$ctrl.imgClass\"/>\n    </a>\n</div>\n";
            };
            ImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = ImageTemplate.createDefaultView(); }
                templateCache.put(ImageTemplate.templateName, view);
            };
            ImageTemplate.templateName = "allors/bootstrap/image";
            return ImageTemplate;
        }());
        Bootstrap.ImageTemplate = ImageTemplate;
        var ImageController = /** @class */ (function (_super) {
            __extends(ImageController, _super);
            function ImageController($scope, $uibModal, $log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                _this.$scope = $scope;
                _this.$uibModal = $uibModal;
                _this.imgClass = "img-responsive";
                return _this;
            }
            ImageController.prototype.add = function () {
                var _this = this;
                var modalInstance = this.$uibModal.open({
                    templateUrl: Bootstrap.ImageModalTemplate.templateName,
                    controller: Bootstrap.ImageModalController,
                    controllerAs: "$ctrl",
                    resolve: {
                        size: function () { return _this.size; },
                        format: function () { return _this.format; },
                        quality: function () { return _this.quality; },
                        aspect: function () { return _this.aspect; }
                    }
                });
                modalInstance.result
                    .then(function (selectedItem) {
                    if (!_this.role) {
                        _this.role = _this.object.session.create("Media");
                    }
                    var media = _this.role;
                    media.InDataUri = selectedItem;
                })
                    .catch(function () { });
                ;
            };
            ImageController.bindings = {
                object: "<",
                relation: "@",
                imgClass: "@",
                size: "<",
                format: "<",
                quality: "<",
                aspect: "<"
            };
            ImageController.$inject = ["$scope", "$uibModal", "$log", "$translate"];
            return ImageController;
        }(Bootstrap.Field));
        Bootstrap.ImageController = ImageController;
        angular
            .module("allors")
            .component("bImage", {
            controller: ImageController,
            templateUrl: ImageTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: ImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var ImageModalTemplate = /** @class */ (function () {
            function ImageModalTemplate() {
            }
            ImageModalTemplate.register = function (templateCache) {
                templateCache.put(ImageModalTemplate.templateName, ImageModalTemplate.view);
            };
            ImageModalTemplate.templateName = "allors/bootstrap/image/modal";
            ImageModalTemplate.view = "\n<div class=\"modal-header\">\n    <h3 class=\"modal-title\">Image</h3>\n</div>\n\n<div class=\"modal-body\">\n    \n    <div class=\"row\" style=\"height:20vw;\">\n        <div class=\"col-sm-12 center-block\" style=\"height:100%;\">\n            <img ng-if=\"$ctrl.image\" ng-src=\"{{$ctrl.image}}\" class=\"img-responsive img-thumbnail\" style=\"vertical-align: middle; height: 90%\"/>\n        </div>\n    </div>\n\n</div>\n\n<div class=\"modal-footer\">\n    <div class=\"pull-left\">\n        <label class=\"btn btn-default\" for=\"file-selector\">\n            <input id=\"file-selector\" type=\"file\" style=\"display:none;\" model-data-uri=\"$ctrl.image\">\n            Upload an image\n        </label>\n    </div>\n\n    <button ng-enabled=\"$ctrl.image\" class=\"btn btn-primary\" type=\"button\" ng-click=\"$ctrl.ok()\">OK</button>\n    <button class=\"btn btn-danger\" type=\"button\" ng-click=\"$ctrl.cancel()\">Cancel</button>\n</div>\n";
            return ImageModalTemplate;
        }());
        Bootstrap.ImageModalTemplate = ImageModalTemplate;
        var ImageModalController = /** @class */ (function () {
            function ImageModalController($scope, $uibModalInstance, $log, $translate, size, format, quality, aspect) {
                this.$scope = $scope;
                this.$uibModalInstance = $uibModalInstance;
                this.size = size;
                this.format = format;
                this.quality = quality;
                this.aspect = aspect;
                this.image = "";
                this.croppedImage = "";
            }
            ImageModalController.prototype.ok = function () {
                this.$uibModalInstance.close(this.image);
            };
            ImageModalController.prototype.cancel = function () {
                this.$uibModalInstance.dismiss("cancel");
            };
            ImageModalController.$inject = ["$scope", "$uibModalInstance", "$log", "$translate", "size", "format", "quality", "aspect"];
            return ImageModalController;
        }());
        Bootstrap.ImageModalController = ImageModalController;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var RadioTemplate = /** @class */ (function () {
            function RadioTemplate() {
            }
            RadioTemplate.createDefaultView = function () {
                return "\n<label>\n<input type=\"radio\" \n        ng-model=\"$ctrl.role\" \n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        ng-value=\"true\">\n{{$ctrl.trueLabel}}\n</label>\n\n<br/>\n\n<label>\n    <input type=\"radio\" \n        ng-model=\"$ctrl.role\" \n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        ng-value=\"false\">\n    {{$ctrl.falseLabel}}\n</label>\n\n<br/>\n\n\n";
            };
            RadioTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = RadioTemplate.createDefaultView(); }
                templateCache.put(RadioTemplate.templateName, view);
            };
            RadioTemplate.templateName = "allors/bootstrap/radio";
            return RadioTemplate;
        }());
        Bootstrap.RadioTemplate = RadioTemplate;
        var RadioController = /** @class */ (function (_super) {
            __extends(RadioController, _super);
            function RadioController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            RadioController.prototype.$onInit = function () {
            };
            RadioController.bindings = {
                object: "<",
                relation: "@",
                trueLabel: "@true",
                falseLabel: "@false"
            };
            RadioController.$inject = ["$log", "$translate"];
            return RadioController;
        }(Bootstrap.Field));
        Bootstrap.RadioController = RadioController;
        angular
            .module("allors")
            .component("bRadio", {
            controller: RadioController,
            templateUrl: RadioTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: RadioController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var RadioButtonTemplate = /** @class */ (function () {
            function RadioButtonTemplate() {
            }
            RadioButtonTemplate.createDefaultView = function () {
                return "\n<div class=\"btn-group\">\n    <label class=\"btn btn-info\" \n           uib-btn-radio=\"true\" \n           ng-model=\"$ctrl.role\" \n           ng-disabled=\"!$ctrl.canWrite\"\n           ng-required=\"$ctrl.roleType.isRequired\">{{$ctrl.trueLabel}}</label>\n    <label class=\"btn btn-info\" \n           uib-btn-radio=\"false\"\n           ng-model=\"$ctrl.role\"\n           ng-disabled=\"!$ctrl.canWrite\"\n           ng-required=\"$ctrl.roleType.isRequired\">{{$ctrl.falseLabel}}</label>\n</div>\n";
            };
            RadioButtonTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = RadioButtonTemplate.createDefaultView(); }
                templateCache.put(RadioButtonTemplate.templateName, view);
            };
            RadioButtonTemplate.templateName = "allors/bootstrap/radio-button";
            return RadioButtonTemplate;
        }());
        Bootstrap.RadioButtonTemplate = RadioButtonTemplate;
        var RadioButtonController = /** @class */ (function (_super) {
            __extends(RadioButtonController, _super);
            function RadioButtonController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            RadioButtonController.bindings = {
                object: "<",
                relation: "@",
                trueLabel: "@true",
                falseLabel: "@false"
            };
            RadioButtonController.$inject = ["$log", "$translate"];
            return RadioButtonController;
        }(Bootstrap.Field));
        Bootstrap.RadioButtonController = RadioButtonController;
        angular
            .module("allors")
            .component("bRadioButton", {
            controller: RadioButtonController,
            templateUrl: RadioButtonTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: RadioButtonController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectTemplate = /** @class */ (function () {
            function SelectTemplate() {
            }
            SelectTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.roleType.isOne && $ctrl.options !== undefined\" ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select a value\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isOne && $ctrl.options === undefined\" ng-model=\"$ctrl.role\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select a value\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isMany && $ctrl.options !== undefined\" multiple ng-model=\"$ctrl.sortedRole\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select values\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.roleType.isMany && $ctrl.options === undefined\" multiple ng-model=\"$ctrl.sortedRole\" ng-disabled=\"!$ctrl.canWrite\" ng-required=\"$ctrl.roleType.isRequired\">\n    <ui-select-match placeholder=\"Select values\" allow-clear=\"{{!$ctrl.roleType.isRequired}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectTemplate.createDefaultView(); }
                templateCache.put(SelectTemplate.templateName, view);
            };
            SelectTemplate.templateName = "allors/bootstrap/select";
            return SelectTemplate;
        }());
        Bootstrap.SelectTemplate = SelectTemplate;
        var SelectController = /** @class */ (function (_super) {
            __extends(SelectController, _super);
            function SelectController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            SelectController.prototype.filterFunction = function (criteria) {
                var _this = this;
                return function (object) {
                    // If no criteria then all is a match
                    if (criteria === undefined || criteria.length === 0) {
                        return true;
                    }
                    var value = object[_this.display];
                    if (value) {
                        if (value.toLowerCase) {
                            var lowerCaseValue = value.toLowerCase();
                            var lowerCaseCriteria = criteria.toLowerCase();
                            return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
                        }
                        else {
                            return value.toString().indexOf(criteria) >= 0;
                        }
                    }
                    return false;
                };
            };
            SelectController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectController.prototype.refresh = function (criteria) {
                var _this = this;
                this.lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                })
                    .catch(function () { });
                ;
            };
            Object.defineProperty(SelectController.prototype, "sortedRole", {
                get: function () {
                    var _this = this;
                    if (!this.role) {
                        this._sortedRole = null;
                    }
                    else {
                        var arraysAreEqual = function (a, b) {
                            if (!b || a.length !== b.length) {
                                return false;
                            }
                            for (var i = 0; i < a.length; i++) {
                                if (a[i] !== b[i]) {
                                    return false;
                                }
                            }
                            return true;
                        };
                        var newSortedRole = this.role.slice(0).sort(function (a, b) { return a[_this.orderBy()] < b[_this.orderBy()] ? -1 : 1; });
                        if (!arraysAreEqual(newSortedRole, this._sortedRole)) {
                            this._sortedRole = newSortedRole;
                        }
                    }
                    return this._sortedRole;
                },
                set: function (value) {
                    this.role = value;
                },
                enumerable: true,
                configurable: true
            });
            SelectController.bindings = {
                object: "<",
                relation: "@",
                display: "@",
                options: "<",
                order: "<",
                refreshDelay: "<",
                lookup: "&lookup"
            };
            SelectController.$inject = ["$log", "$translate"];
            return SelectController;
        }(Bootstrap.Field));
        Bootstrap.SelectController = SelectController;
        angular
            .module("allors")
            .component("bSelect", {
            controller: SelectController,
            templateUrl: SelectTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: SelectController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectManyTemplate = /** @class */ (function () {
            function SelectManyTemplate() {
            }
            SelectManyTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.options !== undefined\" multiple ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\">\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.options === undefined\" multiple ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\">\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$item[$ctrl.display]}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectManyTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectManyTemplate.createDefaultView(); }
                templateCache.put(SelectManyTemplate.templateName, view);
            };
            SelectManyTemplate.templateName = "allors/bootstrap/select-many";
            return SelectManyTemplate;
        }());
        Bootstrap.SelectManyTemplate = SelectManyTemplate;
        var SelectManyController = /** @class */ (function () {
            function SelectManyController($log) {
                this.allowClear = true;
                this.placeholder = "Select values";
            }
            SelectManyController.prototype.$onInit = function () {
                if (!this.model) {
                    this.model = new Array();
                }
            };
            Object.defineProperty(SelectManyController.prototype, "displayValue", {
                get: function () {
                    return this.model && this.model[this.display];
                },
                enumerable: true,
                configurable: true
            });
            SelectManyController.prototype.filterFunction = function (criteria) {
                var _this = this;
                return function (object) {
                    // If no criteria then all is a match
                    if (criteria === undefined || criteria.length === 0) {
                        return true;
                    }
                    var value = object[_this.display];
                    if (value) {
                        if (value.toLowerCase) {
                            var lowerCaseValue = value.toLowerCase();
                            var lowerCaseCriteria = criteria.toLowerCase();
                            return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
                        }
                        else {
                            return value.toString().indexOf(criteria) >= 0;
                        }
                    }
                    return false;
                };
            };
            SelectManyController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectManyController.prototype.refresh = function (criteria) {
                var _this = this;
                this.lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                })
                    .catch(function () { });
                ;
            };
            SelectManyController.bindings = {
                model: "=",
                display: "@",
                options: "<",
                refreshDelay: "<",
                lookup: "&lookup",
                order: "<",
                allowClear: "<",
                appendToBody: "<",
                placeholder: "@"
            };
            SelectManyController.$inject = ["$log", "$translate"];
            return SelectManyController;
        }());
        Bootstrap.SelectManyController = SelectManyController;
        angular
            .module("allors")
            .component("bSelectMany", {
            controller: SelectManyController,
            templateUrl: SelectManyTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: SelectManyController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var SelectOneTemplate = /** @class */ (function () {
            function SelectOneTemplate() {
            }
            SelectOneTemplate.createDefaultView = function () {
                return "\n<ui-select ng-if=\"$ctrl.options !== undefined\" ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\" >\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.options | filter: $ctrl.filterFunction($select.search) | orderBy: $ctrl.orderBy()\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n\n<ui-select ng-if=\"$ctrl.options === undefined\" ng-model=\"$ctrl.model\" append-to-body=\"$ctrl.appendToBody\" >\n    <ui-select-match placeholder=\"{{$ctrl.placeholder}}\" allow-clear=\"{{$ctrl.allowClear}}\">{{$ctrl.displayValue}}</ui-select-match>\n    <ui-select-choices repeat=\"item in $ctrl.asyncOptions | orderBy: $ctrl.orderBy()\" refresh=\"$ctrl.refresh($select.search)\" refresh-delay=\"$ctrl.refreshDelay\">\n        <div ng-bind-html=\"item[$ctrl.display] | highlight: $select.search\"></div>\n    </ui-select-choices>\n</ui-select>\n";
            };
            SelectOneTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = SelectOneTemplate.createDefaultView(); }
                templateCache.put(SelectOneTemplate.templateName, view);
            };
            SelectOneTemplate.templateName = "allors/bootstrap/select-one";
            return SelectOneTemplate;
        }());
        Bootstrap.SelectOneTemplate = SelectOneTemplate;
        var SelectOneController = /** @class */ (function () {
            function SelectOneController($log) {
                this.allowClear = true;
                this.placeholder = "Select a value";
            }
            Object.defineProperty(SelectOneController.prototype, "displayValue", {
                get: function () {
                    return this.model && this.model[this.display];
                },
                enumerable: true,
                configurable: true
            });
            SelectOneController.prototype.filterFunction = function (criteria) {
                var _this = this;
                return function (object) {
                    // If no criteria then all is a match
                    if (criteria === undefined || criteria.length === 0) {
                        return true;
                    }
                    var value = object[_this.display];
                    if (value) {
                        if (value.toLowerCase) {
                            var lowerCaseValue = value.toLowerCase();
                            var lowerCaseCriteria = criteria.toLowerCase();
                            return lowerCaseValue.indexOf(lowerCaseCriteria) >= 0;
                        }
                        else {
                            return value.toString().indexOf(criteria) >= 0;
                        }
                    }
                    return false;
                };
            };
            SelectOneController.prototype.orderBy = function () {
                if (this.order) {
                    return this.order;
                }
                else {
                    return this.display;
                }
            };
            SelectOneController.prototype.refresh = function (criteria) {
                var _this = this;
                this.lookup({ criteria: criteria })
                    .then(function (results) {
                    _this.asyncOptions = results;
                })
                    .catch(function () { });
                ;
            };
            SelectOneController.bindings = {
                model: "=",
                display: "@",
                options: "<",
                refreshDelay: "<",
                lookup: "&lookup",
                order: "<",
                allowClear: "<",
                appendToBody: "<",
                placeholder: "@"
            };
            SelectOneController.$inject = ["$log", "$translate"];
            return SelectOneController;
        }());
        Bootstrap.SelectOneController = SelectOneController;
        angular
            .module("allors")
            .component("bSelectOne", {
            controller: SelectOneController,
            templateUrl: SelectOneTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: SelectOneController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var StaticTemplate = /** @class */ (function () {
            function StaticTemplate() {
            }
            StaticTemplate.createDefaultView = function () {
                return "\n<p class=\"form-control-static\" ng-bind-html=\"$ctrl.role\"></p>\n";
            };
            StaticTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = StaticTemplate.createDefaultView(); }
                templateCache.put(StaticTemplate.templateName, view);
            };
            StaticTemplate.templateName = "allors/bootstrap/static";
            return StaticTemplate;
        }());
        Bootstrap.StaticTemplate = StaticTemplate;
        var StaticController = /** @class */ (function (_super) {
            __extends(StaticController, _super);
            function StaticController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            StaticController.bindings = {
                object: "<",
                relation: "@"
            };
            StaticController.$inject = ["$log", "$translate"];
            return StaticController;
        }(Bootstrap.Field));
        Bootstrap.StaticController = StaticController;
        angular
            .module("allors")
            .component("bStatic", {
            controller: StaticController,
            templateUrl: StaticTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: StaticController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var StaticEnumTemplate = /** @class */ (function () {
            function StaticEnumTemplate() {
            }
            StaticEnumTemplate.createDefaultView = function () {
                return "\n<p class=\"form-control-static\" ng-bind=\"$ctrl.enum.name\"></p>\n";
            };
            StaticEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = StaticEnumTemplate.createDefaultView(); }
                templateCache.put(StaticEnumTemplate.templateName, view);
            };
            StaticEnumTemplate.templateName = "allors/bootstrap/static-enum";
            return StaticEnumTemplate;
        }());
        Bootstrap.StaticEnumTemplate = StaticEnumTemplate;
        var StaticEnum = /** @class */ (function () {
            function StaticEnum(value, name) {
                this.value = value;
                this.name = name;
            }
            return StaticEnum;
        }());
        Bootstrap.StaticEnum = StaticEnum;
        var StaticEnumController = /** @class */ (function (_super) {
            __extends(StaticEnumController, _super);
            function StaticEnumController($log, $translate) {
                var _this = _super.call(this, $log, $translate) || this;
                var type = eval(_this.fullTypeName);
                if (type) {
                    var lastIndex = _this.fullTypeName.lastIndexOf(".");
                    var typeName = _this.fullTypeName.substr(lastIndex + 1);
                    _this.enums = [];
                    for (var k in type) {
                        if (type.hasOwnProperty(k)) {
                            var value = type[k];
                            if (typeof value === "number") {
                                var name_2 = type[value];
                                var humanizedName = Allors.Filters.Humanize.filter(name_2);
                                var enumeration = new StaticEnum(value, humanizedName);
                                _this.enums.push(enumeration);
                                (function (enumeration, key1, key2) {
                                    _this.translate(key1, key2, function (translatedName) {
                                        if (translatedName) {
                                            enumeration.name = translatedName;
                                        }
                                    });
                                })(enumeration, "enum_" + typeName + "_" + value, "enum_" + typeName + "_" + name_2);
                            }
                        }
                    }
                }
                return _this;
            }
            Object.defineProperty(StaticEnumController.prototype, "enum", {
                get: function () {
                    var _this = this;
                    var filtered = this.enums.filter(function (v) { return v.value === _this.role; });
                    return !!filtered ? filtered[0] : undefined;
                },
                enumerable: true,
                configurable: true
            });
            StaticEnumController.bindings = {
                object: "<",
                relation: "@",
                fullTypeName: "@enum"
            };
            StaticEnumController.$inject = ["$log", "$translate"];
            return StaticEnumController;
        }(Bootstrap.Field));
        Bootstrap.StaticEnumController = StaticEnumController;
        angular
            .module("allors")
            .component("bStaticEnum", {
            controller: StaticEnumController,
            templateUrl: StaticEnumTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: StaticEnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextTemplate = /** @class */ (function () {
            function TextTemplate() {
            }
            TextTemplate.createDefaultView = function () {
                return "\n<div class=\"input-group\">\n    <input placeholder=\"{{$ctrl.placeholder}}\" \n            class=\"form-control\"\n            name=\"{{$ctrl.name || $ctrl.relation}}\"\n            ng-model=\"$ctrl.role\"\n            ng-disabled=\"!$ctrl.canWrite\"\n            ng-required=\"$ctrl.roleType.isRequired\"\n            ng-attr-type=\"{{$ctrl.htmlType}}\">\n    <span ng-if=\"$ctrl.addon\" class=\"input-group-addon\">{{$ctrl.addon}}</span>\n</div>\n";
            };
            ;
            TextTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextTemplate.createDefaultView(); }
                templateCache.put(TextTemplate.templateName, view);
            };
            TextTemplate.templateName = "allors/bootstrap/text";
            return TextTemplate;
        }());
        Bootstrap.TextTemplate = TextTemplate;
        var TextController = /** @class */ (function (_super) {
            __extends(TextController, _super);
            function TextController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            Object.defineProperty(TextController.prototype, "htmlType", {
                get: function () {
                    if (this.roleType) {
                        if (this.roleType.objectType.isInteger ||
                            this.roleType.objectType.isDecimal ||
                            this.roleType.objectType.isFloat) {
                            return "number";
                        }
                    }
                    return "text";
                },
                enumerable: true,
                configurable: true
            });
            TextController.bindings = {
                object: "<",
                relation: "@",
                addon: "<"
            };
            TextController.$inject = ["$log", "$translate"];
            return TextController;
        }(Bootstrap.Field));
        Bootstrap.TextController = TextController;
        angular
            .module("allors")
            .component("bText", {
            controller: TextController,
            templateUrl: TextTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: TextController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextAngularTemplate = /** @class */ (function () {
            function TextAngularTemplate() {
            }
            TextAngularTemplate.createDefaultView = function () {
                return "\n<div ng-if=\"$ctrl.canWrite\">\n<text-angular ng-model=\"$ctrl.role\" '/>\n</div>\n<div ng-if=\"!$ctrl.canWrite\">\n<text-angular ta-bind=\"text\" ng-model=\"$ctrl.role\" '/>\n</div>";
            };
            TextAngularTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextAngularTemplate.createDefaultView(); }
                templateCache.put(TextAngularTemplate.templateName, view);
            };
            TextAngularTemplate.templateName = "allors/bootstrap/text-angular";
            return TextAngularTemplate;
        }());
        Bootstrap.TextAngularTemplate = TextAngularTemplate;
        var TextAngularController = /** @class */ (function (_super) {
            __extends(TextAngularController, _super);
            function TextAngularController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            TextAngularController.bindings = {
                object: "<",
                relation: "@"
            };
            TextAngularController.$inject = ["$log", "$translate"];
            return TextAngularController;
        }(Bootstrap.Field));
        Bootstrap.TextAngularController = TextAngularController;
        angular
            .module("allors")
            .component("bTextAngular", {
            controller: TextAngularController,
            templateUrl: TextAngularTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: TextAngularController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TextareaTemplate = /** @class */ (function () {
            function TextareaTemplate() {
            }
            TextareaTemplate.createDefaultView = function () {
                return "\n<textarea placeholder=\"{{$ctrl.placeholder}}\" class=\"form-control\"\n        ng-model=\"$ctrl.role\"\n        ng-disabled=\"!$ctrl.canWrite\"\n        ng-required=\"$ctrl.roleType.isRequired\"\n        rows=\"{{$ctrl.rows}}\">\n";
            };
            TextareaTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TextareaTemplate.createDefaultView(); }
                templateCache.put(TextareaTemplate.templateName, view);
            };
            TextareaTemplate.templateName = "allors/bootstrap/textarea";
            return TextareaTemplate;
        }());
        Bootstrap.TextareaTemplate = TextareaTemplate;
        var TextareaController = /** @class */ (function (_super) {
            __extends(TextareaController, _super);
            function TextareaController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            TextareaController.bindings = {
                object: "<",
                relation: "@",
                rows: "<"
            };
            TextareaController.$inject = ["$log", "$translate"];
            return TextareaController;
        }(Bootstrap.Field));
        Bootstrap.TextareaController = TextareaController;
        angular
            .module("allors")
            .component("bTextarea", {
            controller: TextareaController,
            templateUrl: TextareaTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: TextareaController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../allors.module.ts" />
/// <reference path="internal/Field.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var TypeaheadTemplate = /** @class */ (function () {
            function TypeaheadTemplate() {
            }
            TypeaheadTemplate.createDefaultView = function () {
                return "\n<input  type=\"text\"\n        ng-disabled=\"!$ctrl.canWrite\" \n        ng-required=\"$ctrl.roleType.isRequired\" \n        ng-model=\"$ctrl.role\"\n        uib-typeahead=\"item for item in $ctrl.options | filter:$viewValue | limitTo:10\"\n        class=\"form-control\" />\n";
            };
            TypeaheadTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = TypeaheadTemplate.createDefaultView(); }
                templateCache.put(TypeaheadTemplate.templateName, view);
            };
            TypeaheadTemplate.templateName = "allors/bootstrap/typeahead";
            return TypeaheadTemplate;
        }());
        Bootstrap.TypeaheadTemplate = TypeaheadTemplate;
        var TypeaheadController = /** @class */ (function (_super) {
            __extends(TypeaheadController, _super);
            function TypeaheadController($log, $translate) {
                return _super.call(this, $log, $translate) || this;
            }
            TypeaheadController.bindings = {
                object: "<",
                relation: "@",
                options: "<"
            };
            TypeaheadController.$inject = ["$log", "$translate"];
            return TypeaheadController;
        }(Bootstrap.Field));
        Bootstrap.TypeaheadController = TypeaheadController;
        angular
            .module("allors")
            .component("bTypeahead", {
            controller: TypeaheadController,
            templateUrl: TypeaheadTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: TypeaheadController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabelTemplate = /** @class */ (function () {
            function LabelTemplate() {
            }
            LabelTemplate.register = function (templateCache) {
                templateCache.put(LabelTemplate.templateName, LabelTemplate.view);
            };
            LabelTemplate.templateName = "allors/bootstrap/label";
            LabelTemplate.view = "\n<label class=\"control-label\" ng-class=\"$ctrl.form.horizontal ? 'col-sm-2' : '' \">{{$ctrl.field.label}}\n    <span ng-if=\"$ctrl.field.help\" class=\"fa fa-question-circle\"\n          uib-tooltip=\"{{$ctrl.field.help}}\"\n          tooltip-placement=\"right\">\n    </span>\n</label>\n";
            return LabelTemplate;
        }());
        Bootstrap.LabelTemplate = LabelTemplate;
        var LabelComponent = /** @class */ (function () {
            function LabelComponent($log) {
            }
            LabelComponent.$inject = ["$log"];
            return LabelComponent;
        }());
        angular
            .module("allors")
            .component("bLabel", {
            controller: LabelComponent,
            templateUrl: LabelTemplate.templateName,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTemplate = /** @class */ (function () {
            function LabeledTemplate() {
            }
            LabeledTemplate.register = function (templateCache) {
                templateCache.put(LabeledTemplate.templateName, LabeledTemplate.view);
            };
            LabeledTemplate.templateName = "allors/bootstrap/labeled";
            LabeledTemplate.view = "<div ng-class=\"{true: 'form-group required', false :'form-group'}[{{$ctrl.field.roleType.isRequired}}]\" ng-if=\"$ctrl.field.canRead\">\n<ng-transclude/>\n</div>";
            return LabeledTemplate;
        }());
        Bootstrap.LabeledTemplate = LabeledTemplate;
        var LabeledComponent = /** @class */ (function () {
            function LabeledComponent($log) {
            }
            LabeledComponent.$inject = ["$log"];
            return LabeledComponent;
        }());
        angular
            .module("allors")
            .component("bLabeled", {
            controller: LabeledComponent,
            transclude: true,
            templateUrl: LabeledTemplate.templateName,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledInputTemplate = /** @class */ (function () {
            function LabeledInputTemplate() {
            }
            LabeledInputTemplate.register = function (templateCache) {
                templateCache.put(LabeledInputTemplate.templatename, LabeledInputTemplate.view);
            };
            LabeledInputTemplate.templatename = "allors/bootstrap/labeled-input";
            LabeledInputTemplate.view = "\n<div ng-class=\"$ctrl.form.horizontal ? 'col-sm-8' : ''\">\n<ng-transclude/>\n</div>\n";
            return LabeledInputTemplate;
        }());
        Bootstrap.LabeledInputTemplate = LabeledInputTemplate;
        var LabeledInputComponent = /** @class */ (function () {
            function LabeledInputComponent($log) {
            }
            LabeledInputComponent.$inject = ["$log"];
            return LabeledInputComponent;
        }());
        angular
            .module("allors")
            .component("bLabeledInput", {
            controller: LabeledInputComponent,
            transclude: true,
            templateUrl: LabeledInputTemplate.templatename,
            require: {
                form: "^bForm"
            },
            bindings: {
                field: "<"
            }
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledContentTemplate = /** @class */ (function () {
            function LabeledContentTemplate() {
            }
            LabeledContentTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.ContentTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledContentTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledContentTemplate.createDefaultView(); }
                templateCache.put(LabeledContentTemplate.templateName, view);
            };
            LabeledContentTemplate.templateName = "allors/bootstrap/labeled-Content";
            return LabeledContentTemplate;
        }());
        Bootstrap.LabeledContentTemplate = LabeledContentTemplate;
        angular
            .module("allors")
            .component("bLabeledContent", {
            controller: Bootstrap.ContentController,
            templateUrl: LabeledContentTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.ContentController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledCroppedImageTemplate = /** @class */ (function () {
            function LabeledCroppedImageTemplate() {
            }
            LabeledCroppedImageTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n " + Bootstrap.CroppedImageTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledCroppedImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledCroppedImageTemplate.createDefaultView(); }
                templateCache.put(LabeledCroppedImageTemplate.templateName, view);
            };
            LabeledCroppedImageTemplate.templateName = "allors/bootstrap/labeled-cropped-image";
            return LabeledCroppedImageTemplate;
        }());
        Bootstrap.LabeledCroppedImageTemplate = LabeledCroppedImageTemplate;
        angular
            .module("allors")
            .component("bLabeledCroppedImage", {
            controller: Bootstrap.CroppedImageController,
            templateUrl: LabeledCroppedImageTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.CroppedImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledDatepickerPopupTemplate = /** @class */ (function () {
            function LabeledDatepickerPopupTemplate() {
            }
            LabeledDatepickerPopupTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.DatepickerPopupTemplate.createDefaultView() + "\n    </b-input-group>\n</b-labeled>\n";
            };
            LabeledDatepickerPopupTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledDatepickerPopupTemplate.createDefaultView(); }
                templateCache.put(LabeledDatepickerPopupTemplate.templateName, view);
            };
            LabeledDatepickerPopupTemplate.templateName = "allors/bootstrap/labeled-datepicker-popup";
            return LabeledDatepickerPopupTemplate;
        }());
        Bootstrap.LabeledDatepickerPopupTemplate = LabeledDatepickerPopupTemplate;
        angular
            .module("allors")
            .component("bLabeledDatepickerPopup", {
            controller: Bootstrap.DatepickerPopupController,
            templateUrl: LabeledDatepickerPopupTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.DatepickerPopupController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledEnumTemplate = /** @class */ (function () {
            function LabeledEnumTemplate() {
            }
            LabeledEnumTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.EnumTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledEnumTemplate.createDefaultView(); }
                templateCache.put(LabeledEnumTemplate.templateName, view);
            };
            LabeledEnumTemplate.templateName = "allors/bootstrap/labeled-enum";
            return LabeledEnumTemplate;
        }());
        Bootstrap.LabeledEnumTemplate = LabeledEnumTemplate;
        angular
            .module("allors")
            .component("bLabeledEnum", {
            controller: Bootstrap.EnumController,
            templateUrl: LabeledEnumTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.EnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledImageTemplate = /** @class */ (function () {
            function LabeledImageTemplate() {
            }
            LabeledImageTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n " + Bootstrap.ImageTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledImageTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledImageTemplate.createDefaultView(); }
                templateCache.put(LabeledImageTemplate.templateName, view);
            };
            LabeledImageTemplate.templateName = "allors/bootstrap/labeled-image";
            return LabeledImageTemplate;
        }());
        Bootstrap.LabeledImageTemplate = LabeledImageTemplate;
        angular
            .module("allors")
            .component("bLabeledImage", {
            controller: Bootstrap.ImageController,
            templateUrl: LabeledImageTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.ImageController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledRadioTemplate = /** @class */ (function () {
            function LabeledRadioTemplate() {
            }
            LabeledRadioTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.RadioTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledRadioTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledRadioTemplate.createDefaultView(); }
                templateCache.put(LabeledRadioTemplate.templateName, view);
            };
            LabeledRadioTemplate.templateName = "allors/bootstrap/radio-group";
            return LabeledRadioTemplate;
        }());
        Bootstrap.LabeledRadioTemplate = LabeledRadioTemplate;
        angular
            .module("allors")
            .component("bLabeledRadio", {
            controller: Bootstrap.RadioController,
            templateUrl: ["$element", "$attrs", function () { return LabeledRadioTemplate.templateName; }],
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.RadioController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledRadioButtonTemplate = /** @class */ (function () {
            function LabeledRadioButtonTemplate() {
            }
            LabeledRadioButtonTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.RadioButtonTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledRadioButtonTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledRadioButtonTemplate.createDefaultView(); }
                templateCache.put(LabeledRadioButtonTemplate.templateName, view);
            };
            LabeledRadioButtonTemplate.templateName = "allors/bootstrap/radio-button-group";
            return LabeledRadioButtonTemplate;
        }());
        Bootstrap.LabeledRadioButtonTemplate = LabeledRadioButtonTemplate;
        angular
            .module("allors")
            .component("bLabeledRadioButton", {
            controller: Bootstrap.RadioButtonController,
            templateUrl: ["$element", "$attrs", function () { return LabeledRadioButtonTemplate.templateName; }],
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.RadioButtonController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledSelectTemplate = /** @class */ (function () {
            function LabeledSelectTemplate() {
            }
            LabeledSelectTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/> \n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.SelectTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledSelectTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledSelectTemplate.createDefaultView(); }
                templateCache.put(LabeledSelectTemplate.templateName, view);
            };
            LabeledSelectTemplate.templateName = "allors/bootstrap/labeled-select";
            return LabeledSelectTemplate;
        }());
        Bootstrap.LabeledSelectTemplate = LabeledSelectTemplate;
        angular
            .module("allors")
            .component("bLabeledSelect", {
            controller: Bootstrap.SelectController,
            templateUrl: LabeledSelectTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.SelectController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledStaticTemplate = /** @class */ (function () {
            function LabeledStaticTemplate() {
            }
            LabeledStaticTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.StaticTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledStaticTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledStaticTemplate.createDefaultView(); }
                templateCache.put(LabeledStaticTemplate.templateName, view);
            };
            LabeledStaticTemplate.templateName = "allors/bootstrap/static-group";
            return LabeledStaticTemplate;
        }());
        Bootstrap.LabeledStaticTemplate = LabeledStaticTemplate;
        angular
            .module("allors")
            .component("bLabeledStatic", {
            controller: Bootstrap.StaticController,
            templateUrl: LabeledStaticTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.StaticController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledStaticEnumTemplate = /** @class */ (function () {
            function LabeledStaticEnumTemplate() {
            }
            LabeledStaticEnumTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.StaticEnumTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-group>\n";
            };
            LabeledStaticEnumTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledStaticEnumTemplate.createDefaultView(); }
                templateCache.put(LabeledStaticEnumTemplate.templateName, view);
            };
            LabeledStaticEnumTemplate.templateName = "allors/bootstrap/labeled-static-enum";
            return LabeledStaticEnumTemplate;
        }());
        Bootstrap.LabeledStaticEnumTemplate = LabeledStaticEnumTemplate;
        angular
            .module("allors")
            .component("bLabeledStaticEnum", {
            controller: Bootstrap.StaticEnumController,
            templateUrl: LabeledStaticEnumTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.StaticEnumController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextTemplate = /** @class */ (function () {
            function LabeledTextTemplate() {
            }
            LabeledTextTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            ;
            LabeledTextTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextTemplate.createDefaultView(); }
                templateCache.put(LabeledTextTemplate.templateName, view);
            };
            LabeledTextTemplate.templateName = "allors/bootstrap/labeled-text";
            return LabeledTextTemplate;
        }());
        Bootstrap.LabeledTextTemplate = LabeledTextTemplate;
        angular
            .module("allors")
            .component("bLabeledText", {
            controller: Bootstrap.TextController,
            templateUrl: LabeledTextTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextAngularTemplate = /** @class */ (function () {
            function LabeledTextAngularTemplate() {
            }
            LabeledTextAngularTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextAngularTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTextAngularTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextAngularTemplate.createDefaultView(); }
                templateCache.put(LabeledTextAngularTemplate.templateName, view);
            };
            LabeledTextAngularTemplate.templateName = "allors/bootstrap/labeled-text-angular";
            return LabeledTextAngularTemplate;
        }());
        Bootstrap.LabeledTextAngularTemplate = LabeledTextAngularTemplate;
        angular
            .module("allors")
            .component("bLabeledTextAngular", {
            controller: Bootstrap.TextAngularController,
            templateUrl: LabeledTextAngularTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextAngularController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTextareaTemplate = /** @class */ (function () {
            function LabeledTextareaTemplate() {
            }
            LabeledTextareaTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/>\n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TextareaTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTextareaTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTextareaTemplate.createDefaultView(); }
                templateCache.put(LabeledTextareaTemplate.templateName, view);
            };
            LabeledTextareaTemplate.templateName = "allors/bootstrap/labeled-textarea";
            return LabeledTextareaTemplate;
        }());
        Bootstrap.LabeledTextareaTemplate = LabeledTextareaTemplate;
        angular
            .module("allors")
            .component("bLabeledTextarea", {
            controller: Bootstrap.TextareaController,
            templateUrl: LabeledTextareaTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TextareaController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../../allors.module.ts" />
/// <reference path="../Form.ts" />
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var LabeledTypeaheadTemplate = /** @class */ (function () {
            function LabeledTypeaheadTemplate() {
            }
            LabeledTypeaheadTemplate.createDefaultView = function () {
                return "\n<b-labeled field=\"$ctrl\">\n    <b-label field=\"$ctrl\"/> \n    <b-labeled-input field=\"$ctrl\">\n" + Bootstrap.TypeaheadTemplate.createDefaultView() + "\n    </b-labeled-input>\n</b-labeled>\n";
            };
            LabeledTypeaheadTemplate.register = function (templateCache, view) {
                if (view === void 0) { view = LabeledTypeaheadTemplate.createDefaultView(); }
                templateCache.put(LabeledTypeaheadTemplate.templateName, view);
            };
            LabeledTypeaheadTemplate.templateName = "allors/bootstrap/labeled-typeahead";
            return LabeledTypeaheadTemplate;
        }());
        Bootstrap.LabeledTypeaheadTemplate = LabeledTypeaheadTemplate;
        angular
            .module("allors")
            .component("bLabeledTypeahead", {
            controller: Bootstrap.TypeaheadController,
            templateUrl: LabeledTypeaheadTemplate.templateName,
            require: Bootstrap.FormController.require,
            bindings: Bootstrap.TypeaheadController.bindings
        });
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../allors.module.ts" />
var Allors;
(function (Allors) {
    var ContentEditable = /** @class */ (function () {
        function ContentEditable($sce) {
            this.$sce = $sce;
            this.restrict = "A";
            this.require = "?ngModel";
        }
        ContentEditable.prototype.link = function (scope, element, attrs, ngModel) {
            var _this = this;
            if (!ngModel)
                return;
            ngModel.$render = function () {
                element.html(_this.$sce.getTrustedHtml(ngModel.$viewValue) || "");
            };
            element.on("blur keyup change", function () {
                scope.$evalAsync(read);
            });
            function read() {
                var html = $.trim(element.html()).replace(/&nbsp;/g, "\u00a0");
                if (html === "" || html === "<br>") {
                    html = null;
                }
                ngModel.$setViewValue(html);
            }
        };
        ContentEditable.factory = function () {
            var directive = function ($sce) { return new ContentEditable($sce); };
            directive.$inject = ["$sce"];
            return directive;
        };
        return ContentEditable;
    }());
    angular.module("allors").directive("contenteditable", ContentEditable.factory());
})(Allors || (Allors = {}));
/// <reference path="../allors.module.ts" />
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
/// <reference path="../allors.module.ts" />
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
/// <reference path="../allors.module.ts" />
var Allors;
(function (Allors) {
    var Filters;
    (function (Filters) {
        var Humanize;
        (function (Humanize) {
            angular.module("allors").
                filter("humanize", function () { return Humanize.filter; });
            Humanize.filter = function (input) {
                if (input) {
                    return input
                        .replace(/([a-z\d])([A-Z])/g, "$1" + " " + "$2")
                        .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1" + " " + "$2");
                }
            };
        })(Humanize = Filters.Humanize || (Filters.Humanize = {}));
    })(Filters = Allors.Filters || (Allors.Filters = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var AccessControl = /** @class */ (function () {
        function AccessControl(id, version, permissionIds) {
            this.id = id;
            this.version = version;
            this.permissionIds = permissionIds;
        }
        return AccessControl;
    }());
    Allors.AccessControl = AccessControl;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Permission = /** @class */ (function () {
        function Permission(id, objectType, operandType, operation) {
            this.id = id;
            this.objectType = objectType;
            this.operandType = operandType;
            this.operation = operation;
        }
        return Permission;
    }());
    Allors.Permission = Permission;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var ids = Allors.Meta.ids;
        function serializeObject(roles) {
            if (roles) {
                return Object
                    .keys(roles)
                    .reduce(function (obj, v) {
                    obj[v] = serialize(roles[v]);
                    return obj;
                }, {});
            }
            return {};
        }
        Protocol.serializeObject = serializeObject;
        function serializeArray(roles) {
            if (roles) {
                return roles.map(function (v) { return serialize(v); });
            }
            return [];
        }
        Protocol.serializeArray = serializeArray;
        function serialize(role) {
            if (role === undefined || role === null) {
                return null;
            }
            if (typeof role === 'string') {
                return role;
            }
            if (role instanceof Date) {
                return role.toISOString();
            }
            return role.toString();
        }
        Protocol.serialize = serialize;
        function deserialize(value, objectType) {
            switch (objectType.id) {
                case ids.Boolean:
                    return value === 'true' ? true : false;
                case ids.Float:
                    return parseFloat(value);
                case ids.Integer:
                    return parseInt(value, 10);
            }
            return value;
        }
        Protocol.deserialize = deserialize;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
/// <reference path="../Workspace/Protocol/Serialization.ts" />
var Allors;
(function (Allors) {
    var Compressor = Allors.Protocol.Compressor;
    var deserialize = Allors.Protocol.deserialize;
    var WorkspaceObject = /** @class */ (function () {
        function WorkspaceObject(workspace) {
            this.workspace = workspace;
            this.cachedAccessControls = null;
            this.cachedDeniedPermissions = null;
            this.roleByRoleTypeId = new Map();
        }
        WorkspaceObject.prototype.new = function (id, objectType) {
            this.objectType = objectType;
            this.id = id;
            this.version = '0';
        };
        WorkspaceObject.prototype.sync = function (syncResponseObject, sortedAccessControlIdsDecompress, sortedDeniedPermissionIdsDecompress, metaDecompress) {
            var _this = this;
            this.id = syncResponseObject.i;
            this.version = syncResponseObject.v;
            this.objectType = metaDecompress(syncResponseObject.t);
            this.roleByRoleTypeId = new Map();
            if (syncResponseObject.r) {
                syncResponseObject.r.forEach(function (role) {
                    var roleTypeId = role.t;
                    var roleType = metaDecompress(roleTypeId);
                    var value = role.v;
                    if (roleType.objectType.isUnit) {
                        value = deserialize(value, roleType.objectType);
                    }
                    else {
                        if (roleType.isMany) {
                            value = value.split(Compressor.itemSeparator);
                        }
                    }
                    _this.roleByRoleTypeId.set(roleType.id, value);
                });
            }
            this.sortedAccessControlIds = sortedAccessControlIdsDecompress(syncResponseObject.a);
            this.sortedDeniedPermissionIds = sortedDeniedPermissionIdsDecompress(syncResponseObject.d);
        };
        WorkspaceObject.prototype.isPermitted = function (permission) {
            var e_2, _a;
            var _this = this;
            if (permission == null) {
                return false;
            }
            if (this.sortedAccessControlIds !== this.cachedSortedAccessControlIds) {
                this.cachedSortedAccessControlIds = this.sortedAccessControlIds;
                if (this.sortedAccessControlIds) {
                    this.cachedAccessControls = this.sortedAccessControlIds
                        .split(Compressor.itemSeparator)
                        .map(function (v) { return _this.workspace.accessControlById.get(v); });
                }
                else {
                    this.sortedAccessControlIds = null;
                }
            }
            if (this.sortedDeniedPermissionIds !== this.cachedSortedDeniedPermissionIds) {
                this.cachedSortedDeniedPermissionIds = this.sortedDeniedPermissionIds;
                if (this.sortedDeniedPermissionIds) {
                    this.cachedDeniedPermissions = new Set();
                    this.sortedDeniedPermissionIds
                        .split(Compressor.itemSeparator)
                        .forEach(function (v) { return _this.cachedDeniedPermissions.add(_this.workspace.permissionById.get(v)); });
                }
                else {
                    this.cachedDeniedPermissions = null;
                }
            }
            if (this.cachedDeniedPermissions && this.cachedDeniedPermissions.has(permission)) {
                return false;
            }
            if (this.cachedAccessControls) {
                try {
                    for (var _b = __values(this.cachedAccessControls), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var accessControl = _c.value;
                        if (accessControl.permissionIds.has(permission.id)) {
                            return true;
                        }
                    }
                }
                catch (e_2_1) { e_2 = { error: e_2_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_2) throw e_2.error; }
                }
            }
            return false;
        };
        WorkspaceObject.prototype.invalidate = function () {
            this.version = '';
        };
        return WorkspaceObject;
    }());
    Allors.WorkspaceObject = WorkspaceObject;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var PushRequest = /** @class */ (function () {
            function PushRequest(fields) {
                Object.assign(this, fields);
            }
            return PushRequest;
        }());
        Protocol.PushRequest = PushRequest;
        var PushRequestNewObject = /** @class */ (function () {
            function PushRequestNewObject() {
            }
            return PushRequestNewObject;
        }());
        Protocol.PushRequestNewObject = PushRequestNewObject;
        var PushRequestObject = /** @class */ (function () {
            function PushRequestObject() {
            }
            return PushRequestObject;
        }());
        Protocol.PushRequestObject = PushRequestObject;
        var PushRequestRole = /** @class */ (function () {
            function PushRequestRole() {
            }
            return PushRequestRole;
        }());
        Protocol.PushRequestRole = PushRequestRole;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
var __read = (this && this.__read) || function (o, n) {
    var m = typeof Symbol === "function" && o[Symbol.iterator];
    if (!m) return o;
    var i = m.call(o), r, ar = [], e;
    try {
        while ((n === void 0 || n-- > 0) && !(r = i.next()).done) ar.push(r.value);
    }
    catch (error) { e = { error: error }; }
    finally {
        try {
            if (r && !r.done && (m = i["return"])) m.call(i);
        }
        finally { if (e) throw e.error; }
    }
    return ar;
};
/// <reference path="./Protocol/Push/PushRequest.ts" />
var Allors;
(function (Allors) {
    var Operations = Allors.Protocol.Operations;
    var PushRequestObject = Allors.Protocol.PushRequestObject;
    var PushRequestNewObject = Allors.Protocol.PushRequestNewObject;
    var PushRequestRole = Allors.Protocol.PushRequestRole;
    var serialize = Allors.Protocol.serialize;
    var SessionObject = /** @class */ (function () {
        function SessionObject() {
        }
        Object.defineProperty(SessionObject.prototype, "isNew", {
            get: function () {
                return this.newId ? true : false;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SessionObject.prototype, "hasChanges", {
            get: function () {
                if (this.newId) {
                    return true;
                }
                return !!this.changedRoleByRoleType;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SessionObject.prototype, "id", {
            get: function () {
                return this.workspaceObject ? this.workspaceObject.id : this.newId;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(SessionObject.prototype, "version", {
            get: function () {
                return this.workspaceObject ? this.workspaceObject.version : undefined;
            },
            enumerable: true,
            configurable: true
        });
        SessionObject.prototype.canRead = function (roleType) {
            if (typeof roleType === 'string') {
                // TODO:
                return true;
            }
            return this.isPermited(roleType, Operations.Read);
        };
        SessionObject.prototype.canWrite = function (roleType) {
            if (typeof roleType === 'string') {
                // TODO:
                return true;
            }
            return this.isPermited(roleType, Operations.Write);
        };
        SessionObject.prototype.canExecute = function (methodType) {
            if (typeof methodType === 'string') {
                // TODO:
                return true;
            }
            return this.isPermited(methodType, Operations.Execute);
        };
        SessionObject.prototype.isPermited = function (operandType, operation) {
            if (this.roleByRoleType === undefined) {
                return undefined;
            }
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                var permission = this.session.workspace.permission(this.objectType, operandType, operation);
                return this.workspaceObject.isPermitted(permission);
            }
            return false;
        };
        SessionObject.prototype.method = function (methodType) {
            if (this.roleByRoleType === undefined) {
                return undefined;
            }
            return new Allors.Method(this, methodType);
        };
        SessionObject.prototype.get = function (roleType) {
            var _this = this;
            if (this.roleByRoleType === undefined) {
                return undefined;
            }
            var value = this.roleByRoleType.get(roleType);
            if (value === undefined) {
                if (this.newId === undefined) {
                    if (roleType.objectType.isUnit) {
                        value = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                        if (value === undefined) {
                            value = null;
                        }
                    }
                    else {
                        try {
                            if (roleType.isOne) {
                                var role = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                                value = role ? this.session.get(role) : null;
                            }
                            else {
                                var roles = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                                value = roles ? roles.map(function (role) {
                                    return _this.session.get(role);
                                }) : [];
                            }
                        }
                        catch (e) {
                            var stringValue = 'N/A';
                            try {
                                stringValue = this.toString();
                            }
                            catch (e2) {
                                throw new Error("Could not get role " + roleType.name + " from [objectType: " + this.objectType.name + ", id: " + this.id + "]");
                            }
                            throw new Error("Could not get role " + roleType.name + " from [objectType: " + this.objectType.name + ", id: " + this.id + ", value: '" + stringValue + "']");
                        }
                    }
                }
                else {
                    if (roleType.objectType.isComposite && roleType.isMany) {
                        value = [];
                    }
                    else {
                        value = null;
                    }
                }
                this.roleByRoleType.set(roleType, value);
            }
            return value;
        };
        SessionObject.prototype.getForAssociation = function (roleType) {
            var _this = this;
            if (this.roleByRoleType === undefined) {
                return undefined;
            }
            var value = this.roleByRoleType.get(roleType);
            if (value === undefined) {
                if (this.newId === undefined) {
                    if (roleType.objectType.isUnit) {
                        value = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                        if (value === undefined) {
                            value = null;
                        }
                    }
                    else {
                        if (roleType.isOne) {
                            var role = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                            value = role ? this.session.getForAssociation(role) : null;
                        }
                        else {
                            var roles = this.workspaceObject.roleByRoleTypeId.get(roleType.id);
                            value = roles ? roles.map(function (role) {
                                return _this.session.getForAssociation(role);
                            }) : [];
                        }
                    }
                }
                else {
                    if (roleType.objectType.isComposite && roleType.isMany) {
                        value = [];
                    }
                    else {
                        value = null;
                    }
                }
                this.roleByRoleType.set(roleType, value);
            }
            return value;
        };
        SessionObject.prototype.set = function (roleType, value) {
            this.assertExists();
            if (this.changedRoleByRoleType === undefined) {
                this.changedRoleByRoleType = new Map();
            }
            if (value === undefined) {
                value = null;
            }
            if (value === null) {
                if (roleType.objectType.isComposite && roleType.isMany) {
                    value = [];
                }
            }
            if (value === '') {
                if (roleType.objectType.isUnit) {
                    if (!roleType.objectType.isString) {
                        value = null;
                    }
                }
            }
            this.roleByRoleType.set(roleType, value);
            this.changedRoleByRoleType.set(roleType, value);
            this.session.hasChanges = true;
        };
        SessionObject.prototype.add = function (roleType, value) {
            if (!!value) {
                this.assertExists();
                var roles = this.get(roleType);
                if (roles.indexOf(value) < 0) {
                    roles.push(value);
                }
                this.set(roleType, roles);
                this.session.hasChanges = true;
            }
        };
        SessionObject.prototype.remove = function (roleType, value) {
            if (!!value) {
                this.assertExists();
                var roles = this.get(roleType);
                var index = roles.indexOf(value);
                if (index >= 0) {
                    roles.splice(index, 1);
                }
                this.set(roleType, roles);
                this.session.hasChanges = true;
            }
        };
        SessionObject.prototype.getAssociation = function (associationType) {
            this.assertExists();
            var associations = this.session.getAssociation(this, associationType);
            if (associationType.isOne) {
                return associations.length > 0 ? associations[0] : null;
            }
            return associations;
        };
        SessionObject.prototype.save = function (compressor) {
            if (this.changedRoleByRoleType !== undefined) {
                var data = new PushRequestObject();
                data.i = this.id;
                data.v = this.version;
                data.roles = this.saveRoles(compressor);
                return data;
            }
            return undefined;
        };
        SessionObject.prototype.saveNew = function (compressor) {
            this.assertExists();
            var data = new PushRequestNewObject();
            data.ni = this.newId;
            data.t = compressor.write(this.objectType.id);
            if (this.changedRoleByRoleType !== undefined) {
                data.roles = this.saveRoles(compressor);
            }
            return data;
        };
        SessionObject.prototype.reset = function () {
            if (this.newId) {
                delete this.newId;
                delete this.session;
                delete this.objectType;
                delete this.roleByRoleType;
            }
            else {
                this.workspaceObject = this.workspaceObject.workspace.get(this.id);
                this.roleByRoleType = new Map();
            }
            delete this.changedRoleByRoleType;
        };
        SessionObject.prototype.onDelete = function (deleted) {
            var e_3, _a;
            if (this.changedRoleByRoleType !== undefined) {
                try {
                    for (var _b = __values(this.changedRoleByRoleType), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var _d = __read(_c.value, 2), roleType = _d[0], value = _d[1];
                        if (!roleType.objectType.isUnit) {
                            if (roleType.isOne) {
                                var role = value;
                                if (role && role === deleted) {
                                    this.set(roleType, null);
                                }
                            }
                            else {
                                var roles = value;
                                if (roles && roles.indexOf(deleted) > -1) {
                                    this.remove(roleType, deleted);
                                }
                            }
                        }
                    }
                }
                catch (e_3_1) { e_3 = { error: e_3_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_3) throw e_3.error; }
                }
            }
        };
        SessionObject.prototype.init = function () {
            this.roleByRoleType = new Map();
        };
        SessionObject.prototype.assertExists = function () {
            if (this.roleByRoleType === undefined) {
                throw new Error('Object doesn\'t exist anymore.');
            }
        };
        SessionObject.prototype.saveRoles = function (compressor) {
            var e_4, _a;
            var saveRoles = new Array();
            if (this.changedRoleByRoleType) {
                var _loop_1 = function (roleType, value) {
                    var saveRole = new PushRequestRole();
                    saveRole.t = compressor.write(roleType.id);
                    var role = value;
                    if (roleType.objectType.isUnit) {
                        role = serialize(role);
                        saveRole.s = role;
                    }
                    else {
                        if (roleType.isOne) {
                            saveRole.s = role ? role.id || role.newId : null;
                        }
                        else {
                            var roleIds_1 = role.map(function (item) { return item.id || item.newId; });
                            if (this_1.newId) {
                                saveRole.a = roleIds_1;
                            }
                            else {
                                var originalRoleIds_1 = this_1.workspaceObject.roleByRoleTypeId.get(roleType.id);
                                if (!originalRoleIds_1) {
                                    saveRole.a = roleIds_1;
                                }
                                else {
                                    saveRole.a = roleIds_1.filter(function (v) { return originalRoleIds_1.indexOf(v) < 0; });
                                    saveRole.r = originalRoleIds_1.filter(function (v) { return roleIds_1.indexOf(v) < 0; });
                                }
                            }
                        }
                    }
                    saveRoles.push(saveRole);
                };
                var this_1 = this;
                try {
                    for (var _b = __values(this.changedRoleByRoleType), _c = _b.next(); !_c.done; _c = _b.next()) {
                        var _d = __read(_c.value, 2), roleType = _d[0], value = _d[1];
                        _loop_1(roleType, value);
                    }
                }
                catch (e_4_1) { e_4 = { error: e_4_1 }; }
                finally {
                    try {
                        if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                    }
                    finally { if (e_4) throw e_4.error; }
                }
                return saveRoles;
            }
        };
        return SessionObject;
    }());
    Allors.SessionObject = SessionObject;
})(Allors || (Allors = {}));
/// <reference path="./Protocol/Compressor.ts" />
/// <reference path="./WorkspaceObject.ts" />
/// <reference path="./SessionObject.ts" />
var Allors;
(function (Allors) {
    var PushRequest = Allors.Protocol.PushRequest;
    var Compressor = Allors.Protocol.Compressor;
    var Operations = Allors.Protocol.Operations;
    var Session = /** @class */ (function () {
        function Session(workspace) {
            this.workspace = workspace;
            this.hasChanges = false;
            this.existingSessionObjectById = new Map();
            this.newSessionObjectById = new Map();
            this.sessionObjectByIdByClass = new Map();
        }
        Session.prototype.get = function (id) {
            if (!id) {
                return undefined;
            }
            var sessionObject = this.existingSessionObjectById.get(id);
            if (sessionObject === undefined) {
                sessionObject = this.newSessionObjectById.get(id);
                if (sessionObject === undefined) {
                    var workspaceObject = this.workspace.get(id);
                    var constructor = this.workspace.constructorByObjectType.get(workspaceObject.objectType);
                    sessionObject = new constructor();
                    sessionObject.session = this;
                    sessionObject.workspaceObject = workspaceObject;
                    sessionObject.objectType = workspaceObject.objectType;
                    this.existingSessionObjectById.set(sessionObject.id, sessionObject);
                    this.addByObjectTypeId(sessionObject);
                }
            }
            return sessionObject;
        };
        Session.prototype.getForAssociation = function (id) {
            if (!id) {
                return undefined;
            }
            var sessionObject = this.existingSessionObjectById.get(id);
            if (sessionObject === undefined) {
                sessionObject = this.newSessionObjectById.get(id);
                if (sessionObject === undefined) {
                    var workspaceObject = this.workspace.getForAssociation(id);
                    if (workspaceObject) {
                        var constructor = this.workspace.constructorByObjectType.get(workspaceObject.objectType);
                        sessionObject = new constructor();
                        sessionObject.session = this;
                        sessionObject.workspaceObject = workspaceObject;
                        sessionObject.objectType = workspaceObject.objectType;
                        this.existingSessionObjectById.set(sessionObject.id, sessionObject);
                        this.addByObjectTypeId(sessionObject);
                    }
                }
            }
            return sessionObject;
        };
        Session.prototype.create = function (objectType) {
            if (typeof objectType === 'string') {
                objectType = this.workspace.metaPopulation.objectTypeByName.get(objectType);
            }
            var constructor = this.workspace.constructorByObjectType.get(objectType);
            var newSessionObject = new constructor();
            newSessionObject.session = this;
            newSessionObject.objectType = objectType;
            newSessionObject.newId = (--Session.idCounter).toString();
            this.newSessionObjectById.set(newSessionObject.newId, newSessionObject);
            this.addByObjectTypeId(newSessionObject);
            this.hasChanges = true;
            return newSessionObject;
        };
        Session.prototype.delete = function (object) {
            var e_5, _a, e_6, _b;
            if (!object.isNew) {
                throw new Error('Existing objects can not be deleted');
            }
            var newSessionObject = object;
            var newId = newSessionObject.newId;
            if (this.newSessionObjectById.has(newId)) {
                try {
                    for (var _c = __values(this.newSessionObjectById.values()), _d = _c.next(); !_d.done; _d = _c.next()) {
                        var sessionObject = _d.value;
                        sessionObject.onDelete(newSessionObject);
                    }
                }
                catch (e_5_1) { e_5 = { error: e_5_1 }; }
                finally {
                    try {
                        if (_d && !_d.done && (_a = _c.return)) _a.call(_c);
                    }
                    finally { if (e_5) throw e_5.error; }
                }
                try {
                    for (var _e = __values(this.existingSessionObjectById.values()), _f = _e.next(); !_f.done; _f = _e.next()) {
                        var sessionObject = _f.value;
                        sessionObject.onDelete(newSessionObject);
                    }
                }
                catch (e_6_1) { e_6 = { error: e_6_1 }; }
                finally {
                    try {
                        if (_f && !_f.done && (_b = _e.return)) _b.call(_e);
                    }
                    finally { if (e_6) throw e_6.error; }
                }
                var objectType = newSessionObject.objectType;
                newSessionObject.reset();
                this.newSessionObjectById.delete(newId);
                this.removeByObjectTypeId(objectType, newId);
            }
        };
        Session.prototype.reset = function () {
            var e_7, _a, e_8, _b;
            try {
                for (var _c = __values(this.newSessionObjectById.values()), _d = _c.next(); !_d.done; _d = _c.next()) {
                    var sessionObject = _d.value;
                    sessionObject.reset();
                }
            }
            catch (e_7_1) { e_7 = { error: e_7_1 }; }
            finally {
                try {
                    if (_d && !_d.done && (_a = _c.return)) _a.call(_c);
                }
                finally { if (e_7) throw e_7.error; }
            }
            try {
                for (var _e = __values(this.existingSessionObjectById.values()), _f = _e.next(); !_f.done; _f = _e.next()) {
                    var sessionObject = _f.value;
                    sessionObject.reset();
                }
            }
            catch (e_8_1) { e_8 = { error: e_8_1 }; }
            finally {
                try {
                    if (_f && !_f.done && (_b = _e.return)) _b.call(_e);
                }
                finally { if (e_8) throw e_8.error; }
            }
            this.hasChanges = false;
        };
        Session.prototype.pushRequest = function () {
            var compressor = new Compressor();
            return new PushRequest({
                newObjects: Array.from(this.newSessionObjectById.values()).map(function (v) { return v.saveNew(compressor); }).filter(function (v) { return v !== undefined; }),
                objects: Array.from(this.existingSessionObjectById.values()).map(function (v) { return v.save(compressor); }).filter(function (v) { return v !== undefined; }),
            });
        };
        Session.prototype.pushResponse = function (pushResponse) {
            var _this = this;
            if (pushResponse.newObjects) {
                pushResponse.newObjects.forEach(function (pushResponseNewObject) {
                    var newId = pushResponseNewObject.ni;
                    var id = pushResponseNewObject.i;
                    var sessionObject = _this.newSessionObjectById.get(newId);
                    delete sessionObject.newId;
                    sessionObject.workspaceObject = _this.workspace.new(id, sessionObject.objectType);
                    _this.newSessionObjectById.delete(newId);
                    _this.existingSessionObjectById.set(id, sessionObject);
                    _this.removeByObjectTypeId(sessionObject.objectType, newId);
                    _this.addByObjectTypeId(sessionObject);
                });
            }
            if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
                throw new Error('Not all new objects received ids');
            }
        };
        Session.prototype.getAssociation = function (object, associationType) {
            var _this = this;
            var associationClasses = associationType.objectType.classes;
            var roleType = associationType.relationType.roleType;
            var associationIds = new Set();
            var associations = [];
            associationClasses.forEach(function (associationClass) {
                var e_9, _a;
                _this.getAll(associationClass);
                var sessionObjectById = _this.sessionObjectByIdByClass.get(associationClass);
                if (sessionObjectById) {
                    try {
                        for (var _b = __values(sessionObjectById.values()), _c = _b.next(); !_c.done; _c = _b.next()) {
                            var association = _c.value;
                            if (!associationIds.has(association.id) && association.canRead(roleType)) {
                                if (roleType.isOne) {
                                    var role = association.getForAssociation(roleType);
                                    if (role && role.id === object.id) {
                                        associationIds.add(association.id);
                                        associations.push(association);
                                    }
                                }
                                else {
                                    var roles = association.getForAssociation(roleType);
                                    if (roles && roles.find(function (v) { return v === object; })) {
                                        associationIds.add(association.id);
                                        associations.push(association);
                                    }
                                }
                            }
                        }
                    }
                    catch (e_9_1) { e_9 = { error: e_9_1 }; }
                    finally {
                        try {
                            if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                        }
                        finally { if (e_9) throw e_9.error; }
                    }
                }
            });
            if (associationType.isOne && associations.length > 0) {
                return associations;
            }
            associationClasses.forEach(function (associationClass) {
                var e_10, _a;
                var workspaceObjects = _this.workspace.workspaceObjectsByClass.get(associationClass);
                try {
                    for (var workspaceObjects_1 = __values(workspaceObjects), workspaceObjects_1_1 = workspaceObjects_1.next(); !workspaceObjects_1_1.done; workspaceObjects_1_1 = workspaceObjects_1.next()) {
                        var workspaceObject = workspaceObjects_1_1.value;
                        if (!associationIds.has(workspaceObject.id)) {
                            var permission = _this.workspace.permission(workspaceObject.objectType, roleType, Operations.Read);
                            if (workspaceObject.isPermitted(permission)) {
                                if (roleType.isOne) {
                                    var role = workspaceObject.roleByRoleTypeId.get(roleType.id);
                                    if (object.id === role) {
                                        associations.push(_this.get(workspaceObject.id));
                                        break;
                                    }
                                }
                                else {
                                    var roles = workspaceObject.roleByRoleTypeId.get(roleType.id);
                                    if (roles && roles.indexOf(workspaceObject.id) > -1) {
                                        associationIds.add(workspaceObject.id);
                                        associations.push(_this.get(workspaceObject.id));
                                    }
                                }
                            }
                        }
                    }
                }
                catch (e_10_1) { e_10 = { error: e_10_1 }; }
                finally {
                    try {
                        if (workspaceObjects_1_1 && !workspaceObjects_1_1.done && (_a = workspaceObjects_1.return)) _a.call(workspaceObjects_1);
                    }
                    finally { if (e_10) throw e_10.error; }
                }
            });
            return associations;
        };
        Session.prototype.getAll = function (objectType) {
            var e_11, _a;
            var workspaceObjects = this.workspace.workspaceObjectsByClass.get(objectType);
            try {
                for (var workspaceObjects_2 = __values(workspaceObjects), workspaceObjects_2_1 = workspaceObjects_2.next(); !workspaceObjects_2_1.done; workspaceObjects_2_1 = workspaceObjects_2.next()) {
                    var workspaceObject = workspaceObjects_2_1.value;
                    this.get(workspaceObject.id);
                }
            }
            catch (e_11_1) { e_11 = { error: e_11_1 }; }
            finally {
                try {
                    if (workspaceObjects_2_1 && !workspaceObjects_2_1.done && (_a = workspaceObjects_2.return)) _a.call(workspaceObjects_2);
                }
                finally { if (e_11) throw e_11.error; }
            }
        };
        Session.prototype.addByObjectTypeId = function (sessionObject) {
            var sessionObjectById = this.sessionObjectByIdByClass.get(sessionObject.objectType);
            if (!sessionObjectById) {
                sessionObjectById = new Map();
                this.sessionObjectByIdByClass.set(sessionObject.objectType, sessionObjectById);
            }
            sessionObjectById.set(sessionObject.id, sessionObject);
        };
        Session.prototype.removeByObjectTypeId = function (objectType, id) {
            var sessionObjectById = this.sessionObjectByIdByClass.get(objectType);
            if (sessionObjectById) {
                sessionObjectById.delete(id);
            }
        };
        Session.idCounter = 0;
        return Session;
    }());
    Allors.Session = Session;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var SyncRequest = /** @class */ (function () {
            function SyncRequest(fields) {
                Object.assign(this, fields);
            }
            return SyncRequest;
        }());
        Protocol.SyncRequest = SyncRequest;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var SecurityRequest = /** @class */ (function () {
            function SecurityRequest(fields) {
                Object.assign(this, fields);
            }
            return SecurityRequest;
        }());
        Protocol.SecurityRequest = SecurityRequest;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var Operations;
        (function (Operations) {
            Operations[Operations["Read"] = 1] = "Read";
            Operations[Operations["Write"] = 2] = "Write";
            Operations[Operations["Execute"] = 4] = "Execute";
        })(Operations = Protocol.Operations || (Protocol.Operations = {}));
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        Protocol.createMetaDecompressor = function (decompressor, metaPopulation) { return function (compressed) {
            return metaPopulation.metaObjectById.get(decompressor.read(compressed, function (v) { }));
        }; };
        var Decompressor = /** @class */ (function () {
            function Decompressor() {
                this.valueByKey = new Map();
            }
            Decompressor.prototype.read = function (compressed, first) {
                if (compressed !== undefined && compressed !== null) {
                    if (compressed[0] === Protocol.Compressor.indexMarker) {
                        var secondIndexMarkerIndex = compressed.indexOf(Protocol.Compressor.indexMarker, 1);
                        var key = compressed.slice(1, secondIndexMarkerIndex);
                        var value = compressed.slice(secondIndexMarkerIndex + 1);
                        this.valueByKey.set(key, value);
                        first(value);
                        return value;
                    }
                    return this.valueByKey.get(compressed);
                }
                return null;
            };
            return Decompressor;
        }());
        Protocol.Decompressor = Decompressor;
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
var __spread = (this && this.__spread) || function () {
    for (var ar = [], i = 0; i < arguments.length; i++) ar = ar.concat(__read(arguments[i]));
    return ar;
};
/// <reference path="./Protocol/Sync/SyncRequest.ts" />
/// <reference path="./Protocol/Security/SecuriyRequest.ts" />
/// <reference path="./Protocol/Operations.ts" />
/// <reference path="./Protocol/Compressor.ts" />
/// <reference path="./Protocol/Decompressor.ts" />
var Allors;
(function (Allors) {
    var SyncRequest = Allors.Protocol.SyncRequest;
    var SecurityRequest = Allors.Protocol.SecurityRequest;
    var Operations = Allors.Protocol.Operations;
    var Decompressor = Allors.Protocol.Decompressor;
    var Compressor = Allors.Protocol.Compressor;
    var createMetaDecompressor = Allors.Protocol.createMetaDecompressor;
    var Workspace = /** @class */ (function () {
        function Workspace(metaPopulation) {
            var e_12, _a;
            var _this = this;
            this.metaPopulation = metaPopulation;
            this.constructorByObjectType = new Map();
            this.workspaceObjectById = new Map();
            this.workspaceObjectsByClass = new Map();
            try {
                for (var _b = __values(metaPopulation.classes), _c = _b.next(); !_c.done; _c = _b.next()) {
                    var objectType = _c.value;
                    this.workspaceObjectsByClass.set(objectType, new Set());
                }
            }
            catch (e_12_1) { e_12 = { error: e_12_1 }; }
            finally {
                try {
                    if (_c && !_c.done && (_a = _b.return)) _a.call(_b);
                }
                finally { if (e_12) throw e_12.error; }
            }
            this.accessControlById = new Map();
            this.permissionById = new Map();
            this.readPermissionByOperandTypeByClass = new Map();
            this.writePermissionByOperandTypeByClass = new Map();
            this.executePermissionByOperandTypeByClass = new Map();
            metaPopulation.classes.forEach(function (v) {
                _this.readPermissionByOperandTypeByClass.set(v, new Map());
                _this.writePermissionByOperandTypeByClass.set(v, new Map());
                _this.executePermissionByOperandTypeByClass.set(v, new Map());
            });
            this.metaPopulation.classes.forEach(function (objectType) {
                var DynamicClass = (function () {
                    return function () {
                        var prototype1 = Object.getPrototypeOf(this);
                        var prototype2 = Object.getPrototypeOf(prototype1);
                        prototype2.init.call(this);
                    };
                })();
                DynamicClass.prototype = Object.create(Allors.SessionObject.prototype);
                DynamicClass.prototype.constructor = DynamicClass;
                _this.constructorByObjectType.set(objectType, DynamicClass);
                var prototype = DynamicClass.prototype;
                objectType.roleTypes
                    .forEach(function (roleType) {
                    Object.defineProperty(prototype, 'CanRead' + roleType.name, {
                        get: function () {
                            return this.canRead(roleType);
                        },
                    });
                    if (roleType.isDerived) {
                        Object.defineProperty(prototype, roleType.name, {
                            get: function () {
                                return this.get(roleType);
                            },
                        });
                    }
                    else {
                        Object.defineProperty(prototype, 'CanWrite' + roleType.name, {
                            get: function () {
                                return this.canWrite(roleType);
                            },
                        });
                        Object.defineProperty(prototype, roleType.name, {
                            get: function () {
                                return this.get(roleType);
                            },
                            set: function (value) {
                                this.set(roleType, value);
                            },
                        });
                        if (roleType.isMany) {
                            prototype['Add' + roleType.singular] = function (value) {
                                return this.add(roleType, value);
                            };
                            prototype['Remove' + roleType.singular] = function (value) {
                                return this.remove(roleType, value);
                            };
                        }
                    }
                });
                objectType.associationTypes
                    .forEach(function (associationType) {
                    Object.defineProperty(prototype, associationType.name, {
                        get: function () {
                            return this.getAssociation(associationType);
                        },
                    });
                });
                objectType.methodTypes
                    .forEach(function (methodType) {
                    Object.defineProperty(prototype, 'CanExecute' + methodType.name, {
                        get: function () {
                            return this.canExecute(methodType);
                        },
                    });
                    Object.defineProperty(prototype, methodType.name, {
                        get: function () {
                            return this.method(methodType);
                        },
                    });
                });
            });
        }
        Workspace.prototype.get = function (id) {
            var workspaceObject = this.workspaceObjectById.get(id);
            if (workspaceObject === undefined) {
                throw new Error("Object with id " + id + " is not present.");
            }
            return workspaceObject;
        };
        Workspace.prototype.getForAssociation = function (id) {
            var workspaceObject = this.workspaceObjectById.get(id);
            return workspaceObject;
        };
        Workspace.prototype.diff = function (response) {
            var _this = this;
            var decompressor = new Decompressor();
            return new SyncRequest({
                objects: response.objects
                    .filter(function (syncRequestObject) {
                    var _a = __read(syncRequestObject, 4), id = _a[0], version = _a[1], compressedSortedAccessControlIds = _a[2], compressedSortedDeniedPermissionIds = _a[3];
                    var workspaceObject = _this.workspaceObjectById.get(id);
                    var sortedAccessControlIds = decompressor.read(compressedSortedAccessControlIds, function (v) { return v; });
                    var sortedDeniedPermissionIds = decompressor.read(compressedSortedDeniedPermissionIds, function (v) { return v; });
                    return (workspaceObject === undefined) ||
                        (workspaceObject === null) ||
                        (workspaceObject.version !== version) ||
                        (workspaceObject.sortedAccessControlIds !== sortedAccessControlIds) ||
                        (workspaceObject.sortedDeniedPermissionIds !== sortedDeniedPermissionIds);
                })
                    .map(function (syncRequestObject) {
                    return syncRequestObject[0];
                })
            });
        };
        Workspace.prototype.sync = function (syncResponse) {
            var _this = this;
            var decompressor = new Decompressor();
            var missingAccessControlIds = new Set();
            var missingPermissionIds = new Set();
            var metaDecompress = createMetaDecompressor(decompressor, this.metaPopulation);
            var sortedAccessControlIdsDecompress = function (compressed) {
                return decompressor.read(compressed, function (first) {
                    first
                        .split(Compressor.itemSeparator)
                        .forEach(function (v) {
                        if (!_this.accessControlById.has(v)) {
                            missingAccessControlIds.add(v);
                        }
                    });
                });
            };
            var sortedDeniedPermissionIdsDecompress = function (compressed) {
                return decompressor.read(compressed, function (first) {
                    first
                        .split(Compressor.itemSeparator)
                        .forEach(function (v) {
                        if (!_this.permissionById.has(v)) {
                            missingPermissionIds.add(v);
                        }
                    });
                });
            };
            if (syncResponse.objects) {
                syncResponse.objects
                    .forEach(function (v) {
                    var workspaceObject = new Allors.WorkspaceObject(_this);
                    workspaceObject.sync(v, sortedAccessControlIdsDecompress, sortedDeniedPermissionIdsDecompress, metaDecompress);
                    _this.add(workspaceObject);
                });
            }
            if (missingAccessControlIds.size > 0 || missingPermissionIds.size > 0) {
                return new SecurityRequest({
                    accessControls: __spread(missingAccessControlIds),
                    permissions: __spread(missingPermissionIds),
                });
            }
            return null;
        };
        Workspace.prototype.getOrCreate = function (map, key) {
            var value = map.get(key);
            if (!value) {
                value = new Map();
                map.set(key, value);
            }
            return value;
        };
        Workspace.prototype.security = function (securityResponse) {
            var _this = this;
            var decompressor = new Decompressor();
            var metaDecompress = createMetaDecompressor(decompressor, this.metaPopulation);
            if (securityResponse.permissions) {
                securityResponse.permissions.forEach(function (v) {
                    var id = v[0];
                    var objectType = metaDecompress(v[1]);
                    var operandType = metaDecompress(v[2]);
                    var operation = parseInt(v[3], 10);
                    var permission = _this.permissionById.get(id);
                    if (!permission) {
                        permission = new Allors.Permission(id, objectType, operandType, operation);
                        _this.permissionById.set(id, permission);
                    }
                    switch (operation) {
                        case Operations.Read:
                            _this.getOrCreate(_this.readPermissionByOperandTypeByClass, objectType).set(operandType, permission);
                            break;
                        case Operations.Write:
                            _this.getOrCreate(_this.writePermissionByOperandTypeByClass, objectType).set(operandType, permission);
                            break;
                        case Operations.Execute:
                            _this.getOrCreate(_this.executePermissionByOperandTypeByClass, objectType).set(operandType, permission);
                            break;
                    }
                });
            }
            var missingPermissionIds;
            if (securityResponse.accessControls) {
                securityResponse.accessControls.forEach(function (v) {
                    var id = v.i;
                    var version = v.v;
                    var permissionIds = new Set();
                    v.p.forEach(function (w) {
                        if (!_this.permissionById.has(w)) {
                            if (!missingPermissionIds) {
                                missingPermissionIds = new Set();
                            }
                            missingPermissionIds.add(w);
                        }
                        permissionIds.add(w);
                    });
                    var accessControl = new Allors.AccessControl(id, version, permissionIds);
                    _this.accessControlById.set(id, accessControl);
                });
            }
            if (missingPermissionIds) {
                return new SecurityRequest({
                    permissions: __spread(missingPermissionIds),
                });
            }
        };
        Workspace.prototype.new = function (id, objectType) {
            var workspaceObject = new Allors.WorkspaceObject(this);
            workspaceObject.new(id, objectType);
            this.add(workspaceObject);
            return workspaceObject;
        };
        Workspace.prototype.permission = function (objectType, operandType, operation) {
            switch (operation) {
                case Operations.Read:
                    return this.readPermissionByOperandTypeByClass.get(objectType).get(operandType);
                case Operations.Write:
                    return this.writePermissionByOperandTypeByClass.get(objectType).get(operandType);
                default:
                    return this.executePermissionByOperandTypeByClass.get(objectType).get(operandType);
            }
        };
        Workspace.prototype.add = function (workspaceObject) {
            this.workspaceObjectById.set(workspaceObject.id, workspaceObject);
            this.workspaceObjectsByClass.get(workspaceObject.objectType).add(workspaceObject);
        };
        return Workspace;
    }());
    Allors.Workspace = Workspace;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain_1) {
        var Domain = /** @class */ (function () {
            function Domain() {
                this.extensions = [];
            }
            Domain.prototype.extend = function (extension) {
                this.extensions.push(extension);
            };
            Domain.prototype.apply = function (workspace) {
                this.extensions.forEach(function (v) {
                    v(workspace);
                });
            };
            return Domain;
        }());
        Domain_1.domain = new Domain();
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var AssociationType = /** @class */ (function () {
            function AssociationType(relationType) {
                this.relationType = relationType;
                this.metaPopulation = relationType.metaPopulation;
            }
            Object.defineProperty(AssociationType.prototype, "isMany", {
                get: function () { return !this.isOne; },
                enumerable: true,
                configurable: true
            });
            return AssociationType;
        }());
        Meta.AssociationType = AssociationType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var MetaPopulation = /** @class */ (function () {
            function MetaPopulation(data) {
                var _this = this;
                this.metaObjectById = new Map();
                this.objectTypeByName = new Map();
                this.composites = [];
                this.objectTypes = [];
                // Units
                this.units = ['Binary', 'Boolean', 'DateTime', 'Decimal', 'Float', 'Integer', 'String', 'Unique']
                    .map(function (unitName) {
                    var objectType = new Meta.ObjectType(_this);
                    objectType.id = unitIdByTypeName[unitName];
                    objectType.name = unitName;
                    objectType.plural = unitName === 'Binary' ? 'Binaries' : unitName + 's';
                    objectType.kind = Meta.Kind.unit;
                    _this.objectTypes.push(objectType);
                    _this.objectTypeByName.set(objectType.name, objectType);
                    _this.metaObjectById.set(objectType.id, objectType);
                    return objectType;
                });
                // Interfaces
                this.interfaces = data.interfaces.map(function (interfaceData) {
                    var objectType = new Meta.ObjectType(_this);
                    objectType.id = interfaceData.id;
                    objectType.name = interfaceData.name;
                    objectType.plural = interfaceData.plural;
                    objectType.kind = Meta.Kind.interface;
                    _this.composites.push(objectType);
                    _this.objectTypes.push(objectType);
                    _this.objectTypeByName.set(objectType.name, objectType);
                    _this.metaObjectById.set(objectType.id, objectType);
                    return objectType;
                });
                // Classes
                this.classes = data.classes.map(function (classData) {
                    var objectType = new Meta.ObjectType(_this);
                    objectType.id = classData.id;
                    objectType.name = classData.name;
                    objectType.plural = classData.plural;
                    objectType.kind = Meta.Kind.class;
                    _this.composites.push(objectType);
                    _this.objectTypes.push(objectType);
                    _this.objectTypeByName.set(objectType.name, objectType);
                    _this.metaObjectById.set(objectType.id, objectType);
                    return objectType;
                });
                var dataObjectTypes = [].concat(data.interfaces).concat(data.classes);
                // Create Type Hierarchy
                dataObjectTypes.forEach(function (dataObjectType) {
                    var objectType = _this.metaObjectById.get(dataObjectType.id);
                    objectType.interfaces = dataObjectType.interfaceIds
                        ? dataObjectType.interfaceIds.map(function (v) { return _this.metaObjectById.get(v); })
                        : [];
                });
                // Derive subtypes
                this.composites
                    .forEach(function (composite) {
                    composite.interfaces.forEach(function (compositeInterface) {
                        compositeInterface.subtypes.push(composite);
                    });
                });
                // Derive classes
                this.classes
                    .forEach(function (objectType) {
                    objectType.classes.push(objectType);
                    objectType.interfaces.forEach(function (v) {
                        v.classes.push(objectType);
                    });
                });
                // MethodTypes
                this.methodTypes = data.methodTypes.map(function (methodTypeData) {
                    var methodType = new Meta.MethodType(_this);
                    methodType.id = methodTypeData.id;
                    methodType.objectType = _this.metaObjectById.get(methodTypeData.objectTypeId);
                    methodType.name = methodTypeData.name;
                    methodType.objectType.exclusiveMethodTypes.push(methodType);
                    methodType.objectType.methodTypes.push(methodType);
                    if (methodType.objectType.isInterface) {
                        _this.composites
                            .filter(function (v) { return v.interfaces.indexOf(methodType.objectType) > -1; })
                            .forEach(function (v) { return v.methodTypes.push(methodType); });
                    }
                    _this.metaObjectById.set(methodType.id, methodType);
                    return methodType;
                });
                // RelationTypes
                this.relationTypes = data.relationTypes.map(function (relationTypeData) {
                    var dataRoleType = relationTypeData.roleType;
                    var dataAssociationType = relationTypeData.associationType;
                    var relationType = new Meta.RelationType(_this);
                    relationType.id = relationTypeData.id;
                    var associationType = new Meta.AssociationType(relationType);
                    relationType.associationType = associationType;
                    associationType.id = dataAssociationType.id;
                    associationType.objectType = _this.metaObjectById.get(dataAssociationType.objectTypeId);
                    associationType.name = dataAssociationType.name;
                    associationType.isOne = dataAssociationType.isOne;
                    var roleTypeVirtual = new Meta.RoleTypeVirtual();
                    roleTypeVirtual.isRequired = dataRoleType.isRequired;
                    var roleType = new Meta.RoleType(relationType, roleTypeVirtual);
                    relationType.roleType = roleType;
                    roleType.id = dataRoleType.id;
                    roleType.objectType = _this.metaObjectById.get(dataRoleType.objectTypeId);
                    roleType.singular = dataRoleType.singular;
                    roleType.plural = dataRoleType.plural;
                    roleType.isOne = dataRoleType.isOne;
                    roleType.name = roleType.isOne ? roleType.singular : roleType.plural;
                    if (relationTypeData.concreteRoleTypes) {
                        relationTypeData.concreteRoleTypes.forEach(function (dataConcreteRoleType) {
                            var roleTypeOverride = new Meta.RoleTypeVirtual();
                            roleTypeOverride.isRequired = dataConcreteRoleType.isRequired;
                            var objectType = _this.metaObjectById.get(dataConcreteRoleType.objectTypeId);
                            roleType.overridesByClass.set(objectType, roleTypeOverride);
                        });
                    }
                    associationType.objectType.exclusiveRoleTypes.push(roleType);
                    roleType.objectType.exclusiveAssociationTypes.push(associationType);
                    associationType.objectType.roleTypes.push(roleType);
                    roleType.objectType.associationTypes.push(associationType);
                    if (associationType.objectType.isInterface) {
                        associationType.objectType.subtypes.forEach(function (subtype) { return subtype.roleTypes.push(roleType); });
                    }
                    if (roleType.objectType.isInterface) {
                        roleType.objectType.subtypes.forEach(function (subtype) { return subtype.associationTypes.push(associationType); });
                    }
                    _this.metaObjectById.set(relationType.id, relationType);
                    _this.metaObjectById.set(relationType.roleType.id, relationType.roleType);
                    _this.metaObjectById.set(relationType.associationType.id, relationType.associationType);
                    return relationType;
                });
                // Derive RoleType and AssociationType By Name
                this.composites
                    .forEach(function (objectType) {
                    objectType.roleTypes.forEach(function (v) { return objectType.roleTypeByName.set(v.name, v); });
                    objectType.associationTypes.forEach(function (v) { return objectType.associationTypeByName.set(v.name, v); });
                    objectType.methodTypes.forEach(function (v) { return objectType.methodTypeByName.set(v.name, v); });
                    objectType.subtypes.forEach(function (subtype) {
                        subtype.roleTypes
                            .filter(function (subtypeRoleType) { return !objectType.roleTypes.find(function (v) { return v.id === subtypeRoleType.id; }); })
                            .forEach(function (v) { return objectType.roleTypeByName.set(subtype.name + "_" + v.name, v); });
                        subtype.associationTypes
                            .filter(function (subtypeAssociationType) { return !objectType.exclusiveAssociationTypes.find(function (v) { return v === subtypeAssociationType; }); })
                            .forEach(function (v) { return objectType.associationTypeByName.set(subtype.name + "_" + v.name, v); });
                    });
                });
                // Assign Own Properties
                this.composites
                    .forEach(function (objectType) {
                    _this[objectType.name] = objectType;
                    objectType.roleTypes.forEach(function (roleType) { return objectType[roleType.name] = roleType; });
                    objectType.associationTypes.forEach(function (associationType) { return objectType[associationType.name] = associationType; });
                    objectType.methodTypes.forEach(function (methodTypes) { return objectType[methodTypes.name] = methodTypes; });
                });
                // Assign Properties from Interfaces
                this.composites
                    .forEach(function (objectType) {
                    objectType.interfaces.forEach(function (v) {
                        v.roleTypes.forEach(function (roleType) { return objectType[roleType.name] = roleType; });
                        v.associationTypes.forEach(function (associationType) { return objectType[associationType.name] = associationType; });
                        v.methodTypes.forEach(function (methodTypes) { return objectType[methodTypes.name] = methodTypes; });
                    });
                });
            }
            return MetaPopulation;
        }());
        Meta.MetaPopulation = MetaPopulation;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var MethodType = /** @class */ (function () {
            function MethodType(metaPopulation) {
                this.metaPopulation = metaPopulation;
            }
            return MethodType;
        }());
        Meta.MethodType = MethodType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var Kind;
        (function (Kind) {
            Kind[Kind["unit"] = 0] = "unit";
            Kind[Kind["class"] = 1] = "class";
            Kind[Kind["interface"] = 2] = "interface";
        })(Kind = Meta.Kind || (Meta.Kind = {}));
        var ObjectType = /** @class */ (function () {
            function ObjectType(metaPopulation) {
                this.metaPopulation = metaPopulation;
                this.subtypes = [];
                this.classes = [];
                this.exclusiveRoleTypes = [];
                this.exclusiveAssociationTypes = [];
                this.exclusiveMethodTypes = [];
                this.roleTypes = [];
                this.associationTypes = [];
                this.methodTypes = [];
                this.roleTypeByName = new Map();
                this.associationTypeByName = new Map();
                this.methodTypeByName = new Map();
            }
            Object.defineProperty(ObjectType.prototype, "isUnit", {
                get: function () {
                    return this.kind === Kind.unit;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isBinary", {
                get: function () {
                    return this.id === ids.Binary;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isBoolean", {
                get: function () {
                    return this.id === ids.Boolean;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isDateTime", {
                get: function () {
                    return this.id === ids.DateTime;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isDecimal", {
                get: function () {
                    return this.id === ids.Decimal;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isFloat", {
                get: function () {
                    return this.id === ids.Float;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isInteger", {
                get: function () {
                    return this.id === ids.Integer;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isString", {
                get: function () {
                    return this.id === ids.String;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isUnique", {
                get: function () {
                    return this.id === ids.Unique;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isComposite", {
                get: function () {
                    return this.kind !== Kind.unit;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isInterface", {
                get: function () {
                    return this.kind === Kind.interface;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isClass", {
                get: function () {
                    return this.kind === Kind.class;
                },
                enumerable: true,
                configurable: true
            });
            return ObjectType;
        }());
        Meta.ObjectType = ObjectType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var RelationType = /** @class */ (function () {
            function RelationType(metaPopulation) {
                this.metaPopulation = metaPopulation;
                this.metaPopulation = metaPopulation;
            }
            return RelationType;
        }());
        Meta.RelationType = RelationType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var RoleTypeVirtual = /** @class */ (function () {
            function RoleTypeVirtual() {
            }
            return RoleTypeVirtual;
        }());
        Meta.RoleTypeVirtual = RoleTypeVirtual;
        var RoleType = /** @class */ (function () {
            function RoleType(relationType, virtual) {
                this.relationType = relationType;
                this.virtual = virtual;
                relationType.roleType = this;
                this.metaPopulation = relationType.metaPopulation;
                this.overridesByClass = new Map();
            }
            RoleType.prototype.isRequired = function (objectType) {
                var override = this.overridesByClass.get(objectType);
                return override ? override.isRequired : this.virtual.isRequired;
            };
            Object.defineProperty(RoleType.prototype, "isMany", {
                get: function () { return !this.isOne; },
                enumerable: true,
                configurable: true
            });
            return RoleType;
        }());
        Meta.RoleType = RoleType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Protocol;
    (function (Protocol) {
        var ResponseType;
        (function (ResponseType) {
            ResponseType[ResponseType["Invoke"] = 0] = "Invoke";
            ResponseType[ResponseType["Pull"] = 1] = "Pull";
            ResponseType[ResponseType["Sync"] = 2] = "Sync";
            ResponseType[ResponseType["Push"] = 3] = "Push";
            ResponseType[ResponseType["Security"] = 4] = "Security";
        })(ResponseType = Protocol.ResponseType || (Protocol.ResponseType = {}));
    })(Protocol = Allors.Protocol || (Allors.Protocol = {}));
})(Allors || (Allors = {}));
/// <reference path="../Core/Angular/components/bootstrap/Form.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/ImageModal.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/CroppedImageModal.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Static.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/StaticEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Text.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/TextArea.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/TextAngular.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Enum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Select.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Typeahead.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Image.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/CroppedImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Radio.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/RadioButton.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/DatepickerPopup.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/Content.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/Label.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/Labeled.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/internal/LabeledInput.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledContent.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledStaticEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledStatic.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledText.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTextarea.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTextAngular.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledEnum.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledSelect.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledTypeahead.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledCroppedImage.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledRadio.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledRadioButton.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/labeled/LabeledDatepickerPopup.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/SelectOne.ts"/>
/// <reference path="../Core/Angular/components/bootstrap/SelectMany.ts"/>
var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        function registerTemplates($templateCache) {
            // Form
            // ----
            Bootstrap.FormTemplate.register($templateCache);
            // Object
            // ------
            // Shared
            // ------
            Bootstrap.ImageModalTemplate.register($templateCache);
            // Fields
            // ------
            Bootstrap.StaticTemplate.register($templateCache);
            Bootstrap.StaticEnumTemplate.register($templateCache);
            Bootstrap.TextTemplate.register($templateCache);
            Bootstrap.TextareaTemplate.register($templateCache);
            Bootstrap.TextAngularTemplate.register($templateCache);
            Bootstrap.EnumTemplate.register($templateCache);
            Bootstrap.SelectTemplate.register($templateCache);
            Bootstrap.TypeaheadTemplate.register($templateCache);
            Bootstrap.ImageTemplate.register($templateCache);
            Bootstrap.RadioTemplate.register($templateCache);
            Bootstrap.RadioButtonTemplate.register($templateCache);
            Bootstrap.DatepickerPopupTemplate.register($templateCache);
            Bootstrap.ContentTemplate.register($templateCache);
            // Field Groups
            // ------------
            // Internals
            Bootstrap.LabeledTemplate.register($templateCache);
            Bootstrap.LabelTemplate.register($templateCache);
            Bootstrap.LabeledInputTemplate.register($templateCache);
            // Controls
            Bootstrap.LabeledStaticEnumTemplate.register($templateCache);
            Bootstrap.LabeledStaticTemplate.register($templateCache);
            Bootstrap.LabeledTextTemplate.register($templateCache);
            Bootstrap.LabeledTextareaTemplate.register($templateCache);
            Bootstrap.LabeledTextAngularTemplate.register($templateCache);
            Bootstrap.LabeledEnumTemplate.register($templateCache);
            Bootstrap.LabeledSelectTemplate.register($templateCache);
            Bootstrap.LabeledTypeaheadTemplate.register($templateCache);
            Bootstrap.LabeledImageTemplate.register($templateCache);
            Bootstrap.LabeledRadioTemplate.register($templateCache);
            Bootstrap.LabeledRadioButtonTemplate.register($templateCache);
            Bootstrap.LabeledDatepickerPopupTemplate.register($templateCache);
            Bootstrap.LabeledContentTemplate.register($templateCache);
            // Model
            // -----
            Bootstrap.SelectOneTemplate.register($templateCache);
            Bootstrap.SelectManyTemplate.register($templateCache);
        }
        Bootstrap.registerTemplates = registerTemplates;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
/// <reference path="../../generated/domain/Person.g.ts" />
/// <reference path="../../Core/Workspace/Domain/Domain.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        Domain.domain.extend(function (workspace) {
            var m = workspace.metaPopulation;
            var person = workspace.constructorByObjectType.get(m.Person).prototype;
            person.toString = function () {
                return "Hello " + this.displayName;
            };
            Object.defineProperty(person, 'displayName', {
                get: function () {
                    if (this.FirstName || this.LastName) {
                        if (this.FirstName && this.LastName) {
                            return this.FirstName + ' ' + this.LastName;
                        }
                        else if (this.LastName) {
                            return this.LastName;
                        }
                        else {
                            return this.FirstName;
                        }
                    }
                    if (this.UserName) {
                        return this.UserName;
                    }
                    return 'N/A';
                },
            });
        });
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
var App;
(function (App) {
    angular.module("app", [
        "allors",
        // Angular
        "ngSanitize", "ngAnimate", "ngCookies", "ngMessages",
        // Angular UI
        "ui.router", "ui.bootstrap", "ui.select",
        // Third Party
        "pascalprecht.translate", "toastr", "angular-loading-bar", "blockUI"
    ]);
})(App || (App = {}));
/// <reference path="app.ts" />
var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["blockUIConfig"];
    function config(blockUIConfig) {
        blockUIConfig.message = "Processing, please wait.";
        blockUIConfig.blockBrowserNavigation = true;
        blockUIConfig.delay = 2000;
    }
})(App || (App = {}));
var App;
(function (App) {
    var app = angular.module("app");
    app.run(goTo);
    goTo.$inject = ["$cookies", "$state"];
    function goTo($cookies, $state) {
        var cookieName = "GoTo";
        var goTo = $cookies.get(cookieName);
        if (goTo) {
            $cookies.remove(cookieName);
            var parts = goTo.split(" ");
            var to = parts[0];
            var params = {
                id: parts[1]
            };
            $state.go(to, params);
        }
        else {
            $state.go("home");
        }
    }
})(App || (App = {}));
var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["cfpLoadingBarProvider"];
    function config(loadingBar) {
        loadingBar.includeSpinner = true;
    }
})(App || (App = {}));
var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$provide"];
    function config($provide) {
        var jsnlog = JL("AngularJS");
        $provide.decorator("$log", ["$delegate",
            function ($delegate) {
                return new Logger(jsnlog, $delegate);
            }
        ]);
        $provide.decorator("$exceptionHandler", ["$delegate",
            function ($delegate) { return function (exception, cause) {
                $delegate(exception, cause);
                // Either select Error page or alert
                window.location.href = "/Error";
                //window.alert(exception);
            }; }
        ]);
    }
    var Logger = /** @class */ (function () {
        function Logger(logger, originalLogger) {
            this.logger = logger;
            this.originalLogger = originalLogger;
        }
        Logger.prototype.debug = function (args) {
            this.logger.debug(args);
            if (this.originalLogger) {
                this.originalLogger.debug(args);
            }
        };
        Logger.prototype.info = function (args) {
            this.logger.info(args);
            if (this.originalLogger) {
                this.originalLogger.info(args);
            }
        };
        Logger.prototype.warn = function (args) {
            this.logger.warn(args);
            if (this.originalLogger) {
                this.originalLogger.warn(args);
            }
        };
        Logger.prototype.error = function (args) {
            this.logger.error(args);
            if (this.originalLogger) {
                this.originalLogger.error(args);
            }
        };
        Logger.prototype.log = function (args) {
            this.logger.trace(args);
            if (this.originalLogger) {
                this.originalLogger.log(args);
            }
        };
        return Logger;
    }());
})(App || (App = {}));
var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$stateProvider", "$urlRouterProvider", "$locationProvider"];
    function config($stateProvider, $urlRouterProvider) {
        $urlRouterProvider
            .otherwise("/");
        $stateProvider
            // Login
            .state("login", {
            url: "/login",
            templateUrl: "/app/login/login.html",
            controller: "loginController",
            controllerAs: "vm"
        })
            // General
            .state("home", {
            url: "/",
            templateUrl: "/app/pages/home/index.html",
            controller: "homeController",
            controllerAs: "vm"
        })
            .state("person", {
            url: "/person",
            templateUrl: "/app/pages/person/list.html",
            controller: "personListController",
            controllerAs: "vm"
        });
    }
})(App || (App = {}));
var App;
(function (App) {
    templates.$inject = ["$templateCache"];
    function templates($templateCache) {
        Allors.Bootstrap.registerTemplates($templateCache);
    }
    angular
        .module("app")
        .run(templates);
})(App || (App = {}));
var App;
(function (App) {
    var app = angular.module("app");
    app.config(config);
    config.$inject = ["$translateProvider"];
    function config($translateProvider) {
        $translateProvider.useSanitizeValueStrategy("sanitize");
        $translateProvider.useUrlLoader("/translation/translate");
        //$translateProvider.preferredLanguage("en");
        //$translateProvider.fallbackLanguage("en");
        //$translateProvider.registerAvailableLanguageKeys(["en"], {
        //    'en_*': "en",
        //});
        //$translateProvider.determinePreferredLanguage();
        $translateProvider.use('en');
    }
})(App || (App = {}));
var App;
(function (App) {
    var General;
    (function (General) {
        var Tasks;
        (function (Tasks) {
            var LoginController = /** @class */ (function () {
                function LoginController($http, $state, $scope, allors, $cookies) {
                    var _this = this;
                    this.$http = $http;
                    this.$state = $state;
                    this.$scope = $scope;
                    this.allors = allors;
                    var authenticationUrl = this.allors.baseUrl + "/TestAuthentication/Token";
                    var cookieName = "autoLogin";
                    var username = $cookies.get(cookieName);
                    if (username) {
                        $cookies.remove(cookieName);
                        this.$http
                            .post(authenticationUrl, { 'UserName': username, 'Password': "" })
                            .then(function (callbackArg) {
                            var response = callbackArg.data;
                            _this.allors.database.authorization = "Bearer " + response.token;
                            // Update main
                            var main = _this.$scope.$parent["controller"];
                            main.refresh();
                            cookieName = "AutoLoginGoTo";
                            var goTo = $cookies.get(cookieName);
                            if (goTo) {
                                $cookies.remove(cookieName);
                                var parts = goTo.split(" ");
                                var to = parts[0];
                                var params = {
                                    id: parts[1]
                                };
                                $state.go(to, params);
                            }
                            else {
                                $state.go("home");
                            }
                        })
                            .catch(function (e) {
                            window.alert("Error: " + e);
                        });
                    }
                }
                LoginController.prototype.login = function () {
                    var _this = this;
                    var authenticationUrl = this.allors.baseUrl + "/TestAuthentication/Token";
                    return this.$http
                        .post(authenticationUrl, { 'UserName': this.user, 'Password': this.password })
                        .then(function (callbackArg) {
                        var response = callbackArg.data;
                        if (!response.authenticated) {
                            _this.error = "Not authorized";
                            return;
                        }
                        _this.allors.database.authorization = "Bearer " + response.token;
                        // Update main
                        var main = _this.$scope.$parent["controller"];
                        main.refresh();
                        // Go to home
                        _this.$state.go("home");
                    })
                        .catch(function (error) {
                        _this.error = error.message;
                    });
                };
                LoginController.$inject = ["$http", "$state", "$scope", "allorsService", "$cookies"];
                return LoginController;
            }());
            angular
                .module("app")
                .controller("loginController", LoginController);
        })(Tasks = General.Tasks || (General.Tasks = {}));
    })(General = App.General || (App.General = {}));
})(App || (App = {}));
/// <reference path="../../Core/Angular/Scope.ts" />
var App;
(function (App) {
    var Page = /** @class */ (function (_super) {
        __extends(Page, _super);
        function Page(name, allors, $scope) {
            var _this = _super.call(this, name, allors.database, allors.workspace, allors.$rootScope, $scope, allors.$q, allors.$log) || this;
            _this.toastr = allors.toastr;
            return _this;
        }
        Page.prototype.getPersonTypeAhead = function (criteria) {
            return this.queryResults("PersonTypeAhead", { criteria: criteria });
        };
        Page.prototype.save = function () {
            var _this = this;
            return this.$q(function (resolve, reject) {
                _super.prototype.save.call(_this)
                    .then(function (saveResponse) {
                    _this.toastr.info("Successfully saved.");
                    resolve(saveResponse);
                })
                    .catch(function (e) {
                    if (e.responseType) {
                        _this.errorResponse(e);
                        resolve(e);
                    }
                    else {
                        reject(e);
                    }
                });
            });
        };
        Page.prototype.invoke = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var superInvoke;
                if (methodOrService instanceof Allors.Method) {
                    superInvoke = _super.prototype.invoke.call(_this, methodOrService);
                }
                else {
                    superInvoke = _super.prototype.invoke.call(_this, methodOrService, args);
                }
                superInvoke.then(function (invokeResponse) {
                    _this.toastr.info("Successfully executed.");
                    resolve(invokeResponse);
                })
                    .catch(function (e) {
                    if (e.responseType) {
                        _this.errorResponse(e);
                        resolve(e);
                    }
                    else {
                        reject(e);
                    }
                });
            });
        };
        Page.prototype.saveAndInvoke = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var superSaveAndInvoke;
                if (methodOrService instanceof Allors.Method) {
                    superSaveAndInvoke = _super.prototype.saveAndInvoke.call(_this, methodOrService);
                }
                else {
                    superSaveAndInvoke = _super.prototype.saveAndInvoke.call(_this, methodOrService, args);
                }
                superSaveAndInvoke.then(function (invokeResponse) {
                    _this.toastr.info("Successfully executed.");
                    resolve(invokeResponse);
                })
                    .catch(function (e) {
                    if (e.responseType) {
                        _this.errorResponse(e);
                        resolve(e);
                    }
                    else {
                        reject(e);
                    }
                });
            });
        };
        Page.prototype.errorResponse = function (error) {
            var title;
            var message = "<div>";
            if (error.accessErrors && error.accessErrors.length > 0) {
                title = "Access Error";
                message += "<p>You do not have the required rights.</p>";
            }
            else if ((error.versionErrors && error.versionErrors.length > 0) ||
                (error.missingErrors && error.missingErrors.length > 0)) {
                title = "Concurrency Error";
                message += "<p>Modifications were detected since last access.</p>";
            }
            else if (error.derivationErrors && error.derivationErrors.length > 0) {
                title = "Derivation Errors";
                message += "<ul>";
                error.derivationErrors.map(function (derivationError) {
                    message += "<li>" + derivationError.m + "</li>";
                });
                message += "</ul>";
            }
            else {
                title = "General Error";
                message += "<p>" + error.errorMessage + "</p>";
            }
            message += "<div>";
            this.toastr.error(message, title, {
                allowHtml: true,
                closeButton: true,
                timeOut: 0
            });
        };
        return Page;
    }(Allors.Scope));
    App.Page = Page;
})(App || (App = {}));
var App;
(function (App) {
    var General;
    (function (General) {
        var Home;
        (function (Home) {
            var IndexController = /** @class */ (function () {
                function IndexController($log, $state) {
                    this.$log = $log;
                    this.$state = $state;
                }
                IndexController.$inject = ["$log", "$state"];
                return IndexController;
            }());
            angular
                .module("app")
                .controller("homeController", IndexController);
        })(Home = General.Home || (General.Home = {}));
    })(General = App.General || (App.General = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Person;
        (function (Person) {
            var ListController = /** @class */ (function () {
                function ListController($log, $state) {
                    this.$log = $log;
                    this.$state = $state;
                }
                ListController.$inject = ["$log", "$state"];
                return ListController;
            }());
            angular
                .module("app")
                .controller("personListController", ListController);
        })(Person = Pages.Person || (Pages.Person = {}));
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
var App;
(function (App) {
    var Services;
    (function (Services) {
        var MetaPopulation = Allors.Meta.MetaPopulation;
        var Workspace = Allors.Workspace;
        var Domain = Allors.Domain;
        var AllorsService = /** @class */ (function () {
            function AllorsService($log, $http, $q, $rootScope, $uibModal, toastr) {
                this.$log = $log;
                this.$http = $http;
                this.$q = $q;
                this.$rootScope = $rootScope;
                this.$uibModal = $uibModal;
                this.toastr = toastr;
                this.baseUrl = "";
                var postfix = "/Pull";
                var metaPopulation = new MetaPopulation(Data.data);
                var workspace = new Workspace(metaPopulation);
                Domain.domain.apply(workspace);
                this.database = new Allors.Database(this.$http, this.$q, postfix, this.baseUrl);
                this.workspace = new Allors.Workspace(metaPopulation);
            }
            AllorsService.$inject = ["$log", "$http", "$q", "$rootScope", "$uibModal", "toastr"];
            return AllorsService;
        }());
        Services.AllorsService = AllorsService;
        angular.module("app")
            .service("allorsService", AllorsService);
    })(Services = App.Services || (App.Services = {}));
})(App || (App = {}));
/// <reference path="../pages/Page.ts" />
var App;
(function (App) {
    var Shell;
    (function (Shell) {
        var MainController = /** @class */ (function (_super) {
            __extends(MainController, _super);
            function MainController(allors, $scope, $state) {
                var _this = _super.call(this, "Main", allors, $scope) || this;
                _this.$state = $state;
                _this.refresh();
                // Login
                $scope["controller"] = _this;
                return _this;
            }
            MainController.prototype.goto = function (route) {
                this.$state.go(route);
            };
            MainController.prototype.refresh = function () {
                var _this = this;
                return this.load()
                    .then(function () {
                    _this.user = _this.objects["user"];
                })
                    .catch(function (err) {
                    if (err.status && err.status === 401) {
                        _this.$state.go("login");
                    }
                });
            };
            MainController.$inject = ["allorsService", "$scope", "$state"];
            return MainController;
        }(App.Page));
        angular
            .module("app")
            .controller("mainController", MainController);
    })(Shell = App.Shell || (App.Shell = {}));
})(App || (App = {}));
//# sourceMappingURL=tsc.js.map