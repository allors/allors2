var Allors;
(function (Allors) {
    angular.module("allors", []);
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Method = /** @class */ (function () {
        function Method(object, name) {
            this.object = object;
            this.name = name;
        }
        return Method;
    }());
    Allors.Method = Method;
})(Allors || (Allors = {}));
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
                        var requireLoadIds = _this.workspace.diff(response);
                        if (requireLoadIds.objects.length > 0) {
                            _this.database.sync(requireLoadIds)
                                .then(function (loadResponse) {
                                _this.workspace.sync(loadResponse);
                                _this.update(response);
                                _this.session.reset();
                                resolve();
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
                                var result = new Allors.Result(_this.session, response_1);
                                resolve(result);
                            })
                                .catch(function (e2) { return reject(e2); });
                        }
                        else {
                            var result = new Allors.Result(_this.session, response_1);
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
                        try {
                            _this.session.pushResponse(pushResponse);
                            var syncRequest = new Allors.Data.SyncRequest();
                            syncRequest.objects = pushRequest_1.objects.map(function (v) { return v.i; });
                            if (pushResponse.newObjects) {
                                for (var _i = 0, _a = pushResponse.newObjects; _i < _a.length; _i++) {
                                    var newObject = _a[_i];
                                    syncRequest.objects.push(newObject.i);
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
/// <reference path="allors.module.ts" />
/// <reference path="../Workspace/Method.ts" />
var Allors;
(function (Allors) {
    var Database = /** @class */ (function () {
        function Database($http, $q, prefix, postfix, baseUrl) {
            this.$http = $http;
            this.$q = $q;
            this.prefix = prefix;
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
                var serviceName = "" + _this.baseUrl + _this.prefix + "Sync";
                _this.$http.post(serviceName, syncRequest, _this.headers)
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
                var serviceName = "" + _this.baseUrl + _this.prefix + "Push";
                _this.$http.post(serviceName, pushRequest, _this.headers)
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
            return this.$q(function (resolve, reject) {
                var invokeRequest = {
                    i: methods.map(function (v) {
                        return {
                            i: v.object.id,
                            m: v.name,
                            v: v.object.version,
                        };
                    }),
                    o: options
                };
                var serviceName = "" + _this.baseUrl + _this.prefix + "Invoke";
                _this.$http.post(serviceName, invokeRequest, _this.headers)
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
            });
        };
        Database.prototype.invokeService = function (methodOrService, args) {
            var _this = this;
            return this.$q(function (resolve, reject) {
                var serviceName = _this.baseUrl + methodOrService + _this.postfix;
                _this.$http.post(serviceName, args, _this.headers)
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
    var Result = /** @class */ (function () {
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
                        if (this.roleType.objectType === "Integer" ||
                            this.roleType.objectType === "Decimal" ||
                            this.roleType.objectType === "Float") {
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
    var Session = /** @class */ (function () {
        function Session(workspace) {
            this.sessionObjectById = {};
            this.newSessionObjectById = {};
            this.workspace = workspace;
            this.hasChanges = false;
        }
        Session.prototype.get = function (id) {
            if (id === undefined || id === null) {
                return undefined;
            }
            var sessionObject = this.sessionObjectById[id];
            if (sessionObject === undefined) {
                sessionObject = this.newSessionObjectById[id];
                if (sessionObject === undefined) {
                    var workspaceObject = this.workspace.get(id);
                    var type = Allors.Domain[workspaceObject.objectType.name];
                    sessionObject = new type();
                    sessionObject.session = this;
                    sessionObject.workspaceObject = workspaceObject;
                    sessionObject.objectType = workspaceObject.objectType;
                    this.sessionObjectById[sessionObject.id] = sessionObject;
                }
            }
            return sessionObject;
        };
        Session.prototype.create = function (objectTypeName) {
            var type = Allors.Domain[objectTypeName];
            var newSessionObject = new type();
            newSessionObject.session = this;
            newSessionObject.objectType = this.workspace.objectTypeByName[objectTypeName];
            newSessionObject.newId = (--Session.idCounter).toString();
            this.newSessionObjectById[newSessionObject.newId] = newSessionObject;
            this.hasChanges = true;
            return newSessionObject;
        };
        Session.prototype.reset = function () {
            _.forEach(this.newSessionObjectById, function (v) {
                v.reset();
            });
            this.newSessionObjectById = {};
            _.forEach(this.sessionObjectById, function (v) {
                v.reset();
            });
            this.hasChanges = false;
        };
        Session.prototype.pushRequest = function () {
            var data = new Allors.Data.PushRequest();
            data.newObjects = [];
            data.objects = [];
            if (this.newSessionObjectById) {
                _.forEach(this.newSessionObjectById, function (newSessionObject) {
                    var objectData = newSessionObject.saveNew();
                    if (objectData !== undefined) {
                        data.newObjects.push(objectData);
                    }
                });
            }
            _.forEach(this.sessionObjectById, function (sessionObject) {
                var objectData = sessionObject.save();
                if (objectData !== undefined) {
                    data.objects.push(objectData);
                }
            });
            return data;
        };
        Session.prototype.pushResponse = function (pushResponse) {
            var _this = this;
            if (pushResponse.newObjects) {
                _.forEach(pushResponse.newObjects, function (pushResponseNewObject) {
                    var newId = pushResponseNewObject.ni;
                    var id = pushResponseNewObject.i;
                    var newSessionObject = _this.newSessionObjectById[newId];
                    var syncResponse = {
                        responseType: Allors.Data.ResponseType.Sync,
                        userSecurityHash: "#",
                        objects: [
                            {
                                i: id,
                                v: "",
                                t: newSessionObject.objectType.name,
                                roles: [],
                                methods: []
                            }
                        ]
                    };
                    delete (_this.newSessionObjectById[newId]);
                    delete (newSessionObject.newId);
                    _this.workspace.sync(syncResponse);
                    var workspaceObject = _this.workspace.get(id);
                    newSessionObject.workspaceObject = workspaceObject;
                    _this.sessionObjectById[id] = newSessionObject;
                });
            }
            if (Object.getOwnPropertyNames(this.newSessionObjectById).length !== 0) {
                throw new Error("Not all new objects received ids");
            }
        };
        Session.idCounter = 0;
        return Session;
    }());
    Allors.Session = Session;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var SessionObject = /** @class */ (function () {
        function SessionObject() {
            this.roleByRoleTypeName = {};
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
                return this.changedRoleByRoleTypeName !== undefined;
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
        SessionObject.prototype.canRead = function (roleTypeName) {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canRead(roleTypeName);
            }
            return undefined;
        };
        SessionObject.prototype.canWrite = function (roleTypeName) {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canWrite(roleTypeName);
            }
            return undefined;
        };
        SessionObject.prototype.canExecute = function (methodName) {
            if (this.newId) {
                return true;
            }
            else if (this.workspaceObject) {
                return this.workspaceObject.canExecute(methodName);
            }
            return undefined;
        };
        SessionObject.prototype.get = function (roleTypeName) {
            var _this = this;
            if (!this.roleByRoleTypeName) {
                return undefined;
            }
            var value = this.roleByRoleTypeName[roleTypeName];
            if (value === undefined) {
                var roleType = this.objectType.roleTypeByName[roleTypeName];
                if (this.newId === undefined) {
                    if (roleType.isUnit) {
                        value = this.workspaceObject.roles[roleTypeName];
                        if (value === undefined) {
                            value = null;
                        }
                        ;
                    }
                    else {
                        try {
                            if (roleType.isOne) {
                                var role = this.workspaceObject.roles[roleTypeName];
                                value = role ? this.session.get(role) : null;
                            }
                            else {
                                var roles = this.workspaceObject.roles[roleTypeName];
                                value = roles ? roles.map(function (role) {
                                    return _this.session.get(role);
                                }) : [];
                            }
                        }
                        catch (e) {
                            var value_1 = "N/A";
                            try {
                                value_1 = this.toString();
                            }
                            catch (e2) { }
                            ;
                            throw new Error("Could not get role " + roleTypeName + " from [objectType: " + this.objectType.name + ", id: " + this.id + ", value: '" + value_1 + "']");
                        }
                    }
                }
                else {
                    if (roleType.isComposite && roleType.isMany) {
                        value = [];
                    }
                    else {
                        value = null;
                    }
                }
                this.roleByRoleTypeName[roleTypeName] = value;
            }
            return value;
        };
        SessionObject.prototype.set = function (roleTypeName, value) {
            this.assertExists();
            if (this.changedRoleByRoleTypeName === undefined) {
                this.changedRoleByRoleTypeName = {};
            }
            if (value === undefined) {
                value = null;
            }
            if (value === null) {
                var roleType = this.objectType.roleTypeByName[roleTypeName];
                if (roleType.isComposite && roleType.isMany) {
                    value = [];
                }
            }
            this.roleByRoleTypeName[roleTypeName] = value;
            this.changedRoleByRoleTypeName[roleTypeName] = value;
            this.session.hasChanges = true;
        };
        SessionObject.prototype.add = function (roleTypeName, value) {
            this.assertExists();
            var roles = this.get(roleTypeName);
            if (roles.indexOf(value) < 0) {
                roles.push(value);
            }
            this.set(roleTypeName, roles);
            this.session.hasChanges = true;
        };
        SessionObject.prototype.remove = function (roleTypeName, value) {
            this.assertExists();
            var roles = this.get(roleTypeName);
            var index = roles.indexOf(value);
            if (index >= 0) {
                roles.splice(index, 1);
            }
            this.set(roleTypeName, roles);
            this.session.hasChanges = true;
        };
        SessionObject.prototype.save = function () {
            if (this.changedRoleByRoleTypeName !== undefined) {
                var data = new Allors.Data.PushRequestObject();
                data.i = this.id;
                data.v = this.version;
                data.roles = this.saveRoles();
                return data;
            }
            return undefined;
        };
        SessionObject.prototype.saveNew = function () {
            this.assertExists();
            var data = new Allors.Data.PushRequestNewObject();
            data.ni = this.newId;
            data.t = this.objectType.name;
            if (this.changedRoleByRoleTypeName !== undefined) {
                data.roles = this.saveRoles();
            }
            return data;
        };
        SessionObject.prototype.reset = function () {
            if (this.newId) {
                delete this.newId;
                delete this.session;
                delete this.objectType;
                delete this.roleByRoleTypeName;
            }
            else {
                this.workspaceObject = this.workspaceObject.workspace.get(this.id);
                this.roleByRoleTypeName = {};
            }
            delete this.changedRoleByRoleTypeName;
        };
        SessionObject.prototype.assertExists = function () {
            if (!this.roleByRoleTypeName) {
                throw "Object doesn't exist anymore.";
            }
        };
        SessionObject.prototype.saveRoles = function () {
            var _this = this;
            var saveRoles = new Array();
            _.forEach(this.changedRoleByRoleTypeName, function (role, roleTypeName) {
                var roleType = _this.objectType.roleTypeByName[roleTypeName];
                var saveRole = new Allors.Data.PushRequestRole;
                saveRole.t = roleType.name;
                if (roleType.isUnit) {
                    saveRole.s = role;
                }
                else {
                    if (roleType.isOne) {
                        saveRole.s = role ? role.id || role.newId : null;
                    }
                    else {
                        var roleIds = role.map(function (item) { return item.id || item.newId; });
                        if (_this.newId) {
                            saveRole.a = roleIds;
                        }
                        else {
                            var originalRoleIds = _this.workspaceObject.roles[roleTypeName];
                            if (!originalRoleIds) {
                                saveRole.a = roleIds;
                            }
                            else {
                                saveRole.a = _.difference(roleIds, originalRoleIds);
                                saveRole.r = _.difference(originalRoleIds, roleIds);
                            }
                        }
                    }
                }
                saveRoles.push(saveRole);
            });
            return saveRoles;
        };
        return SessionObject;
    }());
    Allors.SessionObject = SessionObject;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Workspace = /** @class */ (function () {
        function Workspace(metaPopulationData) {
            var _this = this;
            this.objectTypeByName = {};
            this.workspaceObjectById = {};
            _.forEach(metaPopulationData.classes, function (classData) {
                var objectType = new Allors.Meta.ObjectType(classData);
                _this.objectTypeByName[objectType.name] = objectType;
            });
        }
        Workspace.prototype.diff = function (response) {
            var _this = this;
            var userSecurityHash = response.userSecurityHash;
            var requireLoadIdsWithVersion = _.filter(response.objects, function (idAndVersion) {
                var id = idAndVersion[0], version = idAndVersion[1];
                var workspaceObject = _this.workspaceObjectById[id];
                return (workspaceObject === undefined) || (workspaceObject === null) || (workspaceObject.version !== version) || (workspaceObject.userSecurityHash !== userSecurityHash);
            });
            var requireLoadIds = new Allors.Data.SyncRequest();
            requireLoadIds.objects = _.map(requireLoadIdsWithVersion, function (idWithVersion) {
                return idWithVersion[0];
            });
            return requireLoadIds;
        };
        Workspace.prototype.sync = function (syncResponse) {
            var _this = this;
            _.forEach(syncResponse.objects, function (objectData) {
                var workspaceObject = new Allors.WorkspaceObject(_this, syncResponse, objectData);
                _this.workspaceObjectById[workspaceObject.id] = workspaceObject;
            });
        };
        Workspace.prototype.get = function (id) {
            var workspaceObject = this.workspaceObjectById[id];
            if (workspaceObject === undefined) {
                throw new Error("Object with id " + id + " is not present.");
            }
            return workspaceObject;
        };
        return Workspace;
    }());
    Allors.Workspace = Workspace;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var WorkspaceObject = /** @class */ (function () {
        function WorkspaceObject(workspace, loadResponse, loadObject) {
            var _this = this;
            this.workspace = workspace;
            this.i = loadObject.i;
            this.v = loadObject.v;
            this.u = loadResponse.userSecurityHash;
            this.t = loadObject.t;
            this.roles = {};
            this.methods = {};
            var objectType = this.workspace.objectTypeByName[this.t];
            _.forEach(loadObject.roles, function (role) {
                var name = role[0], access = role[1];
                var canRead = access.indexOf("r") !== -1;
                var canWrite = access.indexOf("w") !== -1;
                _this.roles["CanRead" + name] = canRead;
                _this.roles["CanWrite" + name] = canWrite;
                if (canRead) {
                    var roleType = objectType.roleTypeByName[name];
                    var value = role[2];
                    if (value && roleType.isUnit && roleType.objectType === "DateTime") {
                        value = new Date(value);
                    }
                    _this.roles[name] = value;
                }
            });
            _.forEach(loadObject.methods, function (method) {
                var name = method[0], access = method[1];
                var canExecute = access.indexOf("x") !== -1;
                _this.methods["CanExecute" + name] = canExecute;
            });
        }
        Object.defineProperty(WorkspaceObject.prototype, "id", {
            get: function () {
                return this.i;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "version", {
            get: function () {
                return this.v;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "userSecurityHash", {
            get: function () {
                return this.u;
            },
            enumerable: true,
            configurable: true
        });
        Object.defineProperty(WorkspaceObject.prototype, "objectType", {
            get: function () {
                return this.workspace.objectTypeByName[this.t];
            },
            enumerable: true,
            configurable: true
        });
        WorkspaceObject.prototype.canRead = function (roleTypeName) {
            return this.roles["CanRead" + roleTypeName];
        };
        WorkspaceObject.prototype.canWrite = function (roleTypeName) {
            return this.roles["CanWrite" + roleTypeName];
        };
        WorkspaceObject.prototype.canExecute = function (methodName) {
            return this.methods["CanExecute" + methodName];
        };
        return WorkspaceObject;
    }());
    Allors.WorkspaceObject = WorkspaceObject;
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var RoleType = /** @class */ (function () {
            function RoleType() {
            }
            return RoleType;
        }());
        Data.RoleType = RoleType;
        var MethodType = /** @class */ (function () {
            function MethodType() {
            }
            return MethodType;
        }());
        Data.MethodType = MethodType;
        var ObjectType = /** @class */ (function () {
            function ObjectType() {
            }
            return ObjectType;
        }());
        Data.ObjectType = ObjectType;
        var MetaPopulation = /** @class */ (function () {
            function MetaPopulation() {
            }
            return MetaPopulation;
        }());
        Data.MetaPopulation = MetaPopulation;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var PushRequestRole = /** @class */ (function () {
            function PushRequestRole() {
            }
            return PushRequestRole;
        }());
        Data.PushRequestRole = PushRequestRole;
        var PushRequestObject = /** @class */ (function () {
            function PushRequestObject() {
            }
            return PushRequestObject;
        }());
        Data.PushRequestObject = PushRequestObject;
        var PushRequestNewObject = /** @class */ (function () {
            function PushRequestNewObject() {
            }
            return PushRequestNewObject;
        }());
        Data.PushRequestNewObject = PushRequestNewObject;
        var PushRequest = /** @class */ (function () {
            function PushRequest() {
            }
            return PushRequest;
        }());
        Data.PushRequest = PushRequest;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var SyncRequest = /** @class */ (function () {
            function SyncRequest() {
            }
            return SyncRequest;
        }());
        Data.SyncRequest = SyncRequest;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var ResponseType;
        (function (ResponseType) {
            ResponseType[ResponseType["Pull"] = 0] = "Pull";
            ResponseType[ResponseType["Sync"] = 1] = "Sync";
            ResponseType[ResponseType["Push"] = 2] = "Push";
            ResponseType[ResponseType["Invoke"] = 3] = "Invoke";
        })(ResponseType = Data.ResponseType || (Data.ResponseType = {}));
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        function extend(type, extension) {
            Object.getOwnPropertyNames(extension).forEach(function (name) {
                var ownPropertyDescriptor = Object.getOwnPropertyDescriptor(extension, name);
                Object.defineProperty(type.prototype, name, ownPropertyDescriptor);
            });
        }
        Domain.extend = extend;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var ObjectType = /** @class */ (function () {
            function ObjectType(data) {
                var _this = this;
                this.roleTypeByName = {};
                this.name = data.name;
                _.forEach(data.roleTypes, function (roleTypeData) {
                    var roleType = new Meta.RoleType(roleTypeData);
                    _this.roleTypeByName[roleType.name] = roleType;
                });
            }
            return ObjectType;
        }());
        Meta.ObjectType = ObjectType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        var RoleType = /** @class */ (function () {
            function RoleType(roleTypeData) {
                this.name = roleTypeData.name;
                this.objectType = roleTypeData.objectType;
                this.isUnit = roleTypeData.isUnit;
                this.isOne = roleTypeData.isOne;
                this.isRequired = roleTypeData.isRequired;
            }
            Object.defineProperty(RoleType.prototype, "isComposite", {
                get: function () { return !this.isUnit; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(RoleType.prototype, "isMany", {
                get: function () { return this.isComposite && !this.isOne; },
                enumerable: true,
                configurable: true
            });
            return RoleType;
        }());
        Meta.RoleType = RoleType;
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var AccessControl = /** @class */ (function (_super) {
            __extends(AccessControl, _super);
            function AccessControl() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(AccessControl.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AccessControl.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return AccessControl;
        }(Allors.SessionObject));
        Domain.AccessControl = AccessControl;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var AutomatedAgent = /** @class */ (function (_super) {
            __extends(AutomatedAgent, _super);
            function AutomatedAgent() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(AutomatedAgent.prototype, "CanReadUserName", {
                get: function () {
                    return this.canRead("UserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanWriteUserName", {
                get: function () {
                    return this.canWrite("UserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "UserName", {
                get: function () {
                    return this.get("UserName");
                },
                set: function (value) {
                    this.set("UserName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanReadNormalizedUserName", {
                get: function () {
                    return this.canRead("NormalizedUserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanWriteNormalizedUserName", {
                get: function () {
                    return this.canWrite("NormalizedUserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "NormalizedUserName", {
                get: function () {
                    return this.get("NormalizedUserName");
                },
                set: function (value) {
                    this.set("NormalizedUserName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanReadUserEmail", {
                get: function () {
                    return this.canRead("UserEmail");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanWriteUserEmail", {
                get: function () {
                    return this.canWrite("UserEmail");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "UserEmail", {
                get: function () {
                    return this.get("UserEmail");
                },
                set: function (value) {
                    this.set("UserEmail", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanReadUserEmailConfirmed", {
                get: function () {
                    return this.canRead("UserEmailConfirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanWriteUserEmailConfirmed", {
                get: function () {
                    return this.canWrite("UserEmailConfirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "UserEmailConfirmed", {
                get: function () {
                    return this.get("UserEmailConfirmed");
                },
                set: function (value) {
                    this.set("UserEmailConfirmed", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanReadNotificationList", {
                get: function () {
                    return this.canRead("NotificationList");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "CanWriteNotificationList", {
                get: function () {
                    return this.canWrite("NotificationList");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(AutomatedAgent.prototype, "NotificationList", {
                get: function () {
                    return this.get("NotificationList");
                },
                set: function (value) {
                    this.set("NotificationList", value);
                },
                enumerable: true,
                configurable: true
            });
            return AutomatedAgent;
        }(Allors.SessionObject));
        Domain.AutomatedAgent = AutomatedAgent;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var C1 = /** @class */ (function (_super) {
            __extends(C1, _super);
            function C1() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(C1.prototype, "CanReadC1AllorsBinary", {
                get: function () {
                    return this.canRead("C1AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsBinary", {
                get: function () {
                    return this.canWrite("C1AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsBinary", {
                get: function () {
                    return this.get("C1AllorsBinary");
                },
                set: function (value) {
                    this.set("C1AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsBoolean", {
                get: function () {
                    return this.canRead("C1AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsBoolean", {
                get: function () {
                    return this.canWrite("C1AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsBoolean", {
                get: function () {
                    return this.get("C1AllorsBoolean");
                },
                set: function (value) {
                    this.set("C1AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsDateTime", {
                get: function () {
                    return this.canRead("C1AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsDateTime", {
                get: function () {
                    return this.canWrite("C1AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsDateTime", {
                get: function () {
                    return this.get("C1AllorsDateTime");
                },
                set: function (value) {
                    this.set("C1AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsDecimal", {
                get: function () {
                    return this.canRead("C1AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsDecimal", {
                get: function () {
                    return this.canWrite("C1AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsDecimal", {
                get: function () {
                    return this.get("C1AllorsDecimal");
                },
                set: function (value) {
                    this.set("C1AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsDouble", {
                get: function () {
                    return this.canRead("C1AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsDouble", {
                get: function () {
                    return this.canWrite("C1AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsDouble", {
                get: function () {
                    return this.get("C1AllorsDouble");
                },
                set: function (value) {
                    this.set("C1AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsInteger", {
                get: function () {
                    return this.canRead("C1AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsInteger", {
                get: function () {
                    return this.canWrite("C1AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsInteger", {
                get: function () {
                    return this.get("C1AllorsInteger");
                },
                set: function (value) {
                    this.set("C1AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsString", {
                get: function () {
                    return this.canRead("C1AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsString", {
                get: function () {
                    return this.canWrite("C1AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsString", {
                get: function () {
                    return this.get("C1AllorsString");
                },
                set: function (value) {
                    this.set("C1AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadAllorsStringMax", {
                get: function () {
                    return this.canRead("AllorsStringMax");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteAllorsStringMax", {
                get: function () {
                    return this.canWrite("AllorsStringMax");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "AllorsStringMax", {
                get: function () {
                    return this.get("AllorsStringMax");
                },
                set: function (value) {
                    this.set("AllorsStringMax", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1AllorsUnique", {
                get: function () {
                    return this.canRead("C1AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1AllorsUnique", {
                get: function () {
                    return this.canWrite("C1AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1AllorsUnique", {
                get: function () {
                    return this.get("C1AllorsUnique");
                },
                set: function (value) {
                    this.set("C1AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1C1Many2Manies", {
                get: function () {
                    return this.canRead("C1C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C1Many2Manies", {
                get: function () {
                    return this.canWrite("C1C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C1Many2Manies", {
                get: function () {
                    return this.get("C1C1Many2Manies");
                },
                set: function (value) {
                    this.set("C1C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1C1Many2Many = function (value) {
                return this.add("C1C1Many2Manies", value);
            };
            C1.prototype.RemoveC1C1Many2Many = function (value) {
                return this.remove("C1C1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1C1Many2One", {
                get: function () {
                    return this.canRead("C1C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C1Many2One", {
                get: function () {
                    return this.canWrite("C1C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C1Many2One", {
                get: function () {
                    return this.get("C1C1Many2One");
                },
                set: function (value) {
                    this.set("C1C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1C1One2Manies", {
                get: function () {
                    return this.canRead("C1C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C1One2Manies", {
                get: function () {
                    return this.canWrite("C1C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C1One2Manies", {
                get: function () {
                    return this.get("C1C1One2Manies");
                },
                set: function (value) {
                    this.set("C1C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1C1One2Many = function (value) {
                return this.add("C1C1One2Manies", value);
            };
            C1.prototype.RemoveC1C1One2Many = function (value) {
                return this.remove("C1C1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1C1One2One", {
                get: function () {
                    return this.canRead("C1C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C1One2One", {
                get: function () {
                    return this.canWrite("C1C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C1One2One", {
                get: function () {
                    return this.get("C1C1One2One");
                },
                set: function (value) {
                    this.set("C1C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1C2Many2Manies", {
                get: function () {
                    return this.canRead("C1C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C2Many2Manies", {
                get: function () {
                    return this.canWrite("C1C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C2Many2Manies", {
                get: function () {
                    return this.get("C1C2Many2Manies");
                },
                set: function (value) {
                    this.set("C1C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1C2Many2Many = function (value) {
                return this.add("C1C2Many2Manies", value);
            };
            C1.prototype.RemoveC1C2Many2Many = function (value) {
                return this.remove("C1C2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1C2Many2One", {
                get: function () {
                    return this.canRead("C1C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C2Many2One", {
                get: function () {
                    return this.canWrite("C1C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C2Many2One", {
                get: function () {
                    return this.get("C1C2Many2One");
                },
                set: function (value) {
                    this.set("C1C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1C2One2Manies", {
                get: function () {
                    return this.canRead("C1C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C2One2Manies", {
                get: function () {
                    return this.canWrite("C1C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C2One2Manies", {
                get: function () {
                    return this.get("C1C2One2Manies");
                },
                set: function (value) {
                    this.set("C1C2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1C2One2Many = function (value) {
                return this.add("C1C2One2Manies", value);
            };
            C1.prototype.RemoveC1C2One2Many = function (value) {
                return this.remove("C1C2One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1C2One2One", {
                get: function () {
                    return this.canRead("C1C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1C2One2One", {
                get: function () {
                    return this.canWrite("C1C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1C2One2One", {
                get: function () {
                    return this.get("C1C2One2One");
                },
                set: function (value) {
                    this.set("C1C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I12Many2Manies", {
                get: function () {
                    return this.canRead("C1I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I12Many2Manies", {
                get: function () {
                    return this.canWrite("C1I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I12Many2Manies", {
                get: function () {
                    return this.get("C1I12Many2Manies");
                },
                set: function (value) {
                    this.set("C1I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I12Many2Many = function (value) {
                return this.add("C1I12Many2Manies", value);
            };
            C1.prototype.RemoveC1I12Many2Many = function (value) {
                return this.remove("C1I12Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I12Many2One", {
                get: function () {
                    return this.canRead("C1I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I12Many2One", {
                get: function () {
                    return this.canWrite("C1I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I12Many2One", {
                get: function () {
                    return this.get("C1I12Many2One");
                },
                set: function (value) {
                    this.set("C1I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I12One2Manies", {
                get: function () {
                    return this.canRead("C1I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I12One2Manies", {
                get: function () {
                    return this.canWrite("C1I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I12One2Manies", {
                get: function () {
                    return this.get("C1I12One2Manies");
                },
                set: function (value) {
                    this.set("C1I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I12One2Many = function (value) {
                return this.add("C1I12One2Manies", value);
            };
            C1.prototype.RemoveC1I12One2Many = function (value) {
                return this.remove("C1I12One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I12One2One", {
                get: function () {
                    return this.canRead("C1I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I12One2One", {
                get: function () {
                    return this.canWrite("C1I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I12One2One", {
                get: function () {
                    return this.get("C1I12One2One");
                },
                set: function (value) {
                    this.set("C1I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I1Many2Manies", {
                get: function () {
                    return this.canRead("C1I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I1Many2Manies", {
                get: function () {
                    return this.canWrite("C1I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I1Many2Manies", {
                get: function () {
                    return this.get("C1I1Many2Manies");
                },
                set: function (value) {
                    this.set("C1I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I1Many2Many = function (value) {
                return this.add("C1I1Many2Manies", value);
            };
            C1.prototype.RemoveC1I1Many2Many = function (value) {
                return this.remove("C1I1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I1Many2One", {
                get: function () {
                    return this.canRead("C1I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I1Many2One", {
                get: function () {
                    return this.canWrite("C1I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I1Many2One", {
                get: function () {
                    return this.get("C1I1Many2One");
                },
                set: function (value) {
                    this.set("C1I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I1One2Manies", {
                get: function () {
                    return this.canRead("C1I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I1One2Manies", {
                get: function () {
                    return this.canWrite("C1I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I1One2Manies", {
                get: function () {
                    return this.get("C1I1One2Manies");
                },
                set: function (value) {
                    this.set("C1I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I1One2Many = function (value) {
                return this.add("C1I1One2Manies", value);
            };
            C1.prototype.RemoveC1I1One2Many = function (value) {
                return this.remove("C1I1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I1One2One", {
                get: function () {
                    return this.canRead("C1I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I1One2One", {
                get: function () {
                    return this.canWrite("C1I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I1One2One", {
                get: function () {
                    return this.get("C1I1One2One");
                },
                set: function (value) {
                    this.set("C1I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I2Many2Manies", {
                get: function () {
                    return this.canRead("C1I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I2Many2Manies", {
                get: function () {
                    return this.canWrite("C1I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I2Many2Manies", {
                get: function () {
                    return this.get("C1I2Many2Manies");
                },
                set: function (value) {
                    this.set("C1I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I2Many2Many = function (value) {
                return this.add("C1I2Many2Manies", value);
            };
            C1.prototype.RemoveC1I2Many2Many = function (value) {
                return this.remove("C1I2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I2Many2One", {
                get: function () {
                    return this.canRead("C1I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I2Many2One", {
                get: function () {
                    return this.canWrite("C1I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I2Many2One", {
                get: function () {
                    return this.get("C1I2Many2One");
                },
                set: function (value) {
                    this.set("C1I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadC1I2One2Manies", {
                get: function () {
                    return this.canRead("C1I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I2One2Manies", {
                get: function () {
                    return this.canWrite("C1I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I2One2Manies", {
                get: function () {
                    return this.get("C1I2One2Manies");
                },
                set: function (value) {
                    this.set("C1I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddC1I2One2Many = function (value) {
                return this.add("C1I2One2Manies", value);
            };
            C1.prototype.RemoveC1I2One2Many = function (value) {
                return this.remove("C1I2One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadC1I2One2One", {
                get: function () {
                    return this.canRead("C1I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteC1I2One2One", {
                get: function () {
                    return this.canWrite("C1I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "C1I2One2One", {
                get: function () {
                    return this.get("C1I2One2One");
                },
                set: function (value) {
                    this.set("C1I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I1Many2One", {
                get: function () {
                    return this.canRead("I1I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I1Many2One", {
                get: function () {
                    return this.canWrite("I1I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I1Many2One", {
                get: function () {
                    return this.get("I1I1Many2One");
                },
                set: function (value) {
                    this.set("I1I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I12Many2Manies", {
                get: function () {
                    return this.canRead("I1I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I12Many2Manies", {
                get: function () {
                    return this.canWrite("I1I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I12Many2Manies", {
                get: function () {
                    return this.get("I1I12Many2Manies");
                },
                set: function (value) {
                    this.set("I1I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I12Many2Many = function (value) {
                return this.add("I1I12Many2Manies", value);
            };
            C1.prototype.RemoveI1I12Many2Many = function (value) {
                return this.remove("I1I12Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1I2Many2Manies", {
                get: function () {
                    return this.canRead("I1I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I2Many2Manies", {
                get: function () {
                    return this.canWrite("I1I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I2Many2Manies", {
                get: function () {
                    return this.get("I1I2Many2Manies");
                },
                set: function (value) {
                    this.set("I1I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I2Many2Many = function (value) {
                return this.add("I1I2Many2Manies", value);
            };
            C1.prototype.RemoveI1I2Many2Many = function (value) {
                return this.remove("I1I2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1I2Many2One", {
                get: function () {
                    return this.canRead("I1I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I2Many2One", {
                get: function () {
                    return this.canWrite("I1I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I2Many2One", {
                get: function () {
                    return this.get("I1I2Many2One");
                },
                set: function (value) {
                    this.set("I1I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1AllorsString", {
                get: function () {
                    return this.canRead("I1AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsString", {
                get: function () {
                    return this.canWrite("I1AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsString", {
                get: function () {
                    return this.get("I1AllorsString");
                },
                set: function (value) {
                    this.set("I1AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I12Many2One", {
                get: function () {
                    return this.canRead("I1I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I12Many2One", {
                get: function () {
                    return this.canWrite("I1I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I12Many2One", {
                get: function () {
                    return this.get("I1I12Many2One");
                },
                set: function (value) {
                    this.set("I1I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1AllorsDateTime", {
                get: function () {
                    return this.canRead("I1AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsDateTime", {
                get: function () {
                    return this.canWrite("I1AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsDateTime", {
                get: function () {
                    return this.get("I1AllorsDateTime");
                },
                set: function (value) {
                    this.set("I1AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I2One2Manies", {
                get: function () {
                    return this.canRead("I1I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I2One2Manies", {
                get: function () {
                    return this.canWrite("I1I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I2One2Manies", {
                get: function () {
                    return this.get("I1I2One2Manies");
                },
                set: function (value) {
                    this.set("I1I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I2One2Many = function (value) {
                return this.add("I1I2One2Manies", value);
            };
            C1.prototype.RemoveI1I2One2Many = function (value) {
                return this.remove("I1I2One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1C2One2Manies", {
                get: function () {
                    return this.canRead("I1C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C2One2Manies", {
                get: function () {
                    return this.canWrite("I1C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C2One2Manies", {
                get: function () {
                    return this.get("I1C2One2Manies");
                },
                set: function (value) {
                    this.set("I1C2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1C2One2Many = function (value) {
                return this.add("I1C2One2Manies", value);
            };
            C1.prototype.RemoveI1C2One2Many = function (value) {
                return this.remove("I1C2One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1C1One2One", {
                get: function () {
                    return this.canRead("I1C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C1One2One", {
                get: function () {
                    return this.canWrite("I1C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C1One2One", {
                get: function () {
                    return this.get("I1C1One2One");
                },
                set: function (value) {
                    this.set("I1C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1AllorsInteger", {
                get: function () {
                    return this.canRead("I1AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsInteger", {
                get: function () {
                    return this.canWrite("I1AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsInteger", {
                get: function () {
                    return this.get("I1AllorsInteger");
                },
                set: function (value) {
                    this.set("I1AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1C2Many2Manies", {
                get: function () {
                    return this.canRead("I1C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C2Many2Manies", {
                get: function () {
                    return this.canWrite("I1C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C2Many2Manies", {
                get: function () {
                    return this.get("I1C2Many2Manies");
                },
                set: function (value) {
                    this.set("I1C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1C2Many2Many = function (value) {
                return this.add("I1C2Many2Manies", value);
            };
            C1.prototype.RemoveI1C2Many2Many = function (value) {
                return this.remove("I1C2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1I1One2Manies", {
                get: function () {
                    return this.canRead("I1I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I1One2Manies", {
                get: function () {
                    return this.canWrite("I1I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I1One2Manies", {
                get: function () {
                    return this.get("I1I1One2Manies");
                },
                set: function (value) {
                    this.set("I1I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I1One2Many = function (value) {
                return this.add("I1I1One2Manies", value);
            };
            C1.prototype.RemoveI1I1One2Many = function (value) {
                return this.remove("I1I1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1I1Many2Manies", {
                get: function () {
                    return this.canRead("I1I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I1Many2Manies", {
                get: function () {
                    return this.canWrite("I1I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I1Many2Manies", {
                get: function () {
                    return this.get("I1I1Many2Manies");
                },
                set: function (value) {
                    this.set("I1I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I1Many2Many = function (value) {
                return this.add("I1I1Many2Manies", value);
            };
            C1.prototype.RemoveI1I1Many2Many = function (value) {
                return this.remove("I1I1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1AllorsBoolean", {
                get: function () {
                    return this.canRead("I1AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsBoolean", {
                get: function () {
                    return this.canWrite("I1AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsBoolean", {
                get: function () {
                    return this.get("I1AllorsBoolean");
                },
                set: function (value) {
                    this.set("I1AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1AllorsDecimal", {
                get: function () {
                    return this.canRead("I1AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsDecimal", {
                get: function () {
                    return this.canWrite("I1AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsDecimal", {
                get: function () {
                    return this.get("I1AllorsDecimal");
                },
                set: function (value) {
                    this.set("I1AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I12One2One", {
                get: function () {
                    return this.canRead("I1I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I12One2One", {
                get: function () {
                    return this.canWrite("I1I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I12One2One", {
                get: function () {
                    return this.get("I1I12One2One");
                },
                set: function (value) {
                    this.set("I1I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I2One2One", {
                get: function () {
                    return this.canRead("I1I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I2One2One", {
                get: function () {
                    return this.canWrite("I1I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I2One2One", {
                get: function () {
                    return this.get("I1I2One2One");
                },
                set: function (value) {
                    this.set("I1I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1C2One2One", {
                get: function () {
                    return this.canRead("I1C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C2One2One", {
                get: function () {
                    return this.canWrite("I1C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C2One2One", {
                get: function () {
                    return this.get("I1C2One2One");
                },
                set: function (value) {
                    this.set("I1C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1C1One2Manies", {
                get: function () {
                    return this.canRead("I1C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C1One2Manies", {
                get: function () {
                    return this.canWrite("I1C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C1One2Manies", {
                get: function () {
                    return this.get("I1C1One2Manies");
                },
                set: function (value) {
                    this.set("I1C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1C1One2Many = function (value) {
                return this.add("I1C1One2Manies", value);
            };
            C1.prototype.RemoveI1C1One2Many = function (value) {
                return this.remove("I1C1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1AllorsBinary", {
                get: function () {
                    return this.canRead("I1AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsBinary", {
                get: function () {
                    return this.canWrite("I1AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsBinary", {
                get: function () {
                    return this.get("I1AllorsBinary");
                },
                set: function (value) {
                    this.set("I1AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1C1Many2Manies", {
                get: function () {
                    return this.canRead("I1C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C1Many2Manies", {
                get: function () {
                    return this.canWrite("I1C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C1Many2Manies", {
                get: function () {
                    return this.get("I1C1Many2Manies");
                },
                set: function (value) {
                    this.set("I1C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1C1Many2Many = function (value) {
                return this.add("I1C1Many2Manies", value);
            };
            C1.prototype.RemoveI1C1Many2Many = function (value) {
                return this.remove("I1C1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1AllorsDouble", {
                get: function () {
                    return this.canRead("I1AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsDouble", {
                get: function () {
                    return this.canWrite("I1AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsDouble", {
                get: function () {
                    return this.get("I1AllorsDouble");
                },
                set: function (value) {
                    this.set("I1AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I1One2One", {
                get: function () {
                    return this.canRead("I1I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I1One2One", {
                get: function () {
                    return this.canWrite("I1I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I1One2One", {
                get: function () {
                    return this.get("I1I1One2One");
                },
                set: function (value) {
                    this.set("I1I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1C1Many2One", {
                get: function () {
                    return this.canRead("I1C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C1Many2One", {
                get: function () {
                    return this.canWrite("I1C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C1Many2One", {
                get: function () {
                    return this.get("I1C1Many2One");
                },
                set: function (value) {
                    this.set("I1C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1I12One2Manies", {
                get: function () {
                    return this.canRead("I1I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1I12One2Manies", {
                get: function () {
                    return this.canWrite("I1I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1I12One2Manies", {
                get: function () {
                    return this.get("I1I12One2Manies");
                },
                set: function (value) {
                    this.set("I1I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI1I12One2Many = function (value) {
                return this.add("I1I12One2Manies", value);
            };
            C1.prototype.RemoveI1I12One2Many = function (value) {
                return this.remove("I1I12One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI1C2Many2One", {
                get: function () {
                    return this.canRead("I1C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1C2Many2One", {
                get: function () {
                    return this.canWrite("I1C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1C2Many2One", {
                get: function () {
                    return this.get("I1C2Many2One");
                },
                set: function (value) {
                    this.set("I1C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI1AllorsUnique", {
                get: function () {
                    return this.canRead("I1AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI1AllorsUnique", {
                get: function () {
                    return this.canWrite("I1AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I1AllorsUnique", {
                get: function () {
                    return this.get("I1AllorsUnique");
                },
                set: function (value) {
                    this.set("I1AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsBinary", {
                get: function () {
                    return this.canRead("I12AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsBinary", {
                get: function () {
                    return this.canWrite("I12AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsBinary", {
                get: function () {
                    return this.get("I12AllorsBinary");
                },
                set: function (value) {
                    this.set("I12AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12C2One2One", {
                get: function () {
                    return this.canRead("I12C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C2One2One", {
                get: function () {
                    return this.canWrite("I12C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C2One2One", {
                get: function () {
                    return this.get("I12C2One2One");
                },
                set: function (value) {
                    this.set("I12C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsDouble", {
                get: function () {
                    return this.canRead("I12AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsDouble", {
                get: function () {
                    return this.canWrite("I12AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsDouble", {
                get: function () {
                    return this.get("I12AllorsDouble");
                },
                set: function (value) {
                    this.set("I12AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I1Many2One", {
                get: function () {
                    return this.canRead("I12I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I1Many2One", {
                get: function () {
                    return this.canWrite("I12I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I1Many2One", {
                get: function () {
                    return this.get("I12I1Many2One");
                },
                set: function (value) {
                    this.set("I12I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsString", {
                get: function () {
                    return this.canRead("I12AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsString", {
                get: function () {
                    return this.canWrite("I12AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsString", {
                get: function () {
                    return this.get("I12AllorsString");
                },
                set: function (value) {
                    this.set("I12AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I12Many2Manies", {
                get: function () {
                    return this.canRead("I12I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I12Many2Manies", {
                get: function () {
                    return this.canWrite("I12I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I12Many2Manies", {
                get: function () {
                    return this.get("I12I12Many2Manies");
                },
                set: function (value) {
                    this.set("I12I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I12Many2Many = function (value) {
                return this.add("I12I12Many2Manies", value);
            };
            C1.prototype.RemoveI12I12Many2Many = function (value) {
                return this.remove("I12I12Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12AllorsDecimal", {
                get: function () {
                    return this.canRead("I12AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsDecimal", {
                get: function () {
                    return this.canWrite("I12AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsDecimal", {
                get: function () {
                    return this.get("I12AllorsDecimal");
                },
                set: function (value) {
                    this.set("I12AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I2Many2Manies", {
                get: function () {
                    return this.canRead("I12I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I2Many2Manies", {
                get: function () {
                    return this.canWrite("I12I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I2Many2Manies", {
                get: function () {
                    return this.get("I12I2Many2Manies");
                },
                set: function (value) {
                    this.set("I12I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I2Many2Many = function (value) {
                return this.add("I12I2Many2Manies", value);
            };
            C1.prototype.RemoveI12I2Many2Many = function (value) {
                return this.remove("I12I2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12C2Many2Manies", {
                get: function () {
                    return this.canRead("I12C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C2Many2Manies", {
                get: function () {
                    return this.canWrite("I12C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C2Many2Manies", {
                get: function () {
                    return this.get("I12C2Many2Manies");
                },
                set: function (value) {
                    this.set("I12C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12C2Many2Many = function (value) {
                return this.add("I12C2Many2Manies", value);
            };
            C1.prototype.RemoveI12C2Many2Many = function (value) {
                return this.remove("I12C2Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12I1Many2Manies", {
                get: function () {
                    return this.canRead("I12I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I1Many2Manies", {
                get: function () {
                    return this.canWrite("I12I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I1Many2Manies", {
                get: function () {
                    return this.get("I12I1Many2Manies");
                },
                set: function (value) {
                    this.set("I12I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I1Many2Many = function (value) {
                return this.add("I12I1Many2Manies", value);
            };
            C1.prototype.RemoveI12I1Many2Many = function (value) {
                return this.remove("I12I1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12I12One2Manies", {
                get: function () {
                    return this.canRead("I12I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I12One2Manies", {
                get: function () {
                    return this.canWrite("I12I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I12One2Manies", {
                get: function () {
                    return this.get("I12I12One2Manies");
                },
                set: function (value) {
                    this.set("I12I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I12One2Many = function (value) {
                return this.add("I12I12One2Manies", value);
            };
            C1.prototype.RemoveI12I12One2Many = function (value) {
                return this.remove("I12I12One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12C1Many2Manies", {
                get: function () {
                    return this.canRead("I12C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C1Many2Manies", {
                get: function () {
                    return this.canWrite("I12C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C1Many2Manies", {
                get: function () {
                    return this.get("I12C1Many2Manies");
                },
                set: function (value) {
                    this.set("I12C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12C1Many2Many = function (value) {
                return this.add("I12C1Many2Manies", value);
            };
            C1.prototype.RemoveI12C1Many2Many = function (value) {
                return this.remove("I12C1Many2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12I2Many2One", {
                get: function () {
                    return this.canRead("I12I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I2Many2One", {
                get: function () {
                    return this.canWrite("I12I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I2Many2One", {
                get: function () {
                    return this.get("I12I2Many2One");
                },
                set: function (value) {
                    this.set("I12I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsUnique", {
                get: function () {
                    return this.canRead("I12AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsUnique", {
                get: function () {
                    return this.canWrite("I12AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsUnique", {
                get: function () {
                    return this.get("I12AllorsUnique");
                },
                set: function (value) {
                    this.set("I12AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsInteger", {
                get: function () {
                    return this.canRead("I12AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsInteger", {
                get: function () {
                    return this.canWrite("I12AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsInteger", {
                get: function () {
                    return this.get("I12AllorsInteger");
                },
                set: function (value) {
                    this.set("I12AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I1One2Manies", {
                get: function () {
                    return this.canRead("I12I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I1One2Manies", {
                get: function () {
                    return this.canWrite("I12I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I1One2Manies", {
                get: function () {
                    return this.get("I12I1One2Manies");
                },
                set: function (value) {
                    this.set("I12I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I1One2Many = function (value) {
                return this.add("I12I1One2Manies", value);
            };
            C1.prototype.RemoveI12I1One2Many = function (value) {
                return this.remove("I12I1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12C1One2One", {
                get: function () {
                    return this.canRead("I12C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C1One2One", {
                get: function () {
                    return this.canWrite("I12C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C1One2One", {
                get: function () {
                    return this.get("I12C1One2One");
                },
                set: function (value) {
                    this.set("I12C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I12One2One", {
                get: function () {
                    return this.canRead("I12I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I12One2One", {
                get: function () {
                    return this.canWrite("I12I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I12One2One", {
                get: function () {
                    return this.get("I12I12One2One");
                },
                set: function (value) {
                    this.set("I12I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I2One2One", {
                get: function () {
                    return this.canRead("I12I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I2One2One", {
                get: function () {
                    return this.canWrite("I12I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I2One2One", {
                get: function () {
                    return this.get("I12I2One2One");
                },
                set: function (value) {
                    this.set("I12I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadDependencies", {
                get: function () {
                    return this.canRead("Dependencies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteDependencies", {
                get: function () {
                    return this.canWrite("Dependencies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "Dependencies", {
                get: function () {
                    return this.get("Dependencies");
                },
                set: function (value) {
                    this.set("Dependencies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddDependency = function (value) {
                return this.add("Dependencies", value);
            };
            C1.prototype.RemoveDependency = function (value) {
                return this.remove("Dependencies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12I2One2Manies", {
                get: function () {
                    return this.canRead("I12I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I2One2Manies", {
                get: function () {
                    return this.canWrite("I12I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I2One2Manies", {
                get: function () {
                    return this.get("I12I2One2Manies");
                },
                set: function (value) {
                    this.set("I12I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12I2One2Many = function (value) {
                return this.add("I12I2One2Manies", value);
            };
            C1.prototype.RemoveI12I2One2Many = function (value) {
                return this.remove("I12I2One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12C2Many2One", {
                get: function () {
                    return this.canRead("I12C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C2Many2One", {
                get: function () {
                    return this.canWrite("I12C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C2Many2One", {
                get: function () {
                    return this.get("I12C2Many2One");
                },
                set: function (value) {
                    this.set("I12C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I12Many2One", {
                get: function () {
                    return this.canRead("I12I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I12Many2One", {
                get: function () {
                    return this.canWrite("I12I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I12Many2One", {
                get: function () {
                    return this.get("I12I12Many2One");
                },
                set: function (value) {
                    this.set("I12I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsBoolean", {
                get: function () {
                    return this.canRead("I12AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsBoolean", {
                get: function () {
                    return this.canWrite("I12AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsBoolean", {
                get: function () {
                    return this.get("I12AllorsBoolean");
                },
                set: function (value) {
                    this.set("I12AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12I1One2One", {
                get: function () {
                    return this.canRead("I12I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12I1One2One", {
                get: function () {
                    return this.canWrite("I12I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12I1One2One", {
                get: function () {
                    return this.get("I12I1One2One");
                },
                set: function (value) {
                    this.set("I12I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12C1One2Manies", {
                get: function () {
                    return this.canRead("I12C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C1One2Manies", {
                get: function () {
                    return this.canWrite("I12C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C1One2Manies", {
                get: function () {
                    return this.get("I12C1One2Manies");
                },
                set: function (value) {
                    this.set("I12C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C1.prototype.AddI12C1One2Many = function (value) {
                return this.add("I12C1One2Manies", value);
            };
            C1.prototype.RemoveI12C1One2Many = function (value) {
                return this.remove("I12C1One2Manies", value);
            };
            Object.defineProperty(C1.prototype, "CanReadI12C1Many2One", {
                get: function () {
                    return this.canRead("I12C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12C1Many2One", {
                get: function () {
                    return this.canWrite("I12C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12C1Many2One", {
                get: function () {
                    return this.get("I12C1Many2One");
                },
                set: function (value) {
                    this.set("I12C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanReadI12AllorsDateTime", {
                get: function () {
                    return this.canRead("I12AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanWriteI12AllorsDateTime", {
                get: function () {
                    return this.canWrite("I12AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "I12AllorsDateTime", {
                get: function () {
                    return this.get("I12AllorsDateTime");
                },
                set: function (value) {
                    this.set("I12AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "CanExecuteClassMethod", {
                get: function () {
                    return this.canExecute("ClassMethod");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C1.prototype, "ClassMethod", {
                get: function () {
                    return new Allors.Method(this, "ClassMethod");
                },
                enumerable: true,
                configurable: true
            });
            return C1;
        }(Allors.SessionObject));
        Domain.C1 = C1;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var C2 = /** @class */ (function (_super) {
            __extends(C2, _super);
            function C2() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(C2.prototype, "CanReadC2AllorsDecimal", {
                get: function () {
                    return this.canRead("C2AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsDecimal", {
                get: function () {
                    return this.canWrite("C2AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsDecimal", {
                get: function () {
                    return this.get("C2AllorsDecimal");
                },
                set: function (value) {
                    this.set("C2AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C1One2One", {
                get: function () {
                    return this.canRead("C2C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C1One2One", {
                get: function () {
                    return this.canWrite("C2C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C1One2One", {
                get: function () {
                    return this.get("C2C1One2One");
                },
                set: function (value) {
                    this.set("C2C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C2Many2One", {
                get: function () {
                    return this.canRead("C2C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C2Many2One", {
                get: function () {
                    return this.canWrite("C2C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C2Many2One", {
                get: function () {
                    return this.get("C2C2Many2One");
                },
                set: function (value) {
                    this.set("C2C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2AllorsUnique", {
                get: function () {
                    return this.canRead("C2AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsUnique", {
                get: function () {
                    return this.canWrite("C2AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsUnique", {
                get: function () {
                    return this.get("C2AllorsUnique");
                },
                set: function (value) {
                    this.set("C2AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I12Many2One", {
                get: function () {
                    return this.canRead("C2I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I12Many2One", {
                get: function () {
                    return this.canWrite("C2I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I12Many2One", {
                get: function () {
                    return this.get("C2I12Many2One");
                },
                set: function (value) {
                    this.set("C2I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I12One2One", {
                get: function () {
                    return this.canRead("C2I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I12One2One", {
                get: function () {
                    return this.canWrite("C2I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I12One2One", {
                get: function () {
                    return this.get("C2I12One2One");
                },
                set: function (value) {
                    this.set("C2I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I1Many2Manies", {
                get: function () {
                    return this.canRead("C2I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I1Many2Manies", {
                get: function () {
                    return this.canWrite("C2I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I1Many2Manies", {
                get: function () {
                    return this.get("C2I1Many2Manies");
                },
                set: function (value) {
                    this.set("C2I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I1Many2Many = function (value) {
                return this.add("C2I1Many2Manies", value);
            };
            C2.prototype.RemoveC2I1Many2Many = function (value) {
                return this.remove("C2I1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2AllorsDouble", {
                get: function () {
                    return this.canRead("C2AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsDouble", {
                get: function () {
                    return this.canWrite("C2AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsDouble", {
                get: function () {
                    return this.get("C2AllorsDouble");
                },
                set: function (value) {
                    this.set("C2AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I1One2Manies", {
                get: function () {
                    return this.canRead("C2I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I1One2Manies", {
                get: function () {
                    return this.canWrite("C2I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I1One2Manies", {
                get: function () {
                    return this.get("C2I1One2Manies");
                },
                set: function (value) {
                    this.set("C2I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I1One2Many = function (value) {
                return this.add("C2I1One2Manies", value);
            };
            C2.prototype.RemoveC2I1One2Many = function (value) {
                return this.remove("C2I1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2I2One2One", {
                get: function () {
                    return this.canRead("C2I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I2One2One", {
                get: function () {
                    return this.canWrite("C2I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I2One2One", {
                get: function () {
                    return this.get("C2I2One2One");
                },
                set: function (value) {
                    this.set("C2I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2AllorsInteger", {
                get: function () {
                    return this.canRead("C2AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsInteger", {
                get: function () {
                    return this.canWrite("C2AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsInteger", {
                get: function () {
                    return this.get("C2AllorsInteger");
                },
                set: function (value) {
                    this.set("C2AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I2Many2Manies", {
                get: function () {
                    return this.canRead("C2I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I2Many2Manies", {
                get: function () {
                    return this.canWrite("C2I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I2Many2Manies", {
                get: function () {
                    return this.get("C2I2Many2Manies");
                },
                set: function (value) {
                    this.set("C2I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I2Many2Many = function (value) {
                return this.add("C2I2Many2Manies", value);
            };
            C2.prototype.RemoveC2I2Many2Many = function (value) {
                return this.remove("C2I2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2I12Many2Manies", {
                get: function () {
                    return this.canRead("C2I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I12Many2Manies", {
                get: function () {
                    return this.canWrite("C2I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I12Many2Manies", {
                get: function () {
                    return this.get("C2I12Many2Manies");
                },
                set: function (value) {
                    this.set("C2I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I12Many2Many = function (value) {
                return this.add("C2I12Many2Manies", value);
            };
            C2.prototype.RemoveC2I12Many2Many = function (value) {
                return this.remove("C2I12Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2C2One2Manies", {
                get: function () {
                    return this.canRead("C2C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C2One2Manies", {
                get: function () {
                    return this.canWrite("C2C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C2One2Manies", {
                get: function () {
                    return this.get("C2C2One2Manies");
                },
                set: function (value) {
                    this.set("C2C2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2C2One2Many = function (value) {
                return this.add("C2C2One2Manies", value);
            };
            C2.prototype.RemoveC2C2One2Many = function (value) {
                return this.remove("C2C2One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2AllorsBoolean", {
                get: function () {
                    return this.canRead("C2AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsBoolean", {
                get: function () {
                    return this.canWrite("C2AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsBoolean", {
                get: function () {
                    return this.get("C2AllorsBoolean");
                },
                set: function (value) {
                    this.set("C2AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I1Many2One", {
                get: function () {
                    return this.canRead("C2I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I1Many2One", {
                get: function () {
                    return this.canWrite("C2I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I1Many2One", {
                get: function () {
                    return this.get("C2I1Many2One");
                },
                set: function (value) {
                    this.set("C2I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I1One2One", {
                get: function () {
                    return this.canRead("C2I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I1One2One", {
                get: function () {
                    return this.canWrite("C2I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I1One2One", {
                get: function () {
                    return this.get("C2I1One2One");
                },
                set: function (value) {
                    this.set("C2I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C1Many2Manies", {
                get: function () {
                    return this.canRead("C2C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C1Many2Manies", {
                get: function () {
                    return this.canWrite("C2C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C1Many2Manies", {
                get: function () {
                    return this.get("C2C1Many2Manies");
                },
                set: function (value) {
                    this.set("C2C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2C1Many2Many = function (value) {
                return this.add("C2C1Many2Manies", value);
            };
            C2.prototype.RemoveC2C1Many2Many = function (value) {
                return this.remove("C2C1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2I12One2Manies", {
                get: function () {
                    return this.canRead("C2I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I12One2Manies", {
                get: function () {
                    return this.canWrite("C2I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I12One2Manies", {
                get: function () {
                    return this.get("C2I12One2Manies");
                },
                set: function (value) {
                    this.set("C2I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I12One2Many = function (value) {
                return this.add("C2I12One2Manies", value);
            };
            C2.prototype.RemoveC2I12One2Many = function (value) {
                return this.remove("C2I12One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2I2One2Manies", {
                get: function () {
                    return this.canRead("C2I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I2One2Manies", {
                get: function () {
                    return this.canWrite("C2I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I2One2Manies", {
                get: function () {
                    return this.get("C2I2One2Manies");
                },
                set: function (value) {
                    this.set("C2I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2I2One2Many = function (value) {
                return this.add("C2I2One2Manies", value);
            };
            C2.prototype.RemoveC2I2One2Many = function (value) {
                return this.remove("C2I2One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2C2One2One", {
                get: function () {
                    return this.canRead("C2C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C2One2One", {
                get: function () {
                    return this.canWrite("C2C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C2One2One", {
                get: function () {
                    return this.get("C2C2One2One");
                },
                set: function (value) {
                    this.set("C2C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2AllorsString", {
                get: function () {
                    return this.canRead("C2AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsString", {
                get: function () {
                    return this.canWrite("C2AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsString", {
                get: function () {
                    return this.get("C2AllorsString");
                },
                set: function (value) {
                    this.set("C2AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C1Many2One", {
                get: function () {
                    return this.canRead("C2C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C1Many2One", {
                get: function () {
                    return this.canWrite("C2C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C1Many2One", {
                get: function () {
                    return this.get("C2C1Many2One");
                },
                set: function (value) {
                    this.set("C2C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C2Many2Manies", {
                get: function () {
                    return this.canRead("C2C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C2Many2Manies", {
                get: function () {
                    return this.canWrite("C2C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C2Many2Manies", {
                get: function () {
                    return this.get("C2C2Many2Manies");
                },
                set: function (value) {
                    this.set("C2C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2C2Many2Many = function (value) {
                return this.add("C2C2Many2Manies", value);
            };
            C2.prototype.RemoveC2C2Many2Many = function (value) {
                return this.remove("C2C2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2AllorsDateTime", {
                get: function () {
                    return this.canRead("C2AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsDateTime", {
                get: function () {
                    return this.canWrite("C2AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsDateTime", {
                get: function () {
                    return this.get("C2AllorsDateTime");
                },
                set: function (value) {
                    this.set("C2AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2I2Many2One", {
                get: function () {
                    return this.canRead("C2I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2I2Many2One", {
                get: function () {
                    return this.canWrite("C2I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2I2Many2One", {
                get: function () {
                    return this.get("C2I2Many2One");
                },
                set: function (value) {
                    this.set("C2I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadC2C1One2Manies", {
                get: function () {
                    return this.canRead("C2C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2C1One2Manies", {
                get: function () {
                    return this.canWrite("C2C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2C1One2Manies", {
                get: function () {
                    return this.get("C2C1One2Manies");
                },
                set: function (value) {
                    this.set("C2C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddC2C1One2Many = function (value) {
                return this.add("C2C1One2Manies", value);
            };
            C2.prototype.RemoveC2C1One2Many = function (value) {
                return this.remove("C2C1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadC2AllorsBinary", {
                get: function () {
                    return this.canRead("C2AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteC2AllorsBinary", {
                get: function () {
                    return this.canWrite("C2AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "C2AllorsBinary", {
                get: function () {
                    return this.get("C2AllorsBinary");
                },
                set: function (value) {
                    this.set("C2AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadS1One2One", {
                get: function () {
                    return this.canRead("S1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteS1One2One", {
                get: function () {
                    return this.canWrite("S1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "S1One2One", {
                get: function () {
                    return this.get("S1One2One");
                },
                set: function (value) {
                    this.set("S1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I2Many2One", {
                get: function () {
                    return this.canRead("I2I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I2Many2One", {
                get: function () {
                    return this.canWrite("I2I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I2Many2One", {
                get: function () {
                    return this.get("I2I2Many2One");
                },
                set: function (value) {
                    this.set("I2I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2C1Many2One", {
                get: function () {
                    return this.canRead("I2C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C1Many2One", {
                get: function () {
                    return this.canWrite("I2C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C1Many2One", {
                get: function () {
                    return this.get("I2C1Many2One");
                },
                set: function (value) {
                    this.set("I2C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I12Many2One", {
                get: function () {
                    return this.canRead("I2I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I12Many2One", {
                get: function () {
                    return this.canWrite("I2I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I12Many2One", {
                get: function () {
                    return this.get("I2I12Many2One");
                },
                set: function (value) {
                    this.set("I2I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsBoolean", {
                get: function () {
                    return this.canRead("I2AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsBoolean", {
                get: function () {
                    return this.canWrite("I2AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsBoolean", {
                get: function () {
                    return this.get("I2AllorsBoolean");
                },
                set: function (value) {
                    this.set("I2AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2C1One2Manies", {
                get: function () {
                    return this.canRead("I2C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C1One2Manies", {
                get: function () {
                    return this.canWrite("I2C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C1One2Manies", {
                get: function () {
                    return this.get("I2C1One2Manies");
                },
                set: function (value) {
                    this.set("I2C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2C1One2Many = function (value) {
                return this.add("I2C1One2Manies", value);
            };
            C2.prototype.RemoveI2C1One2Many = function (value) {
                return this.remove("I2C1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2C1One2One", {
                get: function () {
                    return this.canRead("I2C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C1One2One", {
                get: function () {
                    return this.canWrite("I2C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C1One2One", {
                get: function () {
                    return this.get("I2C1One2One");
                },
                set: function (value) {
                    this.set("I2C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsDecimal", {
                get: function () {
                    return this.canRead("I2AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsDecimal", {
                get: function () {
                    return this.canWrite("I2AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsDecimal", {
                get: function () {
                    return this.get("I2AllorsDecimal");
                },
                set: function (value) {
                    this.set("I2AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I2Many2Manies", {
                get: function () {
                    return this.canRead("I2I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I2Many2Manies", {
                get: function () {
                    return this.canWrite("I2I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I2Many2Manies", {
                get: function () {
                    return this.get("I2I2Many2Manies");
                },
                set: function (value) {
                    this.set("I2I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I2Many2Many = function (value) {
                return this.add("I2I2Many2Manies", value);
            };
            C2.prototype.RemoveI2I2Many2Many = function (value) {
                return this.remove("I2I2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2AllorsBinary", {
                get: function () {
                    return this.canRead("I2AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsBinary", {
                get: function () {
                    return this.canWrite("I2AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsBinary", {
                get: function () {
                    return this.get("I2AllorsBinary");
                },
                set: function (value) {
                    this.set("I2AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsUnique", {
                get: function () {
                    return this.canRead("I2AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsUnique", {
                get: function () {
                    return this.canWrite("I2AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsUnique", {
                get: function () {
                    return this.get("I2AllorsUnique");
                },
                set: function (value) {
                    this.set("I2AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I1Many2One", {
                get: function () {
                    return this.canRead("I2I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I1Many2One", {
                get: function () {
                    return this.canWrite("I2I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I1Many2One", {
                get: function () {
                    return this.get("I2I1Many2One");
                },
                set: function (value) {
                    this.set("I2I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsDateTime", {
                get: function () {
                    return this.canRead("I2AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsDateTime", {
                get: function () {
                    return this.canWrite("I2AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsDateTime", {
                get: function () {
                    return this.get("I2AllorsDateTime");
                },
                set: function (value) {
                    this.set("I2AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I12One2Manies", {
                get: function () {
                    return this.canRead("I2I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I12One2Manies", {
                get: function () {
                    return this.canWrite("I2I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I12One2Manies", {
                get: function () {
                    return this.get("I2I12One2Manies");
                },
                set: function (value) {
                    this.set("I2I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I12One2Many = function (value) {
                return this.add("I2I12One2Manies", value);
            };
            C2.prototype.RemoveI2I12One2Many = function (value) {
                return this.remove("I2I12One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2I12One2One", {
                get: function () {
                    return this.canRead("I2I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I12One2One", {
                get: function () {
                    return this.canWrite("I2I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I12One2One", {
                get: function () {
                    return this.get("I2I12One2One");
                },
                set: function (value) {
                    this.set("I2I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2C2Many2Manies", {
                get: function () {
                    return this.canRead("I2C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C2Many2Manies", {
                get: function () {
                    return this.canWrite("I2C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C2Many2Manies", {
                get: function () {
                    return this.get("I2C2Many2Manies");
                },
                set: function (value) {
                    this.set("I2C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2C2Many2Many = function (value) {
                return this.add("I2C2Many2Manies", value);
            };
            C2.prototype.RemoveI2C2Many2Many = function (value) {
                return this.remove("I2C2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2I1Many2Manies", {
                get: function () {
                    return this.canRead("I2I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I1Many2Manies", {
                get: function () {
                    return this.canWrite("I2I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I1Many2Manies", {
                get: function () {
                    return this.get("I2I1Many2Manies");
                },
                set: function (value) {
                    this.set("I2I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I1Many2Many = function (value) {
                return this.add("I2I1Many2Manies", value);
            };
            C2.prototype.RemoveI2I1Many2Many = function (value) {
                return this.remove("I2I1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2C2Many2One", {
                get: function () {
                    return this.canRead("I2C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C2Many2One", {
                get: function () {
                    return this.canWrite("I2C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C2Many2One", {
                get: function () {
                    return this.get("I2C2Many2One");
                },
                set: function (value) {
                    this.set("I2C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsString", {
                get: function () {
                    return this.canRead("I2AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsString", {
                get: function () {
                    return this.canWrite("I2AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsString", {
                get: function () {
                    return this.get("I2AllorsString");
                },
                set: function (value) {
                    this.set("I2AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2C2One2Manies", {
                get: function () {
                    return this.canRead("I2C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C2One2Manies", {
                get: function () {
                    return this.canWrite("I2C2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C2One2Manies", {
                get: function () {
                    return this.get("I2C2One2Manies");
                },
                set: function (value) {
                    this.set("I2C2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2C2One2Many = function (value) {
                return this.add("I2C2One2Manies", value);
            };
            C2.prototype.RemoveI2C2One2Many = function (value) {
                return this.remove("I2C2One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2I1One2One", {
                get: function () {
                    return this.canRead("I2I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I1One2One", {
                get: function () {
                    return this.canWrite("I2I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I1One2One", {
                get: function () {
                    return this.get("I2I1One2One");
                },
                set: function (value) {
                    this.set("I2I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I1One2Manies", {
                get: function () {
                    return this.canRead("I2I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I1One2Manies", {
                get: function () {
                    return this.canWrite("I2I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I1One2Manies", {
                get: function () {
                    return this.get("I2I1One2Manies");
                },
                set: function (value) {
                    this.set("I2I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I1One2Many = function (value) {
                return this.add("I2I1One2Manies", value);
            };
            C2.prototype.RemoveI2I1One2Many = function (value) {
                return this.remove("I2I1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2I12Many2Manies", {
                get: function () {
                    return this.canRead("I2I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I12Many2Manies", {
                get: function () {
                    return this.canWrite("I2I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I12Many2Manies", {
                get: function () {
                    return this.get("I2I12Many2Manies");
                },
                set: function (value) {
                    this.set("I2I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I12Many2Many = function (value) {
                return this.add("I2I12Many2Manies", value);
            };
            C2.prototype.RemoveI2I12Many2Many = function (value) {
                return this.remove("I2I12Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2I2One2One", {
                get: function () {
                    return this.canRead("I2I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I2One2One", {
                get: function () {
                    return this.canWrite("I2I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I2One2One", {
                get: function () {
                    return this.get("I2I2One2One");
                },
                set: function (value) {
                    this.set("I2I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsInteger", {
                get: function () {
                    return this.canRead("I2AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsInteger", {
                get: function () {
                    return this.canWrite("I2AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsInteger", {
                get: function () {
                    return this.get("I2AllorsInteger");
                },
                set: function (value) {
                    this.set("I2AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2I2One2Manies", {
                get: function () {
                    return this.canRead("I2I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2I2One2Manies", {
                get: function () {
                    return this.canWrite("I2I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2I2One2Manies", {
                get: function () {
                    return this.get("I2I2One2Manies");
                },
                set: function (value) {
                    this.set("I2I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2I2One2Many = function (value) {
                return this.add("I2I2One2Manies", value);
            };
            C2.prototype.RemoveI2I2One2Many = function (value) {
                return this.remove("I2I2One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2C1Many2Manies", {
                get: function () {
                    return this.canRead("I2C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C1Many2Manies", {
                get: function () {
                    return this.canWrite("I2C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C1Many2Manies", {
                get: function () {
                    return this.get("I2C1Many2Manies");
                },
                set: function (value) {
                    this.set("I2C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI2C1Many2Many = function (value) {
                return this.add("I2C1Many2Manies", value);
            };
            C2.prototype.RemoveI2C1Many2Many = function (value) {
                return this.remove("I2C1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI2C2One2One", {
                get: function () {
                    return this.canRead("I2C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2C2One2One", {
                get: function () {
                    return this.canWrite("I2C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2C2One2One", {
                get: function () {
                    return this.get("I2C2One2One");
                },
                set: function (value) {
                    this.set("I2C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI2AllorsDouble", {
                get: function () {
                    return this.canRead("I2AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI2AllorsDouble", {
                get: function () {
                    return this.canWrite("I2AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I2AllorsDouble", {
                get: function () {
                    return this.get("I2AllorsDouble");
                },
                set: function (value) {
                    this.set("I2AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsBinary", {
                get: function () {
                    return this.canRead("I12AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsBinary", {
                get: function () {
                    return this.canWrite("I12AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsBinary", {
                get: function () {
                    return this.get("I12AllorsBinary");
                },
                set: function (value) {
                    this.set("I12AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12C2One2One", {
                get: function () {
                    return this.canRead("I12C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C2One2One", {
                get: function () {
                    return this.canWrite("I12C2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C2One2One", {
                get: function () {
                    return this.get("I12C2One2One");
                },
                set: function (value) {
                    this.set("I12C2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsDouble", {
                get: function () {
                    return this.canRead("I12AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsDouble", {
                get: function () {
                    return this.canWrite("I12AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsDouble", {
                get: function () {
                    return this.get("I12AllorsDouble");
                },
                set: function (value) {
                    this.set("I12AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I1Many2One", {
                get: function () {
                    return this.canRead("I12I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I1Many2One", {
                get: function () {
                    return this.canWrite("I12I1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I1Many2One", {
                get: function () {
                    return this.get("I12I1Many2One");
                },
                set: function (value) {
                    this.set("I12I1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsString", {
                get: function () {
                    return this.canRead("I12AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsString", {
                get: function () {
                    return this.canWrite("I12AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsString", {
                get: function () {
                    return this.get("I12AllorsString");
                },
                set: function (value) {
                    this.set("I12AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I12Many2Manies", {
                get: function () {
                    return this.canRead("I12I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I12Many2Manies", {
                get: function () {
                    return this.canWrite("I12I12Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I12Many2Manies", {
                get: function () {
                    return this.get("I12I12Many2Manies");
                },
                set: function (value) {
                    this.set("I12I12Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I12Many2Many = function (value) {
                return this.add("I12I12Many2Manies", value);
            };
            C2.prototype.RemoveI12I12Many2Many = function (value) {
                return this.remove("I12I12Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12AllorsDecimal", {
                get: function () {
                    return this.canRead("I12AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsDecimal", {
                get: function () {
                    return this.canWrite("I12AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsDecimal", {
                get: function () {
                    return this.get("I12AllorsDecimal");
                },
                set: function (value) {
                    this.set("I12AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I2Many2Manies", {
                get: function () {
                    return this.canRead("I12I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I2Many2Manies", {
                get: function () {
                    return this.canWrite("I12I2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I2Many2Manies", {
                get: function () {
                    return this.get("I12I2Many2Manies");
                },
                set: function (value) {
                    this.set("I12I2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I2Many2Many = function (value) {
                return this.add("I12I2Many2Manies", value);
            };
            C2.prototype.RemoveI12I2Many2Many = function (value) {
                return this.remove("I12I2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12C2Many2Manies", {
                get: function () {
                    return this.canRead("I12C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C2Many2Manies", {
                get: function () {
                    return this.canWrite("I12C2Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C2Many2Manies", {
                get: function () {
                    return this.get("I12C2Many2Manies");
                },
                set: function (value) {
                    this.set("I12C2Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12C2Many2Many = function (value) {
                return this.add("I12C2Many2Manies", value);
            };
            C2.prototype.RemoveI12C2Many2Many = function (value) {
                return this.remove("I12C2Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12I1Many2Manies", {
                get: function () {
                    return this.canRead("I12I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I1Many2Manies", {
                get: function () {
                    return this.canWrite("I12I1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I1Many2Manies", {
                get: function () {
                    return this.get("I12I1Many2Manies");
                },
                set: function (value) {
                    this.set("I12I1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I1Many2Many = function (value) {
                return this.add("I12I1Many2Manies", value);
            };
            C2.prototype.RemoveI12I1Many2Many = function (value) {
                return this.remove("I12I1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12I12One2Manies", {
                get: function () {
                    return this.canRead("I12I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I12One2Manies", {
                get: function () {
                    return this.canWrite("I12I12One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I12One2Manies", {
                get: function () {
                    return this.get("I12I12One2Manies");
                },
                set: function (value) {
                    this.set("I12I12One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I12One2Many = function (value) {
                return this.add("I12I12One2Manies", value);
            };
            C2.prototype.RemoveI12I12One2Many = function (value) {
                return this.remove("I12I12One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12C1Many2Manies", {
                get: function () {
                    return this.canRead("I12C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C1Many2Manies", {
                get: function () {
                    return this.canWrite("I12C1Many2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C1Many2Manies", {
                get: function () {
                    return this.get("I12C1Many2Manies");
                },
                set: function (value) {
                    this.set("I12C1Many2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12C1Many2Many = function (value) {
                return this.add("I12C1Many2Manies", value);
            };
            C2.prototype.RemoveI12C1Many2Many = function (value) {
                return this.remove("I12C1Many2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12I2Many2One", {
                get: function () {
                    return this.canRead("I12I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I2Many2One", {
                get: function () {
                    return this.canWrite("I12I2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I2Many2One", {
                get: function () {
                    return this.get("I12I2Many2One");
                },
                set: function (value) {
                    this.set("I12I2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsUnique", {
                get: function () {
                    return this.canRead("I12AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsUnique", {
                get: function () {
                    return this.canWrite("I12AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsUnique", {
                get: function () {
                    return this.get("I12AllorsUnique");
                },
                set: function (value) {
                    this.set("I12AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsInteger", {
                get: function () {
                    return this.canRead("I12AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsInteger", {
                get: function () {
                    return this.canWrite("I12AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsInteger", {
                get: function () {
                    return this.get("I12AllorsInteger");
                },
                set: function (value) {
                    this.set("I12AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I1One2Manies", {
                get: function () {
                    return this.canRead("I12I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I1One2Manies", {
                get: function () {
                    return this.canWrite("I12I1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I1One2Manies", {
                get: function () {
                    return this.get("I12I1One2Manies");
                },
                set: function (value) {
                    this.set("I12I1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I1One2Many = function (value) {
                return this.add("I12I1One2Manies", value);
            };
            C2.prototype.RemoveI12I1One2Many = function (value) {
                return this.remove("I12I1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12C1One2One", {
                get: function () {
                    return this.canRead("I12C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C1One2One", {
                get: function () {
                    return this.canWrite("I12C1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C1One2One", {
                get: function () {
                    return this.get("I12C1One2One");
                },
                set: function (value) {
                    this.set("I12C1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I12One2One", {
                get: function () {
                    return this.canRead("I12I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I12One2One", {
                get: function () {
                    return this.canWrite("I12I12One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I12One2One", {
                get: function () {
                    return this.get("I12I12One2One");
                },
                set: function (value) {
                    this.set("I12I12One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I2One2One", {
                get: function () {
                    return this.canRead("I12I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I2One2One", {
                get: function () {
                    return this.canWrite("I12I2One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I2One2One", {
                get: function () {
                    return this.get("I12I2One2One");
                },
                set: function (value) {
                    this.set("I12I2One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadDependencies", {
                get: function () {
                    return this.canRead("Dependencies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteDependencies", {
                get: function () {
                    return this.canWrite("Dependencies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "Dependencies", {
                get: function () {
                    return this.get("Dependencies");
                },
                set: function (value) {
                    this.set("Dependencies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddDependency = function (value) {
                return this.add("Dependencies", value);
            };
            C2.prototype.RemoveDependency = function (value) {
                return this.remove("Dependencies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12I2One2Manies", {
                get: function () {
                    return this.canRead("I12I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I2One2Manies", {
                get: function () {
                    return this.canWrite("I12I2One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I2One2Manies", {
                get: function () {
                    return this.get("I12I2One2Manies");
                },
                set: function (value) {
                    this.set("I12I2One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12I2One2Many = function (value) {
                return this.add("I12I2One2Manies", value);
            };
            C2.prototype.RemoveI12I2One2Many = function (value) {
                return this.remove("I12I2One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12C2Many2One", {
                get: function () {
                    return this.canRead("I12C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C2Many2One", {
                get: function () {
                    return this.canWrite("I12C2Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C2Many2One", {
                get: function () {
                    return this.get("I12C2Many2One");
                },
                set: function (value) {
                    this.set("I12C2Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I12Many2One", {
                get: function () {
                    return this.canRead("I12I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I12Many2One", {
                get: function () {
                    return this.canWrite("I12I12Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I12Many2One", {
                get: function () {
                    return this.get("I12I12Many2One");
                },
                set: function (value) {
                    this.set("I12I12Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsBoolean", {
                get: function () {
                    return this.canRead("I12AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsBoolean", {
                get: function () {
                    return this.canWrite("I12AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsBoolean", {
                get: function () {
                    return this.get("I12AllorsBoolean");
                },
                set: function (value) {
                    this.set("I12AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12I1One2One", {
                get: function () {
                    return this.canRead("I12I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12I1One2One", {
                get: function () {
                    return this.canWrite("I12I1One2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12I1One2One", {
                get: function () {
                    return this.get("I12I1One2One");
                },
                set: function (value) {
                    this.set("I12I1One2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12C1One2Manies", {
                get: function () {
                    return this.canRead("I12C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C1One2Manies", {
                get: function () {
                    return this.canWrite("I12C1One2Manies");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C1One2Manies", {
                get: function () {
                    return this.get("I12C1One2Manies");
                },
                set: function (value) {
                    this.set("I12C1One2Manies", value);
                },
                enumerable: true,
                configurable: true
            });
            C2.prototype.AddI12C1One2Many = function (value) {
                return this.add("I12C1One2Manies", value);
            };
            C2.prototype.RemoveI12C1One2Many = function (value) {
                return this.remove("I12C1One2Manies", value);
            };
            Object.defineProperty(C2.prototype, "CanReadI12C1Many2One", {
                get: function () {
                    return this.canRead("I12C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12C1Many2One", {
                get: function () {
                    return this.canWrite("I12C1Many2One");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12C1Many2One", {
                get: function () {
                    return this.get("I12C1Many2One");
                },
                set: function (value) {
                    this.set("I12C1Many2One", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanReadI12AllorsDateTime", {
                get: function () {
                    return this.canRead("I12AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "CanWriteI12AllorsDateTime", {
                get: function () {
                    return this.canWrite("I12AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(C2.prototype, "I12AllorsDateTime", {
                get: function () {
                    return this.get("I12AllorsDateTime");
                },
                set: function (value) {
                    this.set("I12AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            return C2;
        }(Allors.SessionObject));
        Domain.C2 = C2;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Counter = /** @class */ (function (_super) {
            __extends(Counter, _super);
            function Counter() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Counter.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Counter.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Counter.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return Counter;
        }(Allors.SessionObject));
        Domain.Counter = Counter;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Country = /** @class */ (function (_super) {
            __extends(Country, _super);
            function Country() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Country.prototype, "CanReadCurrency", {
                get: function () {
                    return this.canRead("Currency");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanWriteCurrency", {
                get: function () {
                    return this.canWrite("Currency");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "Currency", {
                get: function () {
                    return this.get("Currency");
                },
                set: function (value) {
                    this.set("Currency", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanReadIsoCode", {
                get: function () {
                    return this.canRead("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanWriteIsoCode", {
                get: function () {
                    return this.canWrite("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "IsoCode", {
                get: function () {
                    return this.get("IsoCode");
                },
                set: function (value) {
                    this.set("IsoCode", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanReadLocalisedNames", {
                get: function () {
                    return this.canRead("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "CanWriteLocalisedNames", {
                get: function () {
                    return this.canWrite("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Country.prototype, "LocalisedNames", {
                get: function () {
                    return this.get("LocalisedNames");
                },
                set: function (value) {
                    this.set("LocalisedNames", value);
                },
                enumerable: true,
                configurable: true
            });
            Country.prototype.AddLocalisedName = function (value) {
                return this.add("LocalisedNames", value);
            };
            Country.prototype.RemoveLocalisedName = function (value) {
                return this.remove("LocalisedNames", value);
            };
            return Country;
        }(Allors.SessionObject));
        Domain.Country = Country;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Currency = /** @class */ (function (_super) {
            __extends(Currency, _super);
            function Currency() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Currency.prototype, "CanReadIsoCode", {
                get: function () {
                    return this.canRead("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanWriteIsoCode", {
                get: function () {
                    return this.canWrite("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "IsoCode", {
                get: function () {
                    return this.get("IsoCode");
                },
                set: function (value) {
                    this.set("IsoCode", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanReadLocalisedNames", {
                get: function () {
                    return this.canRead("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanWriteLocalisedNames", {
                get: function () {
                    return this.canWrite("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "LocalisedNames", {
                get: function () {
                    return this.get("LocalisedNames");
                },
                set: function (value) {
                    this.set("LocalisedNames", value);
                },
                enumerable: true,
                configurable: true
            });
            Currency.prototype.AddLocalisedName = function (value) {
                return this.add("LocalisedNames", value);
            };
            Currency.prototype.RemoveLocalisedName = function (value) {
                return this.remove("LocalisedNames", value);
            };
            Object.defineProperty(Currency.prototype, "CanReadIsActive", {
                get: function () {
                    return this.canRead("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanWriteIsActive", {
                get: function () {
                    return this.canWrite("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "IsActive", {
                get: function () {
                    return this.get("IsActive");
                },
                set: function (value) {
                    this.set("IsActive", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Currency.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return Currency;
        }(Allors.SessionObject));
        Domain.Currency = Currency;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Data = /** @class */ (function (_super) {
            __extends(Data, _super);
            function Data() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Data.prototype, "CanReadAutocompleteFilter", {
                get: function () {
                    return this.canRead("AutocompleteFilter");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteAutocompleteFilter", {
                get: function () {
                    return this.canWrite("AutocompleteFilter");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "AutocompleteFilter", {
                get: function () {
                    return this.get("AutocompleteFilter");
                },
                set: function (value) {
                    this.set("AutocompleteFilter", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadAutocompleteOptions", {
                get: function () {
                    return this.canRead("AutocompleteOptions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteAutocompleteOptions", {
                get: function () {
                    return this.canWrite("AutocompleteOptions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "AutocompleteOptions", {
                get: function () {
                    return this.get("AutocompleteOptions");
                },
                set: function (value) {
                    this.set("AutocompleteOptions", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadCheckbox", {
                get: function () {
                    return this.canRead("Checkbox");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteCheckbox", {
                get: function () {
                    return this.canWrite("Checkbox");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "Checkbox", {
                get: function () {
                    return this.get("Checkbox");
                },
                set: function (value) {
                    this.set("Checkbox", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadChips", {
                get: function () {
                    return this.canRead("Chips");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteChips", {
                get: function () {
                    return this.canWrite("Chips");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "Chips", {
                get: function () {
                    return this.get("Chips");
                },
                set: function (value) {
                    this.set("Chips", value);
                },
                enumerable: true,
                configurable: true
            });
            Data.prototype.AddChip = function (value) {
                return this.add("Chips", value);
            };
            Data.prototype.RemoveChip = function (value) {
                return this.remove("Chips", value);
            };
            Object.defineProperty(Data.prototype, "CanReadString", {
                get: function () {
                    return this.canRead("String");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteString", {
                get: function () {
                    return this.canWrite("String");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "String", {
                get: function () {
                    return this.get("String");
                },
                set: function (value) {
                    this.set("String", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadDate", {
                get: function () {
                    return this.canRead("Date");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteDate", {
                get: function () {
                    return this.canWrite("Date");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "Date", {
                get: function () {
                    return this.get("Date");
                },
                set: function (value) {
                    this.set("Date", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadDateTime", {
                get: function () {
                    return this.canRead("DateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteDateTime", {
                get: function () {
                    return this.canWrite("DateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "DateTime", {
                get: function () {
                    return this.get("DateTime");
                },
                set: function (value) {
                    this.set("DateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadDateTime2", {
                get: function () {
                    return this.canRead("DateTime2");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteDateTime2", {
                get: function () {
                    return this.canWrite("DateTime2");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "DateTime2", {
                get: function () {
                    return this.get("DateTime2");
                },
                set: function (value) {
                    this.set("DateTime2", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadFile", {
                get: function () {
                    return this.canRead("File");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteFile", {
                get: function () {
                    return this.canWrite("File");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "File", {
                get: function () {
                    return this.get("File");
                },
                set: function (value) {
                    this.set("File", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadMultipleFiles", {
                get: function () {
                    return this.canRead("MultipleFiles");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteMultipleFiles", {
                get: function () {
                    return this.canWrite("MultipleFiles");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "MultipleFiles", {
                get: function () {
                    return this.get("MultipleFiles");
                },
                set: function (value) {
                    this.set("MultipleFiles", value);
                },
                enumerable: true,
                configurable: true
            });
            Data.prototype.AddMultipleFile = function (value) {
                return this.add("MultipleFiles", value);
            };
            Data.prototype.RemoveMultipleFile = function (value) {
                return this.remove("MultipleFiles", value);
            };
            Object.defineProperty(Data.prototype, "CanReadRadioGroup", {
                get: function () {
                    return this.canRead("RadioGroup");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteRadioGroup", {
                get: function () {
                    return this.canWrite("RadioGroup");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "RadioGroup", {
                get: function () {
                    return this.get("RadioGroup");
                },
                set: function (value) {
                    this.set("RadioGroup", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadSlider", {
                get: function () {
                    return this.canRead("Slider");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteSlider", {
                get: function () {
                    return this.canWrite("Slider");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "Slider", {
                get: function () {
                    return this.get("Slider");
                },
                set: function (value) {
                    this.set("Slider", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadSlideToggle", {
                get: function () {
                    return this.canRead("SlideToggle");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteSlideToggle", {
                get: function () {
                    return this.canWrite("SlideToggle");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "SlideToggle", {
                get: function () {
                    return this.get("SlideToggle");
                },
                set: function (value) {
                    this.set("SlideToggle", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanReadTextArea", {
                get: function () {
                    return this.canRead("TextArea");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "CanWriteTextArea", {
                get: function () {
                    return this.canWrite("TextArea");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Data.prototype, "TextArea", {
                get: function () {
                    return this.get("TextArea");
                },
                set: function (value) {
                    this.set("TextArea", value);
                },
                enumerable: true,
                configurable: true
            });
            return Data;
        }(Allors.SessionObject));
        Domain.Data = Data;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Dependent = /** @class */ (function (_super) {
            __extends(Dependent, _super);
            function Dependent() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Dependent.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Dependent.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Dependent;
        }(Allors.SessionObject));
        Domain.Dependent = Dependent;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Gender = /** @class */ (function (_super) {
            __extends(Gender, _super);
            function Gender() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Gender.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanReadLocalisedNames", {
                get: function () {
                    return this.canRead("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanWriteLocalisedNames", {
                get: function () {
                    return this.canWrite("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "LocalisedNames", {
                get: function () {
                    return this.get("LocalisedNames");
                },
                set: function (value) {
                    this.set("LocalisedNames", value);
                },
                enumerable: true,
                configurable: true
            });
            Gender.prototype.AddLocalisedName = function (value) {
                return this.add("LocalisedNames", value);
            };
            Gender.prototype.RemoveLocalisedName = function (value) {
                return this.remove("LocalisedNames", value);
            };
            Object.defineProperty(Gender.prototype, "CanReadIsActive", {
                get: function () {
                    return this.canRead("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanWriteIsActive", {
                get: function () {
                    return this.canWrite("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "IsActive", {
                get: function () {
                    return this.get("IsActive");
                },
                set: function (value) {
                    this.set("IsActive", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Gender.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return Gender;
        }(Allors.SessionObject));
        Domain.Gender = Gender;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Language = /** @class */ (function (_super) {
            __extends(Language, _super);
            function Language() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Language.prototype, "CanReadIsoCode", {
                get: function () {
                    return this.canRead("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanWriteIsoCode", {
                get: function () {
                    return this.canWrite("IsoCode");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "IsoCode", {
                get: function () {
                    return this.get("IsoCode");
                },
                set: function (value) {
                    this.set("IsoCode", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanReadLocalisedNames", {
                get: function () {
                    return this.canRead("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanWriteLocalisedNames", {
                get: function () {
                    return this.canWrite("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "LocalisedNames", {
                get: function () {
                    return this.get("LocalisedNames");
                },
                set: function (value) {
                    this.set("LocalisedNames", value);
                },
                enumerable: true,
                configurable: true
            });
            Language.prototype.AddLocalisedName = function (value) {
                return this.add("LocalisedNames", value);
            };
            Language.prototype.RemoveLocalisedName = function (value) {
                return this.remove("LocalisedNames", value);
            };
            Object.defineProperty(Language.prototype, "CanReadNativeName", {
                get: function () {
                    return this.canRead("NativeName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "CanWriteNativeName", {
                get: function () {
                    return this.canWrite("NativeName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Language.prototype, "NativeName", {
                get: function () {
                    return this.get("NativeName");
                },
                set: function (value) {
                    this.set("NativeName", value);
                },
                enumerable: true,
                configurable: true
            });
            return Language;
        }(Allors.SessionObject));
        Domain.Language = Language;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Locale = /** @class */ (function (_super) {
            __extends(Locale, _super);
            function Locale() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Locale.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "CanReadLanguage", {
                get: function () {
                    return this.canRead("Language");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "CanWriteLanguage", {
                get: function () {
                    return this.canWrite("Language");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "Language", {
                get: function () {
                    return this.get("Language");
                },
                set: function (value) {
                    this.set("Language", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "CanReadCountry", {
                get: function () {
                    return this.canRead("Country");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "CanWriteCountry", {
                get: function () {
                    return this.canWrite("Country");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Locale.prototype, "Country", {
                get: function () {
                    return this.get("Country");
                },
                set: function (value) {
                    this.set("Country", value);
                },
                enumerable: true,
                configurable: true
            });
            return Locale;
        }(Allors.SessionObject));
        Domain.Locale = Locale;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var LocalisedMedia = /** @class */ (function (_super) {
            __extends(LocalisedMedia, _super);
            function LocalisedMedia() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(LocalisedMedia.prototype, "CanReadMedia", {
                get: function () {
                    return this.canRead("Media");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "CanWriteMedia", {
                get: function () {
                    return this.canWrite("Media");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "Media", {
                get: function () {
                    return this.get("Media");
                },
                set: function (value) {
                    this.set("Media", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "CanReadLocale", {
                get: function () {
                    return this.canRead("Locale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "CanWriteLocale", {
                get: function () {
                    return this.canWrite("Locale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "Locale", {
                get: function () {
                    return this.get("Locale");
                },
                set: function (value) {
                    this.set("Locale", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedMedia.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return LocalisedMedia;
        }(Allors.SessionObject));
        Domain.LocalisedMedia = LocalisedMedia;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var LocalisedText = /** @class */ (function (_super) {
            __extends(LocalisedText, _super);
            function LocalisedText() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(LocalisedText.prototype, "CanReadText", {
                get: function () {
                    return this.canRead("Text");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "CanWriteText", {
                get: function () {
                    return this.canWrite("Text");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "Text", {
                get: function () {
                    return this.get("Text");
                },
                set: function (value) {
                    this.set("Text", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "CanReadLocale", {
                get: function () {
                    return this.canRead("Locale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "CanWriteLocale", {
                get: function () {
                    return this.canWrite("Locale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "Locale", {
                get: function () {
                    return this.get("Locale");
                },
                set: function (value) {
                    this.set("Locale", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(LocalisedText.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return LocalisedText;
        }(Allors.SessionObject));
        Domain.LocalisedText = LocalisedText;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Login = /** @class */ (function (_super) {
            __extends(Login, _super);
            function Login() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Login.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Login.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Login;
        }(Allors.SessionObject));
        Domain.Login = Login;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Media = /** @class */ (function (_super) {
            __extends(Media, _super);
            function Media() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Media.prototype, "CanReadRevision", {
                get: function () {
                    return this.canRead("Revision");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "Revision", {
                get: function () {
                    return this.get("Revision");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadMediaContent", {
                get: function () {
                    return this.canRead("MediaContent");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanWriteMediaContent", {
                get: function () {
                    return this.canWrite("MediaContent");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "MediaContent", {
                get: function () {
                    return this.get("MediaContent");
                },
                set: function (value) {
                    this.set("MediaContent", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadInData", {
                get: function () {
                    return this.canRead("InData");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanWriteInData", {
                get: function () {
                    return this.canWrite("InData");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "InData", {
                get: function () {
                    return this.get("InData");
                },
                set: function (value) {
                    this.set("InData", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadInDataUri", {
                get: function () {
                    return this.canRead("InDataUri");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanWriteInDataUri", {
                get: function () {
                    return this.canWrite("InDataUri");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "InDataUri", {
                get: function () {
                    return this.get("InDataUri");
                },
                set: function (value) {
                    this.set("InDataUri", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadFileName", {
                get: function () {
                    return this.canRead("FileName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanWriteFileName", {
                get: function () {
                    return this.canWrite("FileName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "FileName", {
                get: function () {
                    return this.get("FileName");
                },
                set: function (value) {
                    this.set("FileName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadType", {
                get: function () {
                    return this.canRead("Type");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "Type", {
                get: function () {
                    return this.get("Type");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Media.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Media;
        }(Allors.SessionObject));
        Domain.Media = Media;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var MediaContent = /** @class */ (function (_super) {
            __extends(MediaContent, _super);
            function MediaContent() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(MediaContent.prototype, "CanReadType", {
                get: function () {
                    return this.canRead("Type");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "CanWriteType", {
                get: function () {
                    return this.canWrite("Type");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "Type", {
                get: function () {
                    return this.get("Type");
                },
                set: function (value) {
                    this.set("Type", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "CanReadData", {
                get: function () {
                    return this.canRead("Data");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "CanWriteData", {
                get: function () {
                    return this.canWrite("Data");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "Data", {
                get: function () {
                    return this.get("Data");
                },
                set: function (value) {
                    this.set("Data", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(MediaContent.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return MediaContent;
        }(Allors.SessionObject));
        Domain.MediaContent = MediaContent;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Notification = /** @class */ (function (_super) {
            __extends(Notification, _super);
            function Notification() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Notification.prototype, "CanReadConfirmed", {
                get: function () {
                    return this.canRead("Confirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanWriteConfirmed", {
                get: function () {
                    return this.canWrite("Confirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "Confirmed", {
                get: function () {
                    return this.get("Confirmed");
                },
                set: function (value) {
                    this.set("Confirmed", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanReadTitle", {
                get: function () {
                    return this.canRead("Title");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanWriteTitle", {
                get: function () {
                    return this.canWrite("Title");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "Title", {
                get: function () {
                    return this.get("Title");
                },
                set: function (value) {
                    this.set("Title", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanReadDescription", {
                get: function () {
                    return this.canRead("Description");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanWriteDescription", {
                get: function () {
                    return this.canWrite("Description");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "Description", {
                get: function () {
                    return this.get("Description");
                },
                set: function (value) {
                    this.set("Description", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanReadDateCreated", {
                get: function () {
                    return this.canRead("DateCreated");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "DateCreated", {
                get: function () {
                    return this.get("DateCreated");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Notification.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Notification;
        }(Allors.SessionObject));
        Domain.Notification = Notification;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var NotificationList = /** @class */ (function (_super) {
            __extends(NotificationList, _super);
            function NotificationList() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(NotificationList.prototype, "CanReadUnconfirmedNotifications", {
                get: function () {
                    return this.canRead("UnconfirmedNotifications");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(NotificationList.prototype, "UnconfirmedNotifications", {
                get: function () {
                    return this.get("UnconfirmedNotifications");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(NotificationList.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(NotificationList.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return NotificationList;
        }(Allors.SessionObject));
        Domain.NotificationList = NotificationList;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Order = /** @class */ (function (_super) {
            __extends(Order, _super);
            function Order() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Order.prototype, "CanReadCurrentVersion", {
                get: function () {
                    return this.canRead("CurrentVersion");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Order.prototype, "CanWriteCurrentVersion", {
                get: function () {
                    return this.canWrite("CurrentVersion");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Order.prototype, "CurrentVersion", {
                get: function () {
                    return this.get("CurrentVersion");
                },
                set: function (value) {
                    this.set("CurrentVersion", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Order.prototype, "CanReadAllVersions", {
                get: function () {
                    return this.canRead("AllVersions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Order.prototype, "CanWriteAllVersions", {
                get: function () {
                    return this.canWrite("AllVersions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Order.prototype, "AllVersions", {
                get: function () {
                    return this.get("AllVersions");
                },
                set: function (value) {
                    this.set("AllVersions", value);
                },
                enumerable: true,
                configurable: true
            });
            Order.prototype.AddAllVersion = function (value) {
                return this.add("AllVersions", value);
            };
            Order.prototype.RemoveAllVersion = function (value) {
                return this.remove("AllVersions", value);
            };
            return Order;
        }(Allors.SessionObject));
        Domain.Order = Order;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var OrderLine = /** @class */ (function (_super) {
            __extends(OrderLine, _super);
            function OrderLine() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(OrderLine.prototype, "CanReadCurrentVersion", {
                get: function () {
                    return this.canRead("CurrentVersion");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLine.prototype, "CanWriteCurrentVersion", {
                get: function () {
                    return this.canWrite("CurrentVersion");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLine.prototype, "CurrentVersion", {
                get: function () {
                    return this.get("CurrentVersion");
                },
                set: function (value) {
                    this.set("CurrentVersion", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLine.prototype, "CanReadAllVersions", {
                get: function () {
                    return this.canRead("AllVersions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLine.prototype, "CanWriteAllVersions", {
                get: function () {
                    return this.canWrite("AllVersions");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLine.prototype, "AllVersions", {
                get: function () {
                    return this.get("AllVersions");
                },
                set: function (value) {
                    this.set("AllVersions", value);
                },
                enumerable: true,
                configurable: true
            });
            OrderLine.prototype.AddAllVersion = function (value) {
                return this.add("AllVersions", value);
            };
            OrderLine.prototype.RemoveAllVersion = function (value) {
                return this.remove("AllVersions", value);
            };
            return OrderLine;
        }(Allors.SessionObject));
        Domain.OrderLine = OrderLine;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var OrderLineVersion = /** @class */ (function (_super) {
            __extends(OrderLineVersion, _super);
            function OrderLineVersion() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(OrderLineVersion.prototype, "CanReadDerivationTimeStamp", {
                get: function () {
                    return this.canRead("DerivationTimeStamp");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLineVersion.prototype, "CanWriteDerivationTimeStamp", {
                get: function () {
                    return this.canWrite("DerivationTimeStamp");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderLineVersion.prototype, "DerivationTimeStamp", {
                get: function () {
                    return this.get("DerivationTimeStamp");
                },
                set: function (value) {
                    this.set("DerivationTimeStamp", value);
                },
                enumerable: true,
                configurable: true
            });
            return OrderLineVersion;
        }(Allors.SessionObject));
        Domain.OrderLineVersion = OrderLineVersion;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var OrderState = /** @class */ (function (_super) {
            __extends(OrderState, _super);
            function OrderState() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(OrderState.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderState.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderState.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderState.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderState.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderState.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return OrderState;
        }(Allors.SessionObject));
        Domain.OrderState = OrderState;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var OrderVersion = /** @class */ (function (_super) {
            __extends(OrderVersion, _super);
            function OrderVersion() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(OrderVersion.prototype, "CanReadDerivationTimeStamp", {
                get: function () {
                    return this.canRead("DerivationTimeStamp");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderVersion.prototype, "CanWriteDerivationTimeStamp", {
                get: function () {
                    return this.canWrite("DerivationTimeStamp");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(OrderVersion.prototype, "DerivationTimeStamp", {
                get: function () {
                    return this.get("DerivationTimeStamp");
                },
                set: function (value) {
                    this.set("DerivationTimeStamp", value);
                },
                enumerable: true,
                configurable: true
            });
            return OrderVersion;
        }(Allors.SessionObject));
        Domain.OrderVersion = OrderVersion;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Organisation = /** @class */ (function (_super) {
            __extends(Organisation, _super);
            function Organisation() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Organisation.prototype, "CanReadEmployees", {
                get: function () {
                    return this.canRead("Employees");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteEmployees", {
                get: function () {
                    return this.canWrite("Employees");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Employees", {
                get: function () {
                    return this.get("Employees");
                },
                set: function (value) {
                    this.set("Employees", value);
                },
                enumerable: true,
                configurable: true
            });
            Organisation.prototype.AddEmployee = function (value) {
                return this.add("Employees", value);
            };
            Organisation.prototype.RemoveEmployee = function (value) {
                return this.remove("Employees", value);
            };
            Object.defineProperty(Organisation.prototype, "CanReadManager", {
                get: function () {
                    return this.canRead("Manager");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteManager", {
                get: function () {
                    return this.canWrite("Manager");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Manager", {
                get: function () {
                    return this.get("Manager");
                },
                set: function (value) {
                    this.set("Manager", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanReadOwner", {
                get: function () {
                    return this.canRead("Owner");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteOwner", {
                get: function () {
                    return this.canWrite("Owner");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Owner", {
                get: function () {
                    return this.get("Owner");
                },
                set: function (value) {
                    this.set("Owner", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanReadShareholders", {
                get: function () {
                    return this.canRead("Shareholders");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteShareholders", {
                get: function () {
                    return this.canWrite("Shareholders");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Shareholders", {
                get: function () {
                    return this.get("Shareholders");
                },
                set: function (value) {
                    this.set("Shareholders", value);
                },
                enumerable: true,
                configurable: true
            });
            Organisation.prototype.AddShareholder = function (value) {
                return this.add("Shareholders", value);
            };
            Organisation.prototype.RemoveShareholder = function (value) {
                return this.remove("Shareholders", value);
            };
            Object.defineProperty(Organisation.prototype, "CanReadCycleOne", {
                get: function () {
                    return this.canRead("CycleOne");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteCycleOne", {
                get: function () {
                    return this.canWrite("CycleOne");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CycleOne", {
                get: function () {
                    return this.get("CycleOne");
                },
                set: function (value) {
                    this.set("CycleOne", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanReadCycleMany", {
                get: function () {
                    return this.canRead("CycleMany");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteCycleMany", {
                get: function () {
                    return this.canWrite("CycleMany");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CycleMany", {
                get: function () {
                    return this.get("CycleMany");
                },
                set: function (value) {
                    this.set("CycleMany", value);
                },
                enumerable: true,
                configurable: true
            });
            Organisation.prototype.AddCycleMany = function (value) {
                return this.add("CycleMany", value);
            };
            Organisation.prototype.RemoveCycleMany = function (value) {
                return this.remove("CycleMany", value);
            };
            Object.defineProperty(Organisation.prototype, "CanReadOneData", {
                get: function () {
                    return this.canRead("OneData");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteOneData", {
                get: function () {
                    return this.canWrite("OneData");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "OneData", {
                get: function () {
                    return this.get("OneData");
                },
                set: function (value) {
                    this.set("OneData", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanReadManyDatas", {
                get: function () {
                    return this.canRead("ManyDatas");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteManyDatas", {
                get: function () {
                    return this.canWrite("ManyDatas");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "ManyDatas", {
                get: function () {
                    return this.get("ManyDatas");
                },
                set: function (value) {
                    this.set("ManyDatas", value);
                },
                enumerable: true,
                configurable: true
            });
            Organisation.prototype.AddManyData = function (value) {
                return this.add("ManyDatas", value);
            };
            Organisation.prototype.RemoveManyData = function (value) {
                return this.remove("ManyDatas", value);
            };
            Object.defineProperty(Organisation.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanExecuteJustDoIt", {
                get: function () {
                    return this.canExecute("JustDoIt");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "JustDoIt", {
                get: function () {
                    return new Allors.Method(this, "JustDoIt");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanExecuteToggleCanWrite", {
                get: function () {
                    return this.canExecute("ToggleCanWrite");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "ToggleCanWrite", {
                get: function () {
                    return new Allors.Method(this, "ToggleCanWrite");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Organisation.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Organisation;
        }(Allors.SessionObject));
        Domain.Organisation = Organisation;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var PaymentState = /** @class */ (function (_super) {
            __extends(PaymentState, _super);
            function PaymentState() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(PaymentState.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PaymentState.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PaymentState.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PaymentState.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PaymentState.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PaymentState.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return PaymentState;
        }(Allors.SessionObject));
        Domain.PaymentState = PaymentState;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Permission = /** @class */ (function (_super) {
            __extends(Permission, _super);
            function Permission() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Permission.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Permission.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Permission;
        }(Allors.SessionObject));
        Domain.Permission = Permission;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Person = /** @class */ (function (_super) {
            __extends(Person, _super);
            function Person() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Person.prototype, "CanReadFirstName", {
                get: function () {
                    return this.canRead("FirstName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteFirstName", {
                get: function () {
                    return this.canWrite("FirstName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "FirstName", {
                get: function () {
                    return this.get("FirstName");
                },
                set: function (value) {
                    this.set("FirstName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadLastName", {
                get: function () {
                    return this.canRead("LastName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteLastName", {
                get: function () {
                    return this.canWrite("LastName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "LastName", {
                get: function () {
                    return this.get("LastName");
                },
                set: function (value) {
                    this.set("LastName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadMiddleName", {
                get: function () {
                    return this.canRead("MiddleName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteMiddleName", {
                get: function () {
                    return this.canWrite("MiddleName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "MiddleName", {
                get: function () {
                    return this.get("MiddleName");
                },
                set: function (value) {
                    this.set("MiddleName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadBirthDate", {
                get: function () {
                    return this.canRead("BirthDate");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteBirthDate", {
                get: function () {
                    return this.canWrite("BirthDate");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "BirthDate", {
                get: function () {
                    return this.get("BirthDate");
                },
                set: function (value) {
                    this.set("BirthDate", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadFullName", {
                get: function () {
                    return this.canRead("FullName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "FullName", {
                get: function () {
                    return this.get("FullName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadIsStudent", {
                get: function () {
                    return this.canRead("IsStudent");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteIsStudent", {
                get: function () {
                    return this.canWrite("IsStudent");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "IsStudent", {
                get: function () {
                    return this.get("IsStudent");
                },
                set: function (value) {
                    this.set("IsStudent", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadPhoto", {
                get: function () {
                    return this.canRead("Photo");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWritePhoto", {
                get: function () {
                    return this.canWrite("Photo");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "Photo", {
                get: function () {
                    return this.get("Photo");
                },
                set: function (value) {
                    this.set("Photo", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadPictures", {
                get: function () {
                    return this.canRead("Pictures");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWritePictures", {
                get: function () {
                    return this.canWrite("Pictures");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "Pictures", {
                get: function () {
                    return this.get("Pictures");
                },
                set: function (value) {
                    this.set("Pictures", value);
                },
                enumerable: true,
                configurable: true
            });
            Person.prototype.AddPicture = function (value) {
                return this.add("Pictures", value);
            };
            Person.prototype.RemovePicture = function (value) {
                return this.remove("Pictures", value);
            };
            Object.defineProperty(Person.prototype, "CanReadWeight", {
                get: function () {
                    return this.canRead("Weight");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteWeight", {
                get: function () {
                    return this.canWrite("Weight");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "Weight", {
                get: function () {
                    return this.get("Weight");
                },
                set: function (value) {
                    this.set("Weight", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadCycleOne", {
                get: function () {
                    return this.canRead("CycleOne");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteCycleOne", {
                get: function () {
                    return this.canWrite("CycleOne");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CycleOne", {
                get: function () {
                    return this.get("CycleOne");
                },
                set: function (value) {
                    this.set("CycleOne", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadCycleMany", {
                get: function () {
                    return this.canRead("CycleMany");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteCycleMany", {
                get: function () {
                    return this.canWrite("CycleMany");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CycleMany", {
                get: function () {
                    return this.get("CycleMany");
                },
                set: function (value) {
                    this.set("CycleMany", value);
                },
                enumerable: true,
                configurable: true
            });
            Person.prototype.AddCycleMany = function (value) {
                return this.add("CycleMany", value);
            };
            Person.prototype.RemoveCycleMany = function (value) {
                return this.remove("CycleMany", value);
            };
            Object.defineProperty(Person.prototype, "CanReadUserName", {
                get: function () {
                    return this.canRead("UserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteUserName", {
                get: function () {
                    return this.canWrite("UserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "UserName", {
                get: function () {
                    return this.get("UserName");
                },
                set: function (value) {
                    this.set("UserName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadNormalizedUserName", {
                get: function () {
                    return this.canRead("NormalizedUserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteNormalizedUserName", {
                get: function () {
                    return this.canWrite("NormalizedUserName");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "NormalizedUserName", {
                get: function () {
                    return this.get("NormalizedUserName");
                },
                set: function (value) {
                    this.set("NormalizedUserName", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadUserEmail", {
                get: function () {
                    return this.canRead("UserEmail");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteUserEmail", {
                get: function () {
                    return this.canWrite("UserEmail");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "UserEmail", {
                get: function () {
                    return this.get("UserEmail");
                },
                set: function (value) {
                    this.set("UserEmail", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadUserEmailConfirmed", {
                get: function () {
                    return this.canRead("UserEmailConfirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteUserEmailConfirmed", {
                get: function () {
                    return this.canWrite("UserEmailConfirmed");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "UserEmailConfirmed", {
                get: function () {
                    return this.get("UserEmailConfirmed");
                },
                set: function (value) {
                    this.set("UserEmailConfirmed", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadNotificationList", {
                get: function () {
                    return this.canRead("NotificationList");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteNotificationList", {
                get: function () {
                    return this.canWrite("NotificationList");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "NotificationList", {
                get: function () {
                    return this.get("NotificationList");
                },
                set: function (value) {
                    this.set("NotificationList", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Person.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Person;
        }(Allors.SessionObject));
        Domain.Person = Person;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var PreparedExtent = /** @class */ (function (_super) {
            __extends(PreparedExtent, _super);
            function PreparedExtent() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(PreparedExtent.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedExtent.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedExtent.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedExtent.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedExtent.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return PreparedExtent;
        }(Allors.SessionObject));
        Domain.PreparedExtent = PreparedExtent;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var PreparedFetch = /** @class */ (function (_super) {
            __extends(PreparedFetch, _super);
            function PreparedFetch() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(PreparedFetch.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedFetch.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedFetch.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedFetch.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PreparedFetch.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return PreparedFetch;
        }(Allors.SessionObject));
        Domain.PreparedFetch = PreparedFetch;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var PrintDocument = /** @class */ (function (_super) {
            __extends(PrintDocument, _super);
            function PrintDocument() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(PrintDocument.prototype, "CanReadMedia", {
                get: function () {
                    return this.canRead("Media");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PrintDocument.prototype, "CanWriteMedia", {
                get: function () {
                    return this.canWrite("Media");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PrintDocument.prototype, "Media", {
                get: function () {
                    return this.get("Media");
                },
                set: function (value) {
                    this.set("Media", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PrintDocument.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(PrintDocument.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return PrintDocument;
        }(Allors.SessionObject));
        Domain.PrintDocument = PrintDocument;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Role = /** @class */ (function (_super) {
            __extends(Role, _super);
            function Role() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Role.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Role.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Role.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return Role;
        }(Allors.SessionObject));
        Domain.Role = Role;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var SecurityToken = /** @class */ (function (_super) {
            __extends(SecurityToken, _super);
            function SecurityToken() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(SecurityToken.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(SecurityToken.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return SecurityToken;
        }(Allors.SessionObject));
        Domain.SecurityToken = SecurityToken;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var ShipmentState = /** @class */ (function (_super) {
            __extends(ShipmentState, _super);
            function ShipmentState() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(ShipmentState.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ShipmentState.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ShipmentState.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ShipmentState.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ShipmentState.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ShipmentState.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return ShipmentState;
        }(Allors.SessionObject));
        Domain.ShipmentState = ShipmentState;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Singleton = /** @class */ (function (_super) {
            __extends(Singleton, _super);
            function Singleton() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Singleton.prototype, "CanReadDefaultLocale", {
                get: function () {
                    return this.canRead("DefaultLocale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanWriteDefaultLocale", {
                get: function () {
                    return this.canWrite("DefaultLocale");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "DefaultLocale", {
                get: function () {
                    return this.get("DefaultLocale");
                },
                set: function (value) {
                    this.set("DefaultLocale", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanReadAdditionalLocales", {
                get: function () {
                    return this.canRead("AdditionalLocales");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanWriteAdditionalLocales", {
                get: function () {
                    return this.canWrite("AdditionalLocales");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "AdditionalLocales", {
                get: function () {
                    return this.get("AdditionalLocales");
                },
                set: function (value) {
                    this.set("AdditionalLocales", value);
                },
                enumerable: true,
                configurable: true
            });
            Singleton.prototype.AddAdditionalLocale = function (value) {
                return this.add("AdditionalLocales", value);
            };
            Singleton.prototype.RemoveAdditionalLocale = function (value) {
                return this.remove("AdditionalLocales", value);
            };
            Object.defineProperty(Singleton.prototype, "CanReadGuest", {
                get: function () {
                    return this.canRead("Guest");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanWriteGuest", {
                get: function () {
                    return this.canWrite("Guest");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "Guest", {
                get: function () {
                    return this.get("Guest");
                },
                set: function (value) {
                    this.set("Guest", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanReadLogoImage", {
                get: function () {
                    return this.canRead("LogoImage");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "CanWriteLogoImage", {
                get: function () {
                    return this.canWrite("LogoImage");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Singleton.prototype, "LogoImage", {
                get: function () {
                    return this.get("LogoImage");
                },
                set: function (value) {
                    this.set("LogoImage", value);
                },
                enumerable: true,
                configurable: true
            });
            return Singleton;
        }(Allors.SessionObject));
        Domain.Singleton = Singleton;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var TaskAssignment = /** @class */ (function (_super) {
            __extends(TaskAssignment, _super);
            function TaskAssignment() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(TaskAssignment.prototype, "CanReadUser", {
                get: function () {
                    return this.canRead("User");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "CanWriteUser", {
                get: function () {
                    return this.canWrite("User");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "User", {
                get: function () {
                    return this.get("User");
                },
                set: function (value) {
                    this.set("User", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "CanReadTask", {
                get: function () {
                    return this.canRead("Task");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "CanWriteTask", {
                get: function () {
                    return this.canWrite("Task");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "Task", {
                get: function () {
                    return this.get("Task");
                },
                set: function (value) {
                    this.set("Task", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TaskAssignment.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return TaskAssignment;
        }(Allors.SessionObject));
        Domain.TaskAssignment = TaskAssignment;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var Template = /** @class */ (function (_super) {
            __extends(Template, _super);
            function Template() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(Template.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Template.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Template.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Template.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(Template.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return Template;
        }(Allors.SessionObject));
        Domain.Template = Template;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var TemplateType = /** @class */ (function (_super) {
            __extends(TemplateType, _super);
            function TemplateType() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(TemplateType.prototype, "CanReadName", {
                get: function () {
                    return this.canRead("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanWriteName", {
                get: function () {
                    return this.canWrite("Name");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "Name", {
                get: function () {
                    return this.get("Name");
                },
                set: function (value) {
                    this.set("Name", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanReadLocalisedNames", {
                get: function () {
                    return this.canRead("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanWriteLocalisedNames", {
                get: function () {
                    return this.canWrite("LocalisedNames");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "LocalisedNames", {
                get: function () {
                    return this.get("LocalisedNames");
                },
                set: function (value) {
                    this.set("LocalisedNames", value);
                },
                enumerable: true,
                configurable: true
            });
            TemplateType.prototype.AddLocalisedName = function (value) {
                return this.add("LocalisedNames", value);
            };
            TemplateType.prototype.RemoveLocalisedName = function (value) {
                return this.remove("LocalisedNames", value);
            };
            Object.defineProperty(TemplateType.prototype, "CanReadIsActive", {
                get: function () {
                    return this.canRead("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanWriteIsActive", {
                get: function () {
                    return this.canWrite("IsActive");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "IsActive", {
                get: function () {
                    return this.get("IsActive");
                },
                set: function (value) {
                    this.set("IsActive", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "CanExecuteDelete", {
                get: function () {
                    return this.canExecute("Delete");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(TemplateType.prototype, "Delete", {
                get: function () {
                    return new Allors.Method(this, "Delete");
                },
                enumerable: true,
                configurable: true
            });
            return TemplateType;
        }(Allors.SessionObject));
        Domain.TemplateType = TemplateType;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var UnitSample = /** @class */ (function (_super) {
            __extends(UnitSample, _super);
            function UnitSample() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsBinary", {
                get: function () {
                    return this.canRead("AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsBinary", {
                get: function () {
                    return this.canWrite("AllorsBinary");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsBinary", {
                get: function () {
                    return this.get("AllorsBinary");
                },
                set: function (value) {
                    this.set("AllorsBinary", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsDateTime", {
                get: function () {
                    return this.canRead("AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsDateTime", {
                get: function () {
                    return this.canWrite("AllorsDateTime");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsDateTime", {
                get: function () {
                    return this.get("AllorsDateTime");
                },
                set: function (value) {
                    this.set("AllorsDateTime", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsBoolean", {
                get: function () {
                    return this.canRead("AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsBoolean", {
                get: function () {
                    return this.canWrite("AllorsBoolean");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsBoolean", {
                get: function () {
                    return this.get("AllorsBoolean");
                },
                set: function (value) {
                    this.set("AllorsBoolean", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsDouble", {
                get: function () {
                    return this.canRead("AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsDouble", {
                get: function () {
                    return this.canWrite("AllorsDouble");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsDouble", {
                get: function () {
                    return this.get("AllorsDouble");
                },
                set: function (value) {
                    this.set("AllorsDouble", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsInteger", {
                get: function () {
                    return this.canRead("AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsInteger", {
                get: function () {
                    return this.canWrite("AllorsInteger");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsInteger", {
                get: function () {
                    return this.get("AllorsInteger");
                },
                set: function (value) {
                    this.set("AllorsInteger", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsString", {
                get: function () {
                    return this.canRead("AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsString", {
                get: function () {
                    return this.canWrite("AllorsString");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsString", {
                get: function () {
                    return this.get("AllorsString");
                },
                set: function (value) {
                    this.set("AllorsString", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsUnique", {
                get: function () {
                    return this.canRead("AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsUnique", {
                get: function () {
                    return this.canWrite("AllorsUnique");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsUnique", {
                get: function () {
                    return this.get("AllorsUnique");
                },
                set: function (value) {
                    this.set("AllorsUnique", value);
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanReadAllorsDecimal", {
                get: function () {
                    return this.canRead("AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "CanWriteAllorsDecimal", {
                get: function () {
                    return this.canWrite("AllorsDecimal");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UnitSample.prototype, "AllorsDecimal", {
                get: function () {
                    return this.get("AllorsDecimal");
                },
                set: function (value) {
                    this.set("AllorsDecimal", value);
                },
                enumerable: true,
                configurable: true
            });
            return UnitSample;
        }(Allors.SessionObject));
        Domain.UnitSample = UnitSample;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
/// <reference path="../../Core/Workspace/SessionObject.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        var UserGroup = /** @class */ (function (_super) {
            __extends(UserGroup, _super);
            function UserGroup() {
                return _super !== null && _super.apply(this, arguments) || this;
            }
            Object.defineProperty(UserGroup.prototype, "CanReadMembers", {
                get: function () {
                    return this.canRead("Members");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UserGroup.prototype, "CanWriteMembers", {
                get: function () {
                    return this.canWrite("Members");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UserGroup.prototype, "Members", {
                get: function () {
                    return this.get("Members");
                },
                set: function (value) {
                    this.set("Members", value);
                },
                enumerable: true,
                configurable: true
            });
            UserGroup.prototype.AddMember = function (value) {
                return this.add("Members", value);
            };
            UserGroup.prototype.RemoveMember = function (value) {
                return this.remove("Members", value);
            };
            Object.defineProperty(UserGroup.prototype, "CanReadUniqueId", {
                get: function () {
                    return this.canRead("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UserGroup.prototype, "CanWriteUniqueId", {
                get: function () {
                    return this.canWrite("UniqueId");
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(UserGroup.prototype, "UniqueId", {
                get: function () {
                    return this.get("UniqueId");
                },
                set: function (value) {
                    this.set("UniqueId", value);
                },
                enumerable: true,
                configurable: true
            });
            return UserGroup;
        }(Allors.SessionObject));
        Domain.UserGroup = UserGroup;
    })(Domain = Allors.Domain || (Allors.Domain = {}));
})(Allors || (Allors = {}));
// Allors generated file. 
// Do not edit this file, changes will be overwritten.
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        Data.metaPopulation = {
            domains: [
                "Custom",
                "Core"
            ],
            classes: [
                {
                    name: "Counter",
                    interfaces: [
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Singleton",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "DefaultLocale",
                            objectType: "Locale",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AdditionalLocales",
                            objectType: "Locale",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "Guest",
                            objectType: "User",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "LogoImage",
                            objectType: "Media",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Media",
                    interfaces: [
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Revision",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "MediaContent",
                            objectType: "MediaContent",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "InData",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "InDataUri",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "FileName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Type",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "MediaContent",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Type",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Data",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "PrintDocument",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Media",
                            objectType: "Media",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Template",
                    interfaces: [
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "TemplateType",
                    interfaces: [
                        "Enumeration",
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "LocalisedNames",
                            objectType: "LocalisedText",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "IsActive",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "PreparedExtent",
                    interfaces: [
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "PreparedFetch",
                    interfaces: [
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Country",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "Currency",
                            objectType: "Currency",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "IsoCode",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "LocalisedNames",
                            objectType: "LocalisedText",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Currency",
                    interfaces: [
                        "Enumeration",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "IsoCode",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "LocalisedNames",
                            objectType: "LocalisedText",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "IsActive",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Language",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "IsoCode",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "LocalisedNames",
                            objectType: "LocalisedText",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "NativeName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Locale",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Language",
                            objectType: "Language",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Country",
                            objectType: "Country",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "LocalisedMedia",
                    interfaces: [
                        "Localised",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Media",
                            objectType: "Media",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Locale",
                            objectType: "Locale",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "LocalisedText",
                    interfaces: [
                        "Localised",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Text",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Locale",
                            objectType: "Locale",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "AccessControl",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Login",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Permission",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Role",
                    interfaces: [
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "SecurityToken",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "AutomatedAgent",
                    interfaces: [
                        "User",
                    ],
                    roleTypes: [
                        {
                            name: "UserName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "NormalizedUserName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UserEmail",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UserEmailConfirmed",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "NotificationList",
                            objectType: "NotificationList",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Notification",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "Confirmed",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Title",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Description",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "DateCreated",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "NotificationList",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "UnconfirmedNotifications",
                            objectType: "Notification",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Person",
                    interfaces: [
                        "User",
                        "UniquelyIdentifiable",
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "FirstName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "LastName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "MiddleName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "BirthDate",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "FullName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "IsStudent",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Photo",
                            objectType: "Media",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Pictures",
                            objectType: "Media",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "Weight",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "CycleOne",
                            objectType: "Organisation",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "CycleMany",
                            objectType: "Organisation",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "UserName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "NormalizedUserName",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UserEmail",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UserEmailConfirmed",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "NotificationList",
                            objectType: "NotificationList",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "TaskAssignment",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [
                        {
                            name: "User",
                            objectType: "User",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Task",
                            objectType: "Task",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "UserGroup",
                    interfaces: [
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Members",
                            objectType: "User",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "C1",
                    interfaces: [
                        "I1",
                        "I12",
                        "S1",
                    ],
                    roleTypes: [
                        {
                            name: "C1AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsStringMax",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1C2One2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C1I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C1I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1C2One2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I1C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I1AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Dependencies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: [
                        {
                            name: "ClassMethod"
                        }
                    ]
                }, {
                    name: "C2",
                    interfaces: [
                        "I2",
                        "I12",
                    ],
                    roleTypes: [
                        {
                            name: "C2AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2C2One2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "C2C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "C2AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "S1One2One",
                            objectType: "S1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2C2One2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I2C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I2AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C2One2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1Many2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12Many2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I2Many2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C2Many2Manies",
                            objectType: "C2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I1Many2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I12One2Manies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C1Many2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I2Many2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1One2Manies",
                            objectType: "I1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C1One2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12One2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I2One2One",
                            objectType: "I2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Dependencies",
                            objectType: "I12",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12I2One2Manies",
                            objectType: "I2",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C2Many2One",
                            objectType: "C2",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I12Many2One",
                            objectType: "I12",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12I1One2One",
                            objectType: "I1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12C1One2Manies",
                            objectType: "C1",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "I12C1Many2One",
                            objectType: "C1",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "I12AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Data",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "AutocompleteFilter",
                            objectType: "Person",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AutocompleteOptions",
                            objectType: "Person",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Checkbox",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Chips",
                            objectType: "Person",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "String",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Date",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "DateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "DateTime2",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "File",
                            objectType: "Media",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "MultipleFiles",
                            objectType: "Media",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "RadioGroup",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Slider",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "SlideToggle",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "TextArea",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Dependent",
                    interfaces: [
                        "Deletable",
                    ],
                    roleTypes: [],
                    methodTypes: [
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "Gender",
                    interfaces: [
                        "Enumeration",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "LocalisedNames",
                            objectType: "LocalisedText",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "IsActive",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Order",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "CurrentVersion",
                            objectType: "OrderVersion",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllVersions",
                            objectType: "OrderVersion",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "OrderLine",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "CurrentVersion",
                            objectType: "OrderLineVersion",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllVersions",
                            objectType: "OrderLineVersion",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "OrderLineVersion",
                    interfaces: [
                        "Version",
                    ],
                    roleTypes: [
                        {
                            name: "DerivationTimeStamp",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "OrderState",
                    interfaces: [
                        "ObjectState",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "OrderVersion",
                    interfaces: [
                        "Version",
                    ],
                    roleTypes: [
                        {
                            name: "DerivationTimeStamp",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "Organisation",
                    interfaces: [
                        "Deletable",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Employees",
                            objectType: "Person",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "Manager",
                            objectType: "Person",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        {
                            name: "Owner",
                            objectType: "Person",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "Shareholders",
                            objectType: "Person",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "CycleOne",
                            objectType: "Person",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "CycleMany",
                            objectType: "Person",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "OneData",
                            objectType: "Data",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "ManyDatas",
                            objectType: "Data",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: [
                        {
                            name: "JustDoIt"
                        },
                        {
                            name: "ToggleCanWrite"
                        },
                        {
                            name: "Delete"
                        }
                    ]
                }, {
                    name: "PaymentState",
                    interfaces: [
                        "ObjectState",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "ShipmentState",
                    interfaces: [
                        "ObjectState",
                        "UniquelyIdentifiable",
                    ],
                    roleTypes: [
                        {
                            name: "Name",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "UniqueId",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        }
                    ],
                    methodTypes: []
                }, {
                    name: "UnitSample",
                    interfaces: [],
                    roleTypes: [
                        {
                            name: "AllorsBinary",
                            objectType: "Binary",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsDateTime",
                            objectType: "DateTime",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsBoolean",
                            objectType: "Boolean",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsDouble",
                            objectType: "Float",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsInteger",
                            objectType: "Integer",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsString",
                            objectType: "String",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsUnique",
                            objectType: "Unique",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        {
                            name: "AllorsDecimal",
                            objectType: "Decimal",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        }
                    ],
                    methodTypes: []
                },
            ]
        };
    })(Data = Allors.Data || (Allors.Data = {}));
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
/// <reference path="../../Core/Workspace/Domain/extend.ts" />
var Allors;
(function (Allors) {
    var Domain;
    (function (Domain) {
        Domain.extend(Domain.Person, {
            get Fullname() {
                return this.LastName + (this.FirstName ? ", " : "") + this.FirstName;
            },
            set Fullname(value) {
                // Should be present, otherwise typing to fast in a search box causes an error
            },
            toString: function () {
                return this.FirstName;
            }
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
        var jsnlog = JL("Angular");
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
        var AllorsService = /** @class */ (function () {
            function AllorsService($log, $http, $q, $rootScope, $uibModal, toastr) {
                this.$log = $log;
                this.$http = $http;
                this.$q = $q;
                this.$rootScope = $rootScope;
                this.$uibModal = $uibModal;
                this.toastr = toastr;
                this.baseUrl = "";
                var prefix = "/Database/";
                var postfix = "/Pull";
                this.database = new Allors.Database(this.$http, this.$q, prefix, postfix, this.baseUrl);
                this.workspace = new Allors.Workspace(Allors.Data.metaPopulation);
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