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
    var Meta;
    (function (Meta) {
        var Data;
        (function (Data) {
            Data.data = {
                domains: [
                    "Custom",
                    "Core"
                ],
                interfaces: [
                    {
                        id: "9279e337-c658-4086-946d-03c75cdb1ad3",
                        name: "Deletable",
                        plural: "Deletables"
                    },
                    {
                        id: "b7bcc22f-03f0-46fd-b738-4e035921d445",
                        name: "Enumeration",
                        plural: "Enumerations",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                        name: "UniquelyIdentifiable",
                        plural: "UniquelyIdentifiables"
                    },
                    {
                        id: "a6a3c79e-150b-4586-96ea-5ac0e2e638c6",
                        name: "Version",
                        plural: "Versions"
                    },
                    {
                        id: "61207a42-3199-4249-baa4-9dd11dc0f5b1",
                        name: "Printable",
                        plural: "Printables"
                    },
                    {
                        id: "7979a17c-0829-46df-a0d4-1b01775cfaac",
                        name: "Localised",
                        plural: "Localiseds"
                    },
                    {
                        id: "b86d8407-c411-49e4-aae3-64192457c701",
                        name: "ApproveTask",
                        plural: "ApproveTasks",
                        interfaceIds: [
                            "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "f991813f-3146-4431-96d0-554aa2186887",
                        name: "ObjectState",
                        plural: "ObjectStates",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                        name: "Task",
                        plural: "Tasks",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "a0309c3b-6f80-4777-983e-6e69800df5be",
                        name: "User",
                        plural: "Users"
                    },
                    {
                        id: "fbea29c6-6109-4163-a088-9f0b4deac896",
                        name: "WorkItem",
                        plural: "WorkItems"
                    },
                    {
                        id: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                        name: "I1",
                        plural: "I1s",
                        interfaceIds: [
                            "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            "253b0d71-9eaa-4d87-9094-3b549d8446b3"
                        ]
                    },
                    {
                        id: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                        name: "I12",
                        plural: "I12s"
                    },
                    {
                        id: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                        name: "I2",
                        plural: "I2s",
                        interfaceIds: [
                            "b45ec13c-704f-413d-a662-bdc59a17bfe3"
                        ]
                    },
                    {
                        id: "253b0d71-9eaa-4d87-9094-3b549d8446b3",
                        name: "S1",
                        plural: "S1s"
                    }
                ],
                classes: [
                    {
                        id: "0568354f-e3d9-439e-baac-b7dce31b956a",
                        name: "Counter",
                        plural: "Counters",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "313b97a5-328c-4600-9dd2-b5bc146fb13b",
                        name: "Singleton",
                        plural: "Singletons"
                    },
                    {
                        id: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                        name: "Media",
                        plural: "Medias",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "6c20422e-cb3e-4402-bb40-dacaf584405e",
                        name: "MediaContent",
                        plural: "MediaContents",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "6161594b-8acf-4dfa-ae6d-a9bc96040714",
                        name: "PrintDocument",
                        plural: "PrintDocuments",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "93f8b97b-2d9a-42fc-a823-7bdcc5a92203",
                        name: "Template",
                        plural: "Templates",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "bdabb545-3b39-4f91-9d01-a589a5da670e",
                        name: "TemplateType",
                        plural: "TemplateTypes",
                        interfaceIds: [
                            "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "645a4f92-f1f1-41c7-ba76-53a1cc4d2a61",
                        name: "PreparedExtent",
                        plural: "PreparedExtents",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "02c7569c-8f54-4f8d-ac09-1bacd9528f1f",
                        name: "PreparedFetch",
                        plural: "PreparedFetches",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "c22bf60e-6428-4d10-8194-94f7be396f28",
                        name: "Country",
                        plural: "Countries"
                    },
                    {
                        id: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                        name: "Currency",
                        plural: "Currencies",
                        interfaceIds: [
                            "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "4a0eca4b-281f-488d-9c7e-497de882c044",
                        name: "Language",
                        plural: "Languages"
                    },
                    {
                        id: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                        name: "Locale",
                        plural: "Locales"
                    },
                    {
                        id: "2288e1f3-5dc5-458b-9f5e-076f133890c0",
                        name: "LocalisedMedia",
                        plural: "LocalisedMedias",
                        interfaceIds: [
                            "7979a17c-0829-46df-a0d4-1b01775cfaac",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                        name: "LocalisedText",
                        plural: "LocalisedTexts",
                        interfaceIds: [
                            "7979a17c-0829-46df-a0d4-1b01775cfaac",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "c4d93d5e-34c3-4731-9d37-47a8e801d9a8",
                        name: "AccessControl",
                        plural: "AccessControls",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "ad7277a8-eda4-4128-a990-b47fe43d120a",
                        name: "Login",
                        plural: "Logins",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "7fded183-3337-4196-afb0-3266377944bc",
                        name: "Permission",
                        plural: "Permissions",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "af6fe5f4-e5bc-4099-bcd1-97528af6505d",
                        name: "Role",
                        plural: "Roles",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "a53f1aed-0e3f-4c3c-9600-dc579cccf893",
                        name: "SecurityToken",
                        plural: "SecurityTokens",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                        name: "AutomatedAgent",
                        plural: "AutomatedAgents",
                        interfaceIds: [
                            "a0309c3b-6f80-4777-983e-6e69800df5be"
                        ]
                    },
                    {
                        id: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                        name: "Notification",
                        plural: "Notifications",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "b6579993-4ff1-4853-b048-1f8e67419c00",
                        name: "NotificationList",
                        plural: "NotificationLists",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "c799ca62-a554-467d-9aa2-1663293bb37f",
                        name: "Person",
                        plural: "People",
                        interfaceIds: [
                            "a0309c3b-6f80-4777-983e-6e69800df5be",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "4092d0b4-c6f4-4b81-b023-66be3f4c90bd",
                        name: "TaskAssignment",
                        plural: "TaskAssignments",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "60065f5d-a3c2-4418-880d-1026ab607319",
                        name: "UserGroup",
                        plural: "UserGroups",
                        interfaceIds: [
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "7041c691-d896-4628-8f50-1c24f5d03414",
                        name: "C1",
                        plural: "C1s",
                        interfaceIds: [
                            "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            "253b0d71-9eaa-4d87-9094-3b549d8446b3"
                        ]
                    },
                    {
                        id: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                        name: "C2",
                        plural: "C2s",
                        interfaceIds: [
                            "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            "b45ec13c-704f-413d-a662-bdc59a17bfe3"
                        ]
                    },
                    {
                        id: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                        name: "Data",
                        plural: "Datas"
                    },
                    {
                        id: "0cb8d2a7-4566-432f-9882-893b05a77f44",
                        name: "Dependent",
                        plural: "Dependents",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3"
                        ]
                    },
                    {
                        id: "270f0dc8-1bc2-4a42-9617-45e93d5403c8",
                        name: "Gender",
                        plural: "Genders",
                        interfaceIds: [
                            "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "94be4938-77c1-488f-b116-6d4daeffcc8d",
                        name: "Order",
                        plural: "Orders"
                    },
                    {
                        id: "721008c3-c87c-40ab-966b-094e1271ed5f",
                        name: "OrderLine",
                        plural: "OrderLines"
                    },
                    {
                        id: "ba589be8-049b-4107-9e20-fbfec19477c4",
                        name: "OrderLineVersion",
                        plural: "OrderLineVersions",
                        interfaceIds: [
                            "a6a3c79e-150b-4586-96ea-5ac0e2e638c6"
                        ]
                    },
                    {
                        id: "849393ed-cff6-40da-9b4d-483f045f2e99",
                        name: "OrderState",
                        plural: "OrderStates",
                        interfaceIds: [
                            "f991813f-3146-4431-96d0-554aa2186887",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "6a3a9167-9a77-491e-a1c8-ccfe4572afb4",
                        name: "OrderVersion",
                        plural: "OrderVersions",
                        interfaceIds: [
                            "a6a3c79e-150b-4586-96ea-5ac0e2e638c6"
                        ]
                    },
                    {
                        id: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                        name: "Organisation",
                        plural: "Organisations",
                        interfaceIds: [
                            "9279e337-c658-4086-946d-03c75cdb1ad3",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "07e8f845-5ecc-4b42-83ef-bb86e6b10a69",
                        name: "PaymentState",
                        plural: "PaymentStates",
                        interfaceIds: [
                            "f991813f-3146-4431-96d0-554aa2186887",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "ce56a6e9-8e4b-4f40-8676-180f4b0513e2",
                        name: "ShipmentState",
                        plural: "ShipmentStates",
                        interfaceIds: [
                            "f991813f-3146-4431-96d0-554aa2186887",
                            "122ccfe1-f902-44c1-9d6c-6f6a0afa9469"
                        ]
                    },
                    {
                        id: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                        name: "UnitSample",
                        plural: "UnitSamples"
                    }
                ],
                relationTypes: [
                    {
                        id: "9c1634ab-be99-4504-8690-ed4b39fec5bc",
                        associationType: {
                            id: "45a4205d-7c02-40d4-8d97-6d7d59e05def",
                            objectTypeId: "313b97a5-328c-4600-9dd2-b5bc146fb13b",
                            name: "SingletonsWhereDefaultLocale",
                            isOne: false
                        },
                        roleType: {
                            id: "1e051b37-cf30-43ed-a623-dd2928d6d0a3",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            singular: "DefaultLocale",
                            plural: "DefaultLocales",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "9e5a3413-ed33-474f-adf2-149ad5a80719",
                        associationType: {
                            id: "33d5d8b9-3472-48d8-ab1a-83d00d9cb691",
                            objectTypeId: "313b97a5-328c-4600-9dd2-b5bc146fb13b",
                            name: "SingletonWhereAdditionalLocale",
                            isOne: true
                        },
                        roleType: {
                            id: "e75a8956-4d02-49ba-b0cf-747b7a9f350d",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            singular: "AdditionalLocale",
                            plural: "AdditionalLocales",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f16652b0-b712-43d7-8d4e-34a22487514d",
                        associationType: {
                            id: "c92466b5-55ba-496a-8880-2821f32f8f8e",
                            objectTypeId: "313b97a5-328c-4600-9dd2-b5bc146fb13b",
                            name: "SingletonWhereGuest",
                            isOne: true
                        },
                        roleType: {
                            id: "3a12d798-40c3-40e0-ba9f-9d01b1e39e89",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            singular: "Guest",
                            plural: "Guests",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b2166062-84da-449d-b34f-983a0c81bc31",
                        associationType: {
                            id: "22096b27-ed3c-4640-bb60-eb7338a779fb",
                            objectTypeId: "313b97a5-328c-4600-9dd2-b5bc146fb13b",
                            name: "SingletonsWhereLogoImage",
                            isOne: false
                        },
                        roleType: {
                            id: "1e931d15-5137-4c6d-91ed-9cc5c3c95bef",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "LogoImage",
                            plural: "LogoImages",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b74c2159-739a-4f1c-ada7-c2dcc3cdcf83",
                        associationType: {
                            id: "96b21673-f124-4c30-a2f0-df56d29e03f5",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereRevision",
                            isOne: true
                        },
                        roleType: {
                            id: "de0fe224-c40d-469c-bdc5-849a7412efec",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "Revision",
                            plural: "Revisions",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "67082a51-1502-490b-b8db-537799e550bd",
                        associationType: {
                            id: "e8537dcf-1bd7-46c4-a37c-077bee6a78a1",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereMediaContent",
                            isOne: true
                        },
                        roleType: {
                            id: "02fe1ce8-c019-4a40-bd6f-b38d2f47a288",
                            objectTypeId: "6c20422e-cb3e-4402-bb40-dacaf584405e",
                            singular: "MediaContent",
                            plural: "MediaContents",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "18236718-1835-430c-a936-7ec461eee2cf",
                        associationType: {
                            id: "8a79e6c5-4bae-468d-b57c-c7788d3e21e3",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereInData",
                            isOne: true
                        },
                        roleType: {
                            id: "877abdc8-8915-4640-8871-8cef7ef69072",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "InData",
                            plural: "InDatas",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "79b04065-f13b-43b3-b86e-f3adbbaaf0c4",
                        associationType: {
                            id: "287b7291-39f0-43e5-8770-811940e81bae",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereInDataUri",
                            isOne: true
                        },
                        roleType: {
                            id: "ce17bfc7-5a4e-415a-9ae0-fae429cee69c",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "InDataUri",
                            plural: "InDataUris",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ddd6c005-0104-44ca-a19c-1150b8beb4a3",
                        associationType: {
                            id: "4f43b520-404e-436d-a514-71e4aec55ec8",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereFileName",
                            isOne: true
                        },
                        roleType: {
                            id: "4c4ec21c-a3c0-4720-92e0-cf6532000265",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "FileName",
                            plural: "FileNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "29541613-0b16-49ad-8f40-3309a7c7d7b8",
                        associationType: {
                            id: "efb76140-4a2a-4e7f-b51d-c95bca774664",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            name: "MediaWhereType",
                            isOne: true
                        },
                        roleType: {
                            id: "7cfc8b40-5199-4457-bbbd-27a786721465",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Type",
                            plural: "Types",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "890598a9-0be4-49ee-8dd8-3581ee9355e6",
                        associationType: {
                            id: "3cf7f10e-dc56-4a50-95a5-fe7fae0be291",
                            objectTypeId: "6c20422e-cb3e-4402-bb40-dacaf584405e",
                            name: "MediaContentWhereType",
                            isOne: true
                        },
                        roleType: {
                            id: "70823e7d-5829-4db7-99e0-f6c5f2b0e87b",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Type",
                            plural: "Types",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "0756d508-44b7-405e-bf92-bc09e5702e63",
                        associationType: {
                            id: "76e6547b-8dcf-4e69-ae2d-c8f8c33989e9",
                            objectTypeId: "6c20422e-cb3e-4402-bb40-dacaf584405e",
                            name: "MediaContentWhereData",
                            isOne: true
                        },
                        roleType: {
                            id: "85170945-b020-485b-bb6f-c4122992ebfd",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "Data",
                            plural: "Datas",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "4c5c2727-908c-4fb2-9eb5-da31837422fc",
                        associationType: {
                            id: "0e33fc4f-b3d8-4ea8-ad03-e477fa1ad1e8",
                            objectTypeId: "6161594b-8acf-4dfa-ae6d-a9bc96040714",
                            name: "PrintDocumentWhereMedia",
                            isOne: true
                        },
                        roleType: {
                            id: "6fea8bbd-9d58-4ce7-b5ba-b9235fa9194c",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "Media",
                            plural: "Medias",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "62009cef-7424-4ec0-8953-e92b3cd6639d",
                        associationType: {
                            id: "323173ee-385c-4f74-8b78-ff05735460f8",
                            objectTypeId: "c22bf60e-6428-4d10-8194-94f7be396f28",
                            name: "CountriesWhereCurrency",
                            isOne: false
                        },
                        roleType: {
                            id: "4ca5a640-5d9e-4910-95ed-6872c7ea13d2",
                            objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                            singular: "Currency",
                            plural: "Currencies",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f93acc4e-f89e-4610-ada9-e58f21c165bc",
                        associationType: {
                            id: "ea0efe67-89f2-4317-97e7-f0e14358e718",
                            objectTypeId: "c22bf60e-6428-4d10-8194-94f7be396f28",
                            name: "CountryWhereIsoCode",
                            isOne: true
                        },
                        roleType: {
                            id: "4fe997d6-d221-432b-9f09-4f77735c109b",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "IsoCode",
                            plural: "IsoCodes",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "6b9c977f-b394-440e-9781-5d56733b60da",
                        associationType: {
                            id: "6e3532ae-3528-4114-9274-54fc08effd0d",
                            objectTypeId: "c22bf60e-6428-4d10-8194-94f7be396f28",
                            name: "CountryWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "60f1f9a3-13d1-485f-bc77-fda1f9ef1815",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "8236a702-a76d-4bb5-9afd-acacb1508261",
                        associationType: {
                            id: "9b682612-50f9-43f3-abde-4d0cb5156f0d",
                            objectTypeId: "c22bf60e-6428-4d10-8194-94f7be396f28",
                            name: "CountryWhereLocalisedName",
                            isOne: true
                        },
                        roleType: {
                            id: "99c52c13-ef50-4f68-a32f-fef660aa3044",
                            objectTypeId: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                            singular: "LocalisedName",
                            plural: "LocalisedNames",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "294a4bdc-f03a-47a2-a649-419e6b9021a3",
                        associationType: {
                            id: "f9eec7c6-c4cd-4d8c-a5f7-44855328cd7e",
                            objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                            name: "CurrencyWhereIsoCode",
                            isOne: true
                        },
                        roleType: {
                            id: "09d74027-4457-4788-803c-24b857245565",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "IsoCode",
                            plural: "IsoCodes",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "d2a32d9f-21cc-4f9d-b0d3-a9b75da66907",
                        associationType: {
                            id: "6c860e73-d12e-4e35-897e-ed9f8fd8eba0",
                            objectTypeId: "4a0eca4b-281f-488d-9c7e-497de882c044",
                            name: "LanguageWhereIsoCode",
                            isOne: true
                        },
                        roleType: {
                            id: "84f904a6-8dcc-4089-bda6-34325ade6367",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "IsoCode",
                            plural: "IsoCodes",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "be482902-beb5-4a76-8ad0-c1b1c1c0e5c4",
                        associationType: {
                            id: "d3369fa9-afb7-4d5a-b476-3e4d43cce0fd",
                            objectTypeId: "4a0eca4b-281f-488d-9c7e-497de882c044",
                            name: "LanguageWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "308d73b0-1b65-40a9-88f1-288848849c51",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "f091b264-e6b1-4a57-bbfb-8225cbe8190c",
                        associationType: {
                            id: "6650af3b-f537-4c2f-afff-6773552315cd",
                            objectTypeId: "4a0eca4b-281f-488d-9c7e-497de882c044",
                            name: "LanguageWhereLocalisedName",
                            isOne: true
                        },
                        roleType: {
                            id: "5e9fcced-727d-42a2-95e6-a0f9d8be4ec7",
                            objectTypeId: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                            singular: "LocalisedName",
                            plural: "LocalisedNames",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "842cc899-3f37-455a-ae91-51d29d615e69",
                        associationType: {
                            id: "f3c7a07e-a2f3-4a96-91ef-d53ddf009dd4",
                            objectTypeId: "4a0eca4b-281f-488d-9c7e-497de882c044",
                            name: "LanguageWhereNativeName",
                            isOne: true
                        },
                        roleType: {
                            id: "c78e1736-74b4-4574-a365-421dcadf4d4c",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "NativeName",
                            plural: "NativeNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "2a2c6f77-e6a2-4eab-bfe3-5d35a8abd7f7",
                        associationType: {
                            id: "09422255-fa17-41d8-991b-d21d7b37c6c5",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            name: "LocaleWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "647db2b3-997b-4c3a-9ae2-d49b410933c1",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "d8cac34a-9bb2-4190-bd2a-ec0b87e04cf5",
                        associationType: {
                            id: "af501892-3c83-41d1-826b-f5c4cb1de7fe",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            name: "LocalesWhereLanguage",
                            isOne: false
                        },
                        roleType: {
                            id: "ed32b12a-00ad-420b-9dfa-f1c6ce773fcd",
                            objectTypeId: "4a0eca4b-281f-488d-9c7e-497de882c044",
                            singular: "Language",
                            plural: "Languages",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "ea778b77-2929-4ab4-ad99-bf2f970401a9",
                        associationType: {
                            id: "bb5904f5-feb0-47eb-903a-0351d55f0d8c",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            name: "LocalesWhereCountry",
                            isOne: false
                        },
                        roleType: {
                            id: "b2fc6e06-3881-427e-b4cc-8457a65f8076",
                            objectTypeId: "c22bf60e-6428-4d10-8194-94f7be396f28",
                            singular: "Country",
                            plural: "Countries",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "b6ae19ae-76bf-4b84-9cbe-176217d94b9e",
                        associationType: {
                            id: "4c96a64d-6496-412d-915c-77fa9d4da30e",
                            objectTypeId: "2288e1f3-5dc5-458b-9f5e-076f133890c0",
                            name: "LocalisedMediaWhereMedia",
                            isOne: true
                        },
                        roleType: {
                            id: "37fdedb2-4b49-401c-8c5d-d22143691220",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "Media",
                            plural: "Medias",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "50dc85f0-3d22-4bc1-95d9-153674b89f7a",
                        associationType: {
                            id: "accd061b-20b9-4a24-bb2c-c2f7276f43ab",
                            objectTypeId: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                            name: "LocalisedTextWhereText",
                            isOne: true
                        },
                        roleType: {
                            id: "8d3f68e1-fa6e-414f-aa4d-25fcc2c975d6",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Text",
                            plural: "Texts",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "50b1be30-d6a9-49e8-84da-a47647e443f0",
                        associationType: {
                            id: "cb7cc442-b05b-48a5-8696-4baab7aa8cce",
                            objectTypeId: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                            name: "NotificationWhereConfirmed",
                            isOne: true
                        },
                        roleType: {
                            id: "3ee1975d-5019-4043-be5f-65331ef58787",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "Confirmed",
                            plural: "Confirmeds",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "70292962-9e0e-4b57-a710-c8ac34f65b11",
                        associationType: {
                            id: "80e4d1c5-5648-486a-a2ff-3b1690514f20",
                            objectTypeId: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                            name: "NotificationWhereTitle",
                            isOne: true
                        },
                        roleType: {
                            id: "84813900-abe0-4c24-bd2e-5b0d6be5ab6c",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Title",
                            plural: "Titles",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "e83600fc-5411-4c72-9903-80a3741a9165",
                        associationType: {
                            id: "1da1555c-fee6-4084-be37-57a6f58fe591",
                            objectTypeId: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                            name: "NotificationWhereDescription",
                            isOne: true
                        },
                        roleType: {
                            id: "a6f6ed43-b0ab-462f-9be9-fad58d2e8398",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Description",
                            plural: "Descriptions",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "458a8223-9c0f-4475-93c0-82d5cc133f1b",
                        associationType: {
                            id: "8d657749-a823-422b-9e29-041453ccb267",
                            objectTypeId: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                            name: "NotificationWhereDateCreated",
                            isOne: true
                        },
                        roleType: {
                            id: "d100a845-df65-4f06-96b8-dcaeb9c3e205",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DateCreated",
                            plural: "DateCreateds",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: true
                    },
                    {
                        id: "89487904-053e-470f-bcf9-0e01165b0143",
                        associationType: {
                            id: "2d41d7ef-d107-404f-ac9d-fb81105d3ff7",
                            objectTypeId: "b6579993-4ff1-4853-b048-1f8e67419c00",
                            name: "NotificationListWhereUnconfirmedNotification",
                            isOne: true
                        },
                        roleType: {
                            id: "fc089f2e-a625-40f9-bbc0-c9fc05e6e599",
                            objectTypeId: "73dcdc68-7571-4ed1-86db-77c914fe2f62",
                            singular: "UnconfirmedNotification",
                            plural: "UnconfirmedNotifications",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "ed4b710a-fe24-4143-bb96-ed1bd9beae1a",
                        associationType: {
                            id: "1ea9eca4-eed0-4f61-8903-c99feae961ad",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereFirstName",
                            isOne: true
                        },
                        roleType: {
                            id: "f10ea049-6d24-4ca2-8efa-032fcf3000b3",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "FirstName",
                            plural: "FirstNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "8a3e4664-bb40-4208-8e90-a1b5be323f27",
                        associationType: {
                            id: "9b48ff56-afef-4501-ac97-6173731ff2c9",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereLastName",
                            isOne: true
                        },
                        roleType: {
                            id: "ace04ad8-bf64-4fc3-8216-14a720d3105d",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "LastName",
                            plural: "LastNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "eb18bb28-da9c-47b4-a091-2f8f2303dcb6",
                        associationType: {
                            id: "e3a4d7b2-c5f1-4101-9aab-a0135d37a5a5",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereMiddleName",
                            isOne: true
                        },
                        roleType: {
                            id: "a86fc7a6-dedd-4da9-a250-75c9ec730d22",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "MiddleName",
                            plural: "MiddleNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "adf83a86-878d-4148-a9fc-152f56697136",
                        associationType: {
                            id: "b9da077d-bfc7-4b4e-be62-03aded6da22e",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereBirthDate",
                            isOne: true
                        },
                        roleType: {
                            id: "0ffd9c62-efc6-4438-aaa3-755e4c637c21",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "BirthDate",
                            plural: "BirthDates",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "688ebeb9-8a53-4e8d-b284-3faa0a01ef7c",
                        associationType: {
                            id: "8a181cec-7bae-4248-8e24-8abc7e01eea2",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereFullName",
                            isOne: true
                        },
                        roleType: {
                            id: "e431d53c-37ed-4fde-86a9-755f354c1d75",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "FullName",
                            plural: "FullNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "54f11f06-8d3f-4d58-bcdc-d40e6820fdad",
                        associationType: {
                            id: "03a7ffcc-4291-4ae1-a2ab-69f7257fbf04",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereIsStudent",
                            isOne: true
                        },
                        roleType: {
                            id: "abd2a4b3-4b17-48d4-b465-0ffcb5a2664d",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "IsStudent",
                            plural: "IsStudents",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b3ddd2df-8a5a-4747-bd4f-1f1eb37386b3",
                        associationType: {
                            id: "912b48f5-215e-4cc0-a83b-56b74d986608",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PeopleWherePhoto",
                            isOne: false
                        },
                        roleType: {
                            id: "f6624fac-db8e-4fb2-9e86-18021b59d31d",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "Photo",
                            plural: "Photos",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "2e878c18-9df7-4def-8145-983f4a5ccb2d",
                        associationType: {
                            id: "1741b8bb-9a35-4ae1-b562-63531d5a0036",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PeopleWherePicture",
                            isOne: false
                        },
                        roleType: {
                            id: "d19df5e5-a273-4975-a79e-7b63c7ac0450",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "Picture",
                            plural: "Pictures",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "afc32e62-c310-421b-8c1d-6f2b0bb88b54",
                        associationType: {
                            id: "c21ebc52-6b32-4af7-847e-d3d7e1c4defe",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PersonWhereWeight",
                            isOne: true
                        },
                        roleType: {
                            id: "0aab73c3-f997-4dd9-885a-2c1c892adb0e",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "Weight",
                            plural: "Weights",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "5661a98d-a935-4325-9b28-9d86175b1bd6",
                        associationType: {
                            id: "dec66a7b-56f5-4010-a2e7-37e25124bc77",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PeopleWhereCycleOne",
                            isOne: false
                        },
                        roleType: {
                            id: "79ffeed6-e06a-42f4-b12f-d7f7c98b6499",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            singular: "CycleOne",
                            plural: "CycleOnes",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "2eb2af4f-2bf4-475f-bb41-d740197f168e",
                        associationType: {
                            id: "faa1e59e-29ee-4e10-bfe1-94bfbcf238ea",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            name: "PeopleWhereCycleMany",
                            isOne: false
                        },
                        roleType: {
                            id: "7ceea115-23c8-46e2-ba76-1fdb1fa85381",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            singular: "CycleMany",
                            plural: "CycleMany",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "c32c19f1-3f41-4d11-b19d-b8b2aa360166",
                        associationType: {
                            id: "6e08b1d8-f7fa-452d-907a-668bf32702c1",
                            objectTypeId: "4092d0b4-c6f4-4b81-b023-66be3f4c90bd",
                            name: "TaskAssignmentsWhereUser",
                            isOne: false
                        },
                        roleType: {
                            id: "407443f4-5afa-484e-be97-88ef19f00b32",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            singular: "User",
                            plural: "Users",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "8a01f221-480f-4d61-9a12-72e3689a8224",
                        associationType: {
                            id: "e5e52776-c946-42b0-99f4-596ffc1c298f",
                            objectTypeId: "4092d0b4-c6f4-4b81-b023-66be3f4c90bd",
                            name: "TaskAssignmentsWhereTask",
                            isOne: false
                        },
                        roleType: {
                            id: "0be6c69a-1d1c-49d0-8247-fa0ff9a8f223",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            singular: "Task",
                            plural: "Tasks",
                            isUnit: false,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "585bb5cf-9ba4-4865-9027-3667185abc4f",
                        associationType: {
                            id: "1e2d1e31-ed80-4435-8850-7663d9c5f41d",
                            objectTypeId: "60065f5d-a3c2-4418-880d-1026ab607319",
                            name: "UserGroupsWhereMember",
                            isOne: false
                        },
                        roleType: {
                            id: "c552f0b7-95ce-4d45-aaea-56bc8365eee4",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            singular: "Member",
                            plural: "Members",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "97f31053-0e7b-42a0-90c2-ce6f09c56e86",
                        associationType: {
                            id: "70e42b8b-09e2-4cb1-a632-ff3785ee1c8d",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "e5cd692c-ab97-4cf8-9f8a-1de733526e74",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "C1AllorsBinary",
                            plural: "C1AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b4ee673f-bba0-4e24-9cda-3cf993c79a0a",
                        associationType: {
                            id: "948aa9e6-5cb3-48dc-a3b7-3f8770269dae",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "ad456144-a19e-4c89-845b-9391dbc8f372",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "C1AllorsBoolean",
                            plural: "C1AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ef75cc4e-8787-4f1c-ae5c-73577d721467",
                        associationType: {
                            id: "8c8baa81-0c59-485c-b416-c7e6ec972595",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "610129f7-0c35-4649-9f4b-14698d0d1c77",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "C1AllorsDateTime",
                            plural: "C1AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "87eb0d19-73a7-4aae-aeed-66dc9163233c",
                        associationType: {
                            id: "96e8dfaf-3e1e-4c59-88f3-d47be6c96b74",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "43ccd07d-b9c4-465c-b2f9-083a36315e85",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "C1AllorsDecimal",
                            plural: "C1AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f268783d-42ed-41c1-b0b0-b8a60e30a601",
                        associationType: {
                            id: "6ed0694c-a74f-44c3-835b-897f56343576",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "459d20d8-dadd-44e1-aa8a-396e6eab7538",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "C1AllorsDouble",
                            plural: "C1AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f4920d94-8cd0-45b6-be00-f18d377368fd",
                        associationType: {
                            id: "c4202876-b670-4193-a459-3f0376e24c38",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "2687f5be-eebe-4ffb-a8b2-538134cb6f73",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "C1AllorsInteger",
                            plural: "C1AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "20713860-8abd-4d71-8ccc-2b4d1b88bce3",
                        associationType: {
                            id: "974aa133-255b-431f-a15d-b6a126d362b5",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "6dc98925-87a7-4959-8095-90eedef0e9a0",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "C1AllorsString",
                            plural: "C1AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "a64abd21-dadf-483d-9499-d19aa8e33791",
                        associationType: {
                            id: "099e3d39-16b5-431a-853b-942a354c3a52",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereAllorsStringMax",
                            isOne: true
                        },
                        roleType: {
                            id: "c186bb2f-8e19-468d-8a01-561384e5187d",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "AllorsStringMax",
                            plural: "AllorsStringMaxes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "cef13620-b7d7-4bfe-8d3b-c0f826da5989",
                        associationType: {
                            id: "6c18bd8f-9084-470b-9dfe-30263c98771b",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1AllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "2721249b-dadd-410d-b4e0-9d4a48e615d1",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "C1AllorsUnique",
                            plural: "C1AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "8c198447-e943-4f5a-b749-9534b181c664",
                        associationType: {
                            id: "154222cb-0eb8-459d-839c-9c8857bd1c7e",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1C1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "c403f160-6486-4207-b32c-aa9ade27a28c",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C1C1Many2Many",
                            plural: "C1C1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "a8e18ea7-cbf2-4ea7-ae14-9f4bcfdb55de",
                        associationType: {
                            id: "8a546f48-fc09-48ae-997d-4a6de0cd458a",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1C1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "e6b21250-194b-4424-8b92-221c6d0e6228",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C1C1Many2One",
                            plural: "C1C1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "a0ac5a65-2cbd-4c51-9417-b10150bc5699",
                        associationType: {
                            id: "d595765b-5e67-46f2-b19c-c58563dd1ae0",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1C1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "3d121ffa-0ff5-4627-9ec3-879c2830ff04",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C1C1One2Many",
                            plural: "C1C1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "79c00218-bb4f-40e9-af7d-61af444a4a54",
                        associationType: {
                            id: "2276c942-dd96-41a6-b52f-cd3862c4692f",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1C1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "40ee2908-2556-4bdf-a82f-2ea33e181b91",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C1C1One2One",
                            plural: "C1C1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f29d4a52-9ba5-40f6-ba99-050cbd03e554",
                        associationType: {
                            id: "122dc72f-cc92-440c-84e5-fe8340020c43",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1C2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "608db13d-1778-44a8-94f0-b86fc0f6992d",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C1C2Many2Many",
                            plural: "C1C2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "5490dc63-a8f6-4a86-91ef-fef97a86f119",
                        associationType: {
                            id: "3f307d57-1f39-4aba-822d-9881cef7223c",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1C2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "66a06e06-95e4-43ad-9b45-56687f8a2051",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C1C2Many2One",
                            plural: "C1C2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "9f6538c2-e6dd-4c27-80ed-2748f645cb95",
                        associationType: {
                            id: "3ddac067-46f1-4302-bb1b-aa0e05dd55ae",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1C2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "c749e58c-0f1d-4946-b35d-878221aac72f",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C1C2One2Many",
                            plural: "C1C2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "e97fc754-c736-4359-9662-19dce9429f89",
                        associationType: {
                            id: "5bd37271-01c0-4cd3-94d5-0284700b3567",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1C2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "392f5a47-f181-475c-b5c9-f0b729c8413f",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C1C2One2One",
                            plural: "C1C2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "94a2b37d-9431-4496-b992-630cda5b9851",
                        associationType: {
                            id: "a4a31323-7193-4709-828e-88b2c0f3f8aa",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I12Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "f225d708-c98f-44ff-9ed8-847cb1ddaacb",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C1I12Many2Many",
                            plural: "C1I12Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "bcf4df45-6616-4cdf-8ada-f944f9c7ff1a",
                        associationType: {
                            id: "2128418c-6918-4be8-8a02-2bea142b7fc4",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I12Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "b5b4892d-e1d3-4a4b-a8a4-ac6ed0ff930e",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C1I12Many2One",
                            plural: "C1I12Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "98c5f58b-1777-4d9a-8828-37dbf7051510",
                        associationType: {
                            id: "3218ac29-2eac-4dc9-acad-2c708c3df994",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I12One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "51b3b28e-9017-4a1e-b5ba-06a9b14d88d6",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C1I12One2Many",
                            plural: "C1I12One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b9f2c4c7-6979-40cf-82a2-fa99a5d9e9a4",
                        associationType: {
                            id: "911a9327-0237-4254-99a7-afff0d6a0369",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I12One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "50bf56c3-f05f-4172-86e1-aefead4a3a8c",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C1I12One2One",
                            plural: "C1I12One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "815878f6-16f2-42f2-9b24-f394ddf789c2",
                        associationType: {
                            id: "eca51eab-3815-410f-b4c5-f7e2a1318791",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "39f62f9e-52d3-47c5-8fd4-44e91d9b78be",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C1I1Many2Many",
                            plural: "C1I1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "7bb216f2-8e9c-4dcd-890b-579130ab0a8b",
                        associationType: {
                            id: "531e89ab-a295-4f72-8496-cdd0d8605d37",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "8af8fbc6-2f59-4026-9093-5b335dfb8b7f",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C1I1Many2One",
                            plural: "C1I1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "e0656d9a-75a6-4e59-aaa1-3ff03d440059",
                        associationType: {
                            id: "637c5967-fb6c-45d4-81c4-de5559df785f",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "89e4802f-7c61-4deb-a243-f78e79578082",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C1I1One2Many",
                            plural: "C1I1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "0e7f529b-bc91-4a40-a7e7-a17341c6bf5b",
                        associationType: {
                            id: "1d1374c3-a28d-4904-b98a-3a14ceb2f7ea",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "da5ccb42-7878-45a9-9350-17f0f0a52fd4",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C1I1One2One",
                            plural: "C1I1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "cda97972-84c8-48e3-99d8-fd7c99c5dbc9",
                        associationType: {
                            id: "8ef5784c-6f76-431e-b59d-075813ad7863",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "ce5170b0-347a-49b7-9925-a7a5c5eb2c75",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C1I2Many2Many",
                            plural: "C1I2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "d0341bed-2732-4bcb-b1bb-9f9589de5d03",
                        associationType: {
                            id: "dacd7dfa-6650-438d-b564-49fbf89fea8d",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1sWhereC1I2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "2db69dd4-008b-4a17-aba5-6a050f35f7e3",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C1I2Many2One",
                            plural: "C1I2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "82f5fb26-c260-41bc-a784-a2d5e35243bd",
                        associationType: {
                            id: "f5329d84-1301-44ea-85b4-dc7d98554694",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "ca30ba2a-627f-43d1-b467-fe0d7cd015cc",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C1I2One2Many",
                            plural: "C1I2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "6def7988-4bcf-4964-9de6-c6ede41d5e5a",
                        associationType: {
                            id: "75e47fbe-6ce1-4cc1-a20f-51a861df9cc3",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            name: "C1WhereC1I2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "e7d1e28d-69ad-4d3a-b35a-2d0aaacb67db",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C1I2One2One",
                            plural: "C1I2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "07eaa992-322a-40e9-bf2c-aa33b69f54cd",
                        associationType: {
                            id: "172c107a-e140-4462-9a62-5ef91e6ead2a",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "152c92f0-485e-4a28-b321-d6ed3b730fc0",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "C2AllorsDecimal",
                            plural: "C2AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "0c8209e3-b2fc-4c7a-acd2-6b5b8ac89bf4",
                        associationType: {
                            id: "56bb9554-819f-418a-9ce1-a91fa704b371",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2C1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "9292cb86-3e04-4cd4-b3fd-a5af7a5ce538",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C2C1One2One",
                            plural: "C2C1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "12896fc2-c9e9-4a89-b875-0aeb92e298e5",
                        associationType: {
                            id: "781b282e-b86f-4747-9d5e-d0f7c08b0899",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2C2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "f41ddb05-4a96-40fa-859b-8b3d6dfcd86b",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C2C2Many2One",
                            plural: "C2C2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "1444d919-6ca1-4642-8d18-9d955c817581",
                        associationType: {
                            id: "9263c1e7-0cda-4129-a16d-da5351adafcb",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "cc1f2cae-2a5d-4584-aa08-4b03fc2176d2",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "C2AllorsUnique",
                            plural: "C2AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "165cc83e-935d-4d0d-aec7-5da155300086",
                        associationType: {
                            id: "bc437b29-f883-41c1-b36f-20be37bc9b30",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I12Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "b2f83414-aa5c-44fd-a382-56119727785a",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C2I12Many2One",
                            plural: "C2I12Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "1d0c57c9-a3d1-4134-bc7d-7bb587d8250f",
                        associationType: {
                            id: "07c026ad-3515-4df0-bee7-ab61d5a9217d",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I12One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "c0562ba5-0402-44ea-acd0-9e78d20e7576",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C2I12One2One",
                            plural: "C2I12One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "1d98eda7-6dba-43f1-a5ce-44f7ed104cf9",
                        associationType: {
                            id: "cae17f3c-a605-4dce-b38d-01c23eea29df",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "d3e84546-02fc-40be-b550-dbd54cd8a139",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C2I1Many2Many",
                            plural: "C2I1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "262ad367-a52c-4d8b-94e2-b477bb098423",
                        associationType: {
                            id: "31be0ad7-0886-406a-a69f-7e38b4526199",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "c52984df-80f8-4622-84e0-0e9f97cfaff3",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "C2AllorsDouble",
                            plural: "C2AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "2ac55066-c748-4f90-9d0f-1090fe02cc76",
                        associationType: {
                            id: "02a5ac2c-d0ac-482d-abee-117ed7eaa5ba",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "28f373c6-62b6-4f5c-b794-c10138043a63",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C2I1One2Many",
                            plural: "C2I1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "38063edc-271a-410d-b857-807a9100c7b5",
                        associationType: {
                            id: "6bedcc6b-af15-4f27-93e8-4404d23dfd99",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "642f5531-896d-482f-b746-4ecf08f27027",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C2I2One2One",
                            plural: "C2I2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "42f9f4b6-3b35-4168-93cb-35171dbf83f4",
                        associationType: {
                            id: "622f9b4f-efc8-454f-9dd6-884bed5b5f4b",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "f5545dfc-e19a-456a-8469-46708ea5bb68",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "C2AllorsInteger",
                            plural: "C2AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "4a963639-72c3-4e9f-9058-bcfc8fe0bc9e",
                        associationType: {
                            id: "e8c9548b-3d75-4f2b-af4f-f953572c587c",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "a1a975a4-7d1e-4734-962e-2f717386783a",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C2I2Many2Many",
                            plural: "C2I2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "50300577-b5fb-4c16-9ac5-41151543f958",
                        associationType: {
                            id: "1f16f92e-ba99-4553-bd1d-b95ba360468a",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I12Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "6210478c-86e3-4d8c-8e3c-3b29da3175ca",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C2I12Many2Many",
                            plural: "C2I12Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "60680366-4790-4443-a941-b30cd4bd3848",
                        associationType: {
                            id: "8fa68cfd-8e0c-40c6-881b-4ebe88487ae7",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2C2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "bfa632a3-f334-4c92-a1b1-21cfa726ab90",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C2C2One2Many",
                            plural: "C2C2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "61daaaae-dd22-405e-aa98-6321d2f8af04",
                        associationType: {
                            id: "a0291a20-3519-44e6-bb8d-b53682c02c52",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "bff48eef-9e8f-45b7-83ff-7b63dac407f1",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "C2AllorsBoolean",
                            plural: "C2AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "65a246a7-cd78-45eb-90db-39f542e7c6cf",
                        associationType: {
                            id: "eb4f1289-1c6c-4964-a9ba-50f991a96564",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "6ff71b5b-723d-424f-9e2f-fb37bb8118fe",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C2I1Many2One",
                            plural: "C2I1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "67780894-fa62-48ba-8f47-7f54106090cd",
                        associationType: {
                            id: "38cd28ba-c584-4d06-b521-dcc8094c5ed3",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "128eb00f-03fc-432e-bec6-8fcfb265a3a9",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "C2I1One2One",
                            plural: "C2I1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "70600f67-7b18-4b5c-b11e-2ed180c5b2d6",
                        associationType: {
                            id: "a373cb01-731b-48be-a387-d057fdb70684",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2C1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "572738e4-956b-404d-97e8-4bb431ce7692",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C2C1Many2Many",
                            plural: "C2C1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "770eb33c-c8ef-4629-a3a0-20decd92ff62",
                        associationType: {
                            id: "de757393-f81a-413c-897b-a47efd48cc79",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I12One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "8ac9a5cd-35a4-4ca3-a1af-ab3f489c7a52",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "C2I12One2Many",
                            plural: "C2I12One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "7a9129c9-7b6d-4bdd-a630-cfd1392549b7",
                        associationType: {
                            id: "87f7a34c-476f-4670-a670-30451c05842d",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2I2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "19f3caa1-c8d1-4257-b4ad-2f8ccb809524",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C2I2One2Many",
                            plural: "C2I2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "86ad371b-0afd-420b-a855-38ebb3f39f38",
                        associationType: {
                            id: "23f5e29b-c36b-416f-93da-9ef2d79fc0f1",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2C2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "cdf7b6ee-fa50-44a1-9433-d04d61ef3aeb",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C2C2One2One",
                            plural: "C2C2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "9c7cde3f-9b61-4c79-a5d7-afe1067262ce",
                        associationType: {
                            id: "71d6109e-1c04-4598-88fa-f06308beb45b",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "8a96d544-e96f-45b5-aeee-d9381946ff31",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "C2AllorsString",
                            plural: "C2AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "a5315151-aa0f-42a3-9d5b-2c7f2cb92560",
                        associationType: {
                            id: "f2bf51b6-0375-4d77-8881-d4d313d682ef",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2C1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "54dce296-9454-440e-9cf3-1327ea439f0e",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C2C1Many2One",
                            plural: "C2C1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "bc6c7fe0-6501-428c-a929-da87a9f4b885",
                        associationType: {
                            id: "794d2637-293c-49cc-a052-246a779825e9",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2C2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "73d243be-d8d0-42c7-b354-fd9786b4eaaa",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "C2C2Many2Many",
                            plural: "C2C2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ce23482d-3a22-4202-98e7-5934fd9abd2d",
                        associationType: {
                            id: "6d752249-af37-4f22-9e59-bfae9e6537ee",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "6e9490f2-740f-498c-9c0f-d601c76f28ad",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "C2AllorsDateTime",
                            plural: "C2AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "e08d75a9-9b67-4d20-a476-757f8fb17376",
                        associationType: {
                            id: "7d45be10-724e-46c4-8dac-4acdf7f515ad",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2sWhereC2I2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "888cd015-7323-45da-83fe-eb297e8ede51",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "C2I2Many2One",
                            plural: "C2I2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f748949e-de5a-4f2e-85e2-e15516d9bf24",
                        associationType: {
                            id: "92c02837-9e6c-45ad-8772-b97a17afad8c",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2C1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "2c172bc6-a87b-4945-b02f-e00a38eb866d",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "C2C1One2Many",
                            plural: "C2C1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "fa8ad982-9953-47dd-9905-81cc4572300e",
                        associationType: {
                            id: "604eec66-6a75-465b-93d8-349dcbccb2bd",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereC2AllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "e701ac90-488a-476f-9b13-ea361e8ff450",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "C2AllorsBinary",
                            plural: "C2AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "8695d9f6-ae00-4001-9990-851260f3abe7",
                        associationType: {
                            id: "267f5530-584e-40f1-9a90-df5cbd33ecb8",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            name: "C2WhereS1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "5345aada-a434-42d0-930e-1da112711c52",
                            objectTypeId: "253b0d71-9eaa-4d87-9094-3b549d8446b3",
                            singular: "S1One2One",
                            plural: "S1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "36fa4eb8-5ea9-4f56-b5aa-9908ef2b417f",
                        associationType: {
                            id: "c0ca43a1-9c16-42ba-b83b-5e6c72dcb605",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DatasWhereAutocompleteFilter",
                            isOne: false
                        },
                        roleType: {
                            id: "5f26c1a3-bd24-465b-a4f9-d7a5d79a5c80",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "AutocompleteFilter",
                            plural: "AutocompleteFilters",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "c1c4d5d9-eec0-44b5-9317-713e9ab2277e",
                        associationType: {
                            id: "9ed53ba6-6b03-448d-b2e7-42ad045beec3",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DatasWhereAutocompleteOptions",
                            isOne: false
                        },
                        roleType: {
                            id: "4b25dd13-a74d-483c-a0c4-7a5491b9d955",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "AutocompleteOptions",
                            plural: "AutocompleteOptions",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "46964f62-af12-4450-83da-c695c4a0ece8",
                        associationType: {
                            id: "4e112908-e5b4-448c-b6a6-58094165522b",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereCheckbox",
                            isOne: true
                        },
                        roleType: {
                            id: "ba0ea6c5-e62f-487b-b57c-d7412a6bf958",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "Checkbox",
                            plural: "Checkboxes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "7e098b17-2ecb-4d1c-aa73-80684394bd9b",
                        associationType: {
                            id: "d13fdde0-8817-4b13-be41-d54ed349813f",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereChip",
                            isOne: true
                        },
                        roleType: {
                            id: "903f0c58-0867-49d8-b3f7-ea1a6f89ea35",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Chip",
                            plural: "Chips",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "46c310de-8e36-412e-8068-a9d734734e74",
                        associationType: {
                            id: "7f157fb6-8f06-4c71-b0b9-fd3b2e6237ad",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereString",
                            isOne: true
                        },
                        roleType: {
                            id: "3e57db60-d0c5-4748-8095-31fd10a9dd50",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "String",
                            plural: "Strings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "31d0a290-2637-452d-8462-4bbb744e3065",
                        associationType: {
                            id: "0551f665-4510-4cac-ab4e-c4b67b0c6099",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereDate",
                            isOne: true
                        },
                        roleType: {
                            id: "a77403dd-e597-4372-8bc4-61f9f0ba4615",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "Date",
                            plural: "Dates",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "487a0ef5-c987-4064-bf6b-0b7354ec4315",
                        associationType: {
                            id: "49fcdc52-8093-4972-a6e7-0ca9302853f0",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "4285b1d9-5697-4345-b18c-8ef746f82fb5",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DateTime",
                            plural: "DateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "940dad46-78c6-44b3-93a2-4ae0137c2839",
                        associationType: {
                            id: "93c8a47f-1069-4565-9eed-d4d612edf422",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereDateTime2",
                            isOne: true
                        },
                        roleType: {
                            id: "924808ac-861b-4000-89a9-f0d1ec98f8fb",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DateTime2",
                            plural: "DateTime2s",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ba910e25-0d71-43e1-8311-7c9620ac0cde",
                        associationType: {
                            id: "55675b9d-6226-45f1-9de2-ed92263212d9",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DatasWhereFile",
                            isOne: false
                        },
                        roleType: {
                            id: "90fab888-fa1a-4dae-809a-0ac1d4618a30",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "File",
                            plural: "Files",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "68515cce-3e87-4d21-b5e5-2136cc3d4f5c",
                        associationType: {
                            id: "c07cddb3-7d31-416e-b4de-d9197bd5fa25",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DatasWhereMultipleFile",
                            isOne: false
                        },
                        roleType: {
                            id: "65195aef-8a73-40f6-b8df-98a1c1b6b54d",
                            objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                            singular: "MultipleFile",
                            plural: "MultipleFiles",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "3aa7fe12-f9dc-43a8-aca7-3eadaee0d05d",
                        associationType: {
                            id: "c0efde58-8a46-48b1-8742-aedb5970a2e5",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereRadioGroup",
                            isOne: true
                        },
                        roleType: {
                            id: "735f03a0-8f3a-486f-ad7a-55eefd2671e8",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "RadioGroup",
                            plural: "RadioGroups",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "c5061bae-0b3b-474d-abaa-ddad638b8da1",
                        associationType: {
                            id: "d4b26bee-af21-4da4-8c18-4a0e835c2fbd",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereSlider",
                            isOne: true
                        },
                        roleType: {
                            id: "f0dcbf23-fcde-4661-87ec-857d3e983000",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "Slider",
                            plural: "Sliders",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "753e6310-b943-48e8-a9f6-306d2a5db6e4",
                        associationType: {
                            id: "522aa21d-a52b-4f3d-b734-062a63ef4e75",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereSlideToggle",
                            isOne: true
                        },
                        roleType: {
                            id: "73109bbe-1ce4-4677-8cbb-e9375166200a",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "SlideToggle",
                            plural: "SlideToggles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "7b18c411-5414-4e28-a7c1-5749347b673b",
                        associationType: {
                            id: "a964b0d0-3d8d-4d1a-9ebe-36bdf1ac0eb2",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            name: "DataWhereTextArea",
                            isOne: true
                        },
                        roleType: {
                            id: "3af4aa7c-27f4-490c-b0f4-434fb2d981f1",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "TextArea",
                            plural: "TextAreas",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "4058fcba-9323-47c5-b165-a3eed8de70b6",
                        associationType: {
                            id: "7fd58473-6579-4269-a4a1-d1bfae6b3542",
                            objectTypeId: "94be4938-77c1-488f-b116-6d4daeffcc8d",
                            name: "OrderWhereCurrentVersion",
                            isOne: true
                        },
                        roleType: {
                            id: "dab0e0a8-712b-4278-b635-92d367f4d41a",
                            objectTypeId: "6a3a9167-9a77-491e-a1c8-ccfe4572afb4",
                            singular: "CurrentVersion",
                            plural: "CurrentVersions",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "df0e52d4-07b3-45ac-9f36-2c0de9802c2f",
                        associationType: {
                            id: "08a55411-57f6-4015-858d-be9177328319",
                            objectTypeId: "94be4938-77c1-488f-b116-6d4daeffcc8d",
                            name: "OrderWhereAllVersion",
                            isOne: true
                        },
                        roleType: {
                            id: "bf309243-98e3-457d-a396-3e6bcb06de6a",
                            objectTypeId: "6a3a9167-9a77-491e-a1c8-ccfe4572afb4",
                            singular: "AllVersion",
                            plural: "AllVersions",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "55f3d531-c58d-4fa7-b745-9e38d8cec4c6",
                        associationType: {
                            id: "8b5ce991-9cc0-4419-b5a7-e2803f888a8e",
                            objectTypeId: "721008c3-c87c-40ab-966b-094e1271ed5f",
                            name: "OrderLineWhereCurrentVersion",
                            isOne: true
                        },
                        roleType: {
                            id: "7663b87d-f17d-4822-a358-546124622073",
                            objectTypeId: "ba589be8-049b-4107-9e20-fbfec19477c4",
                            singular: "CurrentVersion",
                            plural: "CurrentVersions",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "cfc88b59-87a1-4f9e-abbe-168694ab6cb5",
                        associationType: {
                            id: "2ea46390-f69f-436d-bccc-84bef6cd5997",
                            objectTypeId: "721008c3-c87c-40ab-966b-094e1271ed5f",
                            name: "OrderLineWhereAllVersion",
                            isOne: true
                        },
                        roleType: {
                            id: "03585bb0-e87e-474f-8a76-0644d5c858f4",
                            objectTypeId: "ba589be8-049b-4107-9e20-fbfec19477c4",
                            singular: "AllVersion",
                            plural: "AllVersions",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "49b96f79-c33d-4847-8c64-d50a6adb4985",
                        associationType: {
                            id: "b031ef1a-0102-4b19-b85d-aa9c404596c3",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationWhereEmployee",
                            isOne: true
                        },
                        roleType: {
                            id: "b95c7b34-a295-4600-82c8-826cc2186a00",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Employee",
                            plural: "Employees",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "dbef262d-7184-4b98-8f1f-cf04e884bb92",
                        associationType: {
                            id: "ed76a631-00c4-4753-b3d4-b3a53b9ecf4a",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationWhereManager",
                            isOne: true
                        },
                        roleType: {
                            id: "19de0627-fb1c-4f55-9b65-31d8008d0a48",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Manager",
                            plural: "Managers",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "2cc74901-cda5-4185-bcd8-d51c745a8437",
                        associationType: {
                            id: "896a4589-4caf-4cd2-8365-c4200b12f519",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "baa30557-79ff-406d-b374-9d32519b2de7",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        isDerived: false
                    },
                    {
                        id: "845ff004-516f-4ad5-9870-3d0e966a9f7d",
                        associationType: {
                            id: "3820f65f-0e79-4f30-a973-5d17dca6ad33",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereOwner",
                            isOne: false
                        },
                        roleType: {
                            id: "58d7df91-fbc5-4bcb-9398-a9957949402b",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Owner",
                            plural: "Owners",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "15f33fa4-c878-45a0-b40c-c5214bce350b",
                        associationType: {
                            id: "4fdd9abb-f2e7-4f07-860e-27b4207224bd",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereShareholder",
                            isOne: false
                        },
                        roleType: {
                            id: "45bef644-dfcf-417a-9356-3c1cfbcada1b",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Shareholder",
                            plural: "Shareholders",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "d3db6e8c-9c10-47ba-92b1-45f5ddffa5cc",
                        associationType: {
                            id: "4955ac7f-f840-4f24-b44c-c2d3937d2d44",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereCycleOne",
                            isOne: false
                        },
                        roleType: {
                            id: "9033ae73-83f6-4529-9f81-84fd9d35d597",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "CycleOne",
                            plural: "CycleOnes",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "c6cca1c5-5799-4517-87f5-095da0eeec64",
                        associationType: {
                            id: "6abcd4e2-44a7-46b4-bd98-d052f38b7c50",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereCycleMany",
                            isOne: false
                        },
                        roleType: {
                            id: "e01ace3c-2314-477c-8997-14266d9711e0",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "CycleMany",
                            plural: "CycleMany",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "607c1d85-e722-40bc-a4d6-0c6a7244af6a",
                        associationType: {
                            id: "1afd34a4-f075-4034-92d9-85eddc6998d2",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereOneData",
                            isOne: false
                        },
                        roleType: {
                            id: "fb4834eb-344e-46ed-8d75-bf0c442c7078",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            singular: "OneData",
                            plural: "OneDatas",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "897da15e-c250-441f-8f5c-6f9f3e7870eb",
                        associationType: {
                            id: "3b9b5811-c034-45a1-91dd-2a7c11fc5ec2",
                            objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                            name: "OrganisationsWhereManyData",
                            isOne: false
                        },
                        roleType: {
                            id: "658a5f21-58f2-413f-bea0-de9c3f1f8ab0",
                            objectTypeId: "0e82b155-208c-41fd-b7d0-731eadbb5338",
                            singular: "ManyData",
                            plural: "ManyDatas",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "24771d5b-f920-4820-aff7-ea6391b4a45c",
                        associationType: {
                            id: "fe3aa333-e011-4a1e-85dc-ded48329cf00",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "4d4428fc-bac0-47af-ab5e-7c7b87880206",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "AllorsBinary",
                            plural: "AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "4d6a80f5-0fa7-4867-91f8-37aa92b6707b",
                        associationType: {
                            id: "13f88cf7-aaec-48a1-a896-401df84da34b",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "a462ce40-5885-48c6-b327-7e4c096a99fa",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "AllorsDateTime",
                            plural: "AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "5a788ebe-65e9-4d5e-853a-91bb4addabb5",
                        associationType: {
                            id: "7620281d-3d8a-470a-9258-7a6d1b818b46",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "b5dd13eb-8923-4a66-94df-af5fadb42f1c",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "AllorsBoolean",
                            plural: "AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "74a35820-ef8c-4373-9447-6215ee8279c0",
                        associationType: {
                            id: "e5f7a565-372a-42ed-8da5-ffe6dd599f70",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "4a95fb0d-6849-499e-a140-6c942fb06f4d",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "AllorsDouble",
                            plural: "AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b817ba76-876e-44ea-8e5a-51d552d4045e",
                        associationType: {
                            id: "80683240-71d5-4329-abd0-87c367b44fec",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "07070cb0-6e65-4a00-8754-50cf594ed9e1",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "AllorsInteger",
                            plural: "AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "c724c733-972a-411c-aecb-e865c2628a90",
                        associationType: {
                            id: "e4917fda-a605-4f6f-8f63-579ec688b629",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "f27c150a-ce8d-4ff3-9507-ccb0b91aa0c2",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "AllorsString",
                            plural: "AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ed58ae4c-24e0-4dd1-8b1c-0909df1e0fcd",
                        associationType: {
                            id: "f117e164-ce37-4c12-a79e-38cda962adae",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "25dd4abf-c6da-4739-aed0-8528d1c00b8b",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "AllorsUnique",
                            plural: "AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "f746da51-ea2d-4e22-9ecb-46d4dbc1b084",
                        associationType: {
                            id: "3936ee9b-3bd6-44de-9340-4047749a6c2c",
                            objectTypeId: "4e501cd6-807c-4f10-b60b-acd1d80042cd",
                            name: "UnitSampleWhereAllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "1408cd42-3125-48c7-86d7-4a5f71e75e25",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "AllorsDecimal",
                            plural: "AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "3d3ae4d0-bac6-4645-8a53-3e9f7f9af086",
                        associationType: {
                            id: "004cc333-b8ae-4952-ae13-f2ab80eb018c",
                            objectTypeId: "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            name: "EnumerationWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "5850860d-c772-402f-815b-7634c9a1e697",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "bdabb545-3b39-4f91-9d01-a589a5da670e",
                                isRequired: true
                            },
                            {
                                objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                                isRequired: true
                            },
                            {
                                objectTypeId: "270f0dc8-1bc2-4a42-9617-45e93d5403c8",
                                isRequired: true
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "07e034f1-246a-4115-9662-4c798f31343f",
                        associationType: {
                            id: "bcf428fd-0263-488c-b9ac-963ceca1c972",
                            objectTypeId: "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            name: "EnumerationWhereLocalisedName",
                            isOne: true
                        },
                        roleType: {
                            id: "919fdad7-830e-4b12-b23c-f433951236af",
                            objectTypeId: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                            singular: "LocalisedName",
                            plural: "LocalisedNames",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "bdabb545-3b39-4f91-9d01-a589a5da670e",
                                isRequired: false
                            },
                            {
                                objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                                isRequired: false
                            },
                            {
                                objectTypeId: "270f0dc8-1bc2-4a42-9617-45e93d5403c8",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f57bb62e-77a8-4519-81e6-539d54b71cb7",
                        associationType: {
                            id: "a8993304-52c0-4b53-9982-6caa5675467a",
                            objectTypeId: "b7bcc22f-03f0-46fd-b738-4e035921d445",
                            name: "EnumerationWhereIsActive",
                            isOne: true
                        },
                        roleType: {
                            id: "0c6faf5a-eac9-454c-bd53-3b8409e56d34",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "IsActive",
                            plural: "IsActives",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "bdabb545-3b39-4f91-9d01-a589a5da670e",
                                isRequired: false
                            },
                            {
                                objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                                isRequired: false
                            },
                            {
                                objectTypeId: "270f0dc8-1bc2-4a42-9617-45e93d5403c8",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "e1842d87-8157-40e7-b06e-4375f311f2c3",
                        associationType: {
                            id: "fe413e96-cfcf-4e8d-9f23-0fa4f457fdf1",
                            objectTypeId: "122ccfe1-f902-44c1-9d6c-6f6a0afa9469",
                            name: "UniquelyIdentifiableWhereUniqueId",
                            isOne: true
                        },
                        roleType: {
                            id: "d73fd9a4-13ee-4fa9-8925-d93eca328bf6",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "UniqueId",
                            plural: "UniqueIds",
                            isUnit: true,
                            isOne: true,
                            isRequired: true
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "0568354f-e3d9-439e-baac-b7dce31b956a",
                                isRequired: true
                            },
                            {
                                objectTypeId: "da5b86a3-4f33-4c0d-965d-f4fbc1179374",
                                isRequired: true
                            },
                            {
                                objectTypeId: "93f8b97b-2d9a-42fc-a823-7bdcc5a92203",
                                isRequired: true
                            },
                            {
                                objectTypeId: "645a4f92-f1f1-41c7-ba76-53a1cc4d2a61",
                                isRequired: true
                            },
                            {
                                objectTypeId: "02c7569c-8f54-4f8d-ac09-1bacd9528f1f",
                                isRequired: true
                            },
                            {
                                objectTypeId: "af6fe5f4-e5bc-4099-bcd1-97528af6505d",
                                isRequired: true
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: true
                            },
                            {
                                objectTypeId: "60065f5d-a3c2-4418-880d-1026ab607319",
                                isRequired: true
                            },
                            {
                                objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                                isRequired: true
                            },
                            {
                                objectTypeId: "bdabb545-3b39-4f91-9d01-a589a5da670e",
                                isRequired: true
                            },
                            {
                                objectTypeId: "fd397adf-40b4-4ef8-b449-dd5a24273df3",
                                isRequired: true
                            },
                            {
                                objectTypeId: "270f0dc8-1bc2-4a42-9617-45e93d5403c8",
                                isRequired: true
                            },
                            {
                                objectTypeId: "849393ed-cff6-40da-9b4d-483f045f2e99",
                                isRequired: true
                            },
                            {
                                objectTypeId: "07e8f845-5ecc-4b42-83ef-bb86e6b10a69",
                                isRequired: true
                            },
                            {
                                objectTypeId: "ce56a6e9-8e4b-4f40-8676-180f4b0513e2",
                                isRequired: true
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "adf611c3-047a-4bae-95e3-776022d5ce7b",
                        associationType: {
                            id: "7145b062-aee9-4b30-adb8-c691969c6874",
                            objectTypeId: "a6a3c79e-150b-4586-96ea-5ac0e2e638c6",
                            name: "VersionWhereDerivationTimeStamp",
                            isOne: true
                        },
                        roleType: {
                            id: "b38c700c-7ad9-4962-9f53-35b8aef22e09",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DerivationTimeStamp",
                            plural: "DerivationTimeStamps",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "ba589be8-049b-4107-9e20-fbfec19477c4",
                                isRequired: false
                            },
                            {
                                objectTypeId: "6a3a9167-9a77-491e-a1c8-ccfe4572afb4",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "079c31ba-0d20-4cd7-921c-a1829e226970",
                        associationType: {
                            id: "c98431fe-98ea-44eb-97c4-8d5f2c147424",
                            objectTypeId: "61207a42-3199-4249-baa4-9dd11dc0f5b1",
                            name: "PrintableWherePrintDocument",
                            isOne: true
                        },
                        roleType: {
                            id: "b3ece72c-d62c-4f24-805a-34d7ff21de4f",
                            objectTypeId: "6161594b-8acf-4dfa-ae6d-a9bc96040714",
                            singular: "PrintDocument",
                            plural: "PrintDocuments",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "8c005a4e-5ffe-45fd-b279-778e274f4d83",
                        associationType: {
                            id: "6684d98b-cd43-4612-bf9d-afefe02a0d43",
                            objectTypeId: "7979a17c-0829-46df-a0d4-1b01775cfaac",
                            name: "LocalisedsWhereLocale",
                            isOne: false
                        },
                        roleType: {
                            id: "d43b92ac-9e6f-4238-9625-1e889be054cf",
                            objectTypeId: "45033ae6-85b5-4ced-87ce-02518e6c27fd",
                            singular: "Locale",
                            plural: "Locales",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "2288e1f3-5dc5-458b-9f5e-076f133890c0",
                                isRequired: false
                            },
                            {
                                objectTypeId: "020f5d4d-4a59-4d7b-865a-d72fc70e4d97",
                                isRequired: true
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "a280bf60-2eb7-488a-abf7-f03c9d9197b5",
                        associationType: {
                            id: "33be2d23-16d7-4739-8ef2-42391b0f4bd1",
                            objectTypeId: "b86d8407-c411-49e4-aae3-64192457c701",
                            name: "ApproveTaskWhereComment",
                            isOne: true
                        },
                        roleType: {
                            id: "9f88a8cf-84c1-42cc-be52-1d08597e56fa",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Comment",
                            plural: "Comments",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "b86f9e42-fe10-4302-ab7c-6c6c7d357c39",
                        associationType: {
                            id: "052ec640-3150-458a-99d5-0edce6eb6149",
                            objectTypeId: "f991813f-3146-4431-96d0-554aa2186887",
                            name: "ObjectStateWhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "945cbba6-4b09-4b87-931e-861b147c3823",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "849393ed-cff6-40da-9b4d-483f045f2e99",
                                isRequired: false
                            },
                            {
                                objectTypeId: "07e8f845-5ecc-4b42-83ef-bb86e6b10a69",
                                isRequired: false
                            },
                            {
                                objectTypeId: "ce56a6e9-8e4b-4f40-8676-180f4b0513e2",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f247de73-70fe-47e4-a763-22ee9c68a476",
                        associationType: {
                            id: "2e1ebe97-52d3-46fc-94c2-3203a13856c7",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TasksWhereWorkItem",
                            isOne: false
                        },
                        roleType: {
                            id: "4ca8997f-9232-4c84-8f37-e977071eb316",
                            objectTypeId: "fbea29c6-6109-4163-a088-9f0b4deac896",
                            singular: "WorkItem",
                            plural: "WorkItems",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "0233714e-44a4-4363-8cc4-9d1c1ddd9be5",
                        associationType: {
                            id: "e2870d8c-d314-4a45-855d-88e52c050b0d",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TaskWhereTitle",
                            isOne: true
                        },
                        roleType: {
                            id: "92c2c1cd-9d12-45b6-873d-34215ea9afde",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Title",
                            plural: "Titles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "8ebd9048-a344-417c-bae7-359ca9a74aa1",
                        associationType: {
                            id: "af6cbf34-5f71-498b-a2ec-ef698eeae799",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TaskWhereDateCreated",
                            isOne: true
                        },
                        roleType: {
                            id: "ceba2888-2a6e-4822-881b-1101b48f80f3",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DateCreated",
                            plural: "DateCreateds",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "5ad0b9f5-669c-4b05-8c97-89b59a227da2",
                        associationType: {
                            id: "b3182870-cbe0-4da1-aaeb-804df5a9f869",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TaskWhereDateClosed",
                            isOne: true
                        },
                        roleType: {
                            id: "eacac26b-fea7-49f8-abb6-57d63accd548",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "DateClosed",
                            plural: "DateCloseds",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "55375d57-34b0-43d0-9fac-e9788e1b6cd2",
                        associationType: {
                            id: "0d421578-35fc-4309-b8b6-cda0c9cf948c",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TasksWhereParticipant",
                            isOne: false
                        },
                        roleType: {
                            id: "a7c8f58f-358a-4ae9-9299-0ef560190541",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Participant",
                            plural: "Participants",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "ea8abc59-b625-4d25-85bd-dd04bfe55086",
                        associationType: {
                            id: "90150444-fc18-47fd-a6fd-7740006e64ca",
                            objectTypeId: "84eb0e6e-68e1-478c-a35f-6036d45792be",
                            name: "TasksWherePerformer",
                            isOne: false
                        },
                        roleType: {
                            id: "34320d76-6803-4615-8444-cc3ea8bb0315",
                            objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                            singular: "Performer",
                            plural: "Performers",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: false
                    },
                    {
                        id: "5e8ab257-1a1c-4448-aacc-71dbaaba525b",
                        associationType: {
                            id: "eca7ef36-8928-4116-bfce-1896a685fe8c",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            name: "UserWhereUserName",
                            isOne: true
                        },
                        roleType: {
                            id: "3b7d40a0-18ea-4018-b797-6417723e1890",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "UserName",
                            plural: "UserNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                                isRequired: false
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "7397b596-d8fa-4e3c-8e0e-ea24790fe2e4",
                        associationType: {
                            id: "19cad82c-6538-4c46-aa3f-75c082cc8204",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            name: "UserWhereNormalizedUserName",
                            isOne: true
                        },
                        roleType: {
                            id: "faf89920-880f-4600-baf1-a27a5268444a",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "NormalizedUserName",
                            plural: "NormalizedUserNames",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                                isRequired: false
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "c1ae3652-5854-4b68-9890-a954067767fc",
                        associationType: {
                            id: "111104a2-1181-4958-92f6-6528cef79af7",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            name: "UserWhereUserEmail",
                            isOne: true
                        },
                        roleType: {
                            id: "58e35754-91a9-4956-aa66-ca48d05c7042",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "UserEmail",
                            plural: "UserEmails",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                                isRequired: false
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "0b3b650b-fcd4-4475-b5c4-e2ee4f39b0be",
                        associationType: {
                            id: "c89a8e3f-6f76-41ac-b4dc-839f9080d917",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            name: "UserWhereUserEmailConfirmed",
                            isOne: true
                        },
                        roleType: {
                            id: "1b1409b8-add7-494c-a895-002fc969ac7b",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "UserEmailConfirmed",
                            plural: "UserEmailConfirmeds",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                                isRequired: false
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "bed34563-4ed8-4c6b-88d2-b4199e521d74",
                        associationType: {
                            id: "e678c2f8-5c66-4886-ad21-2be98101f759",
                            objectTypeId: "a0309c3b-6f80-4777-983e-6e69800df5be",
                            name: "UserWhereNotificationList",
                            isOne: true
                        },
                        roleType: {
                            id: "79e9a907-9237-4aab-9340-277d593748f5",
                            objectTypeId: "b6579993-4ff1-4853-b048-1f8e67419c00",
                            singular: "NotificationList",
                            plural: "NotificationLists",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "3587d2e1-c3f6-4c55-a96c-016e0501d99c",
                                isRequired: false
                            },
                            {
                                objectTypeId: "c799ca62-a554-467d-9aa2-1663293bb37f",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "7e6d392b-00e7-4095-8525-d9f4ef8cfaa3",
                        associationType: {
                            id: "b20f1b54-87a4-4fc2-91db-8848d6d40ad1",
                            objectTypeId: "fbea29c6-6109-4163-a088-9f0b4deac896",
                            name: "WorkItemWhereWorkItemDescription",
                            isOne: true
                        },
                        roleType: {
                            id: "cf456f4d-8c76-4bfe-9996-89b660c9b153",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "WorkItemDescription",
                            plural: "WorkItemDescriptions",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        isDerived: true
                    },
                    {
                        id: "06b72534-49a8-4f6d-a991-bc4aaf6f939f",
                        associationType: {
                            id: "854c6ec4-51d4-4d68-bd26-4168c26707de",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "9fd09ce4-3f52-4889-b018-fd9374656e8c",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I1I1Many2One",
                            plural: "I1I1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "0a2895ec-0102-493d-9b94-e12e94b4a403",
                        associationType: {
                            id: "295bfc0e-e123-4ac8-84da-45e8d77b1865",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I12Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "94c8ca3f-45d6-4f70-8b4a-5d469b0ee897",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I1I12Many2Many",
                            plural: "I1I12Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "0acbea28-f8aa-477c-b296-b8976d9b10a5",
                        associationType: {
                            id: "5b4da68a-6aeb-4d5c-8e09-5bef3b1358a9",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "5e8608ed-7987-40d0-a877-a244d6520554",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I1I2Many2Many",
                            plural: "I1I2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "194580f4-e0e3-4b52-b9ba-6020171be4e9",
                        associationType: {
                            id: "39a81eb4-e1bb-45ef-8126-21cf233ba684",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "98017570-bc3b-442b-9e51-b16565fa443c",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I1I2Many2One",
                            plural: "I1I2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "28ceffc2-c776-4a0a-9825-a6d1bcb265dc",
                        associationType: {
                            id: "0287a603-59e5-4241-8b2e-a21698476e67",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "fec573a7-5ab3-4f30-9b50-7d720b4af4b4",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "I1AllorsString",
                            plural: "I1AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "2e85d74a-8d13-4bc0-ae4f-42b305e79373",
                        associationType: {
                            id: "d6ccfcb8-623e-4852-a878-d7cb377af853",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I12Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "ec030f88-1060-4c2b-bda1-d9c5dc4fc9d3",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I1I12Many2One",
                            plural: "I1I12Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "32fc21cc-4be7-4a0e-ac71-df135be95e68",
                        associationType: {
                            id: "e0006bdc-74e2-4067-871c-6f0b53eba5de",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "12824c37-d0d2-4cb9-9481-cad7f5f54976",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "I1AllorsDateTime",
                            plural: "I1AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "39e28141-fd6b-4f49-8884-d5400f6c57ff",
                        associationType: {
                            id: "9118c09c-e8c2-4685-a464-9be9ece2e746",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "a4b456e2-b45f-4398-875b-4ba99ead49fe",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I1I2One2Many",
                            plural: "I1I2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "4506a14b-22f1-41fe-972b-40fab7c6dd31",
                        associationType: {
                            id: "54c659d3-98ff-45e6-b734-bc45f13428d8",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1C2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "d75a5613-4ed9-494f-accf-352d9e115ba9",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I1C2One2Many",
                            plural: "I1C2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "593914b1-af95-4992-9703-2b60f4ea0926",
                        associationType: {
                            id: "ee0f3844-928b-4968-9077-afd255554c8b",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1C1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "bca02f1e-a026-4c0b-9762-1bd52d49b953",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I1C1One2One",
                            plural: "I1C1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "5cb44331-fd8c-4f73-8994-161f702849b6",
                        associationType: {
                            id: "2484aae6-db3b-4795-be76-016b33cbb679",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "c9f9dd15-54b4-4847-8b7e-ac88063804a3",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "I1AllorsInteger",
                            plural: "I1AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "6199e5b4-133d-4d0e-9941-207316164ec8",
                        associationType: {
                            id: "75342efb-659c-43a9-8340-1e110087141c",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1C2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "920f26a7-971a-4771-81b1-af3972c997ff",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I1C2Many2Many",
                            plural: "I1C2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "670c753e-8ea0-40b1-bfc9-7388074191d3",
                        associationType: {
                            id: "b1c6c329-09e3-4b07-8ddf-e6a4fd8d0285",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "6d36c9f9-1426-46a5-8d4f-7275a51c9c17",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I1I1One2Many",
                            plural: "I1I1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "6bb3ba6d-ffc7-4700-9723-c323b9b2d233",
                        associationType: {
                            id: "86623fe9-c7cc-4328-85d9-b0dfce2b9a59",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1I1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "9c64a761-136a-43aa-bef9-6bcd1259d591",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I1I1Many2Many",
                            plural: "I1I1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "6c3d04be-6f95-44b8-863a-245e150e3110",
                        associationType: {
                            id: "e6c314af-d366-4169-b28d-9dc83d694079",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "631a2bdb-ceca-43b2-abb8-9c9ea743c9de",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "I1AllorsBoolean",
                            plural: "I1AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "818b4013-5ef1-4455-9f0d-9a39fa3425bb",
                        associationType: {
                            id: "335902bc-6bfa-4c7b-b52f-9a617c746afd",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "56e68d93-a62f-4090-a93a-8f0f364b08bd",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "I1AllorsDecimal",
                            plural: "I1AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "a51d9d21-40ec-44b9-853d-8c18f54d659d",
                        associationType: {
                            id: "1d785350-3f68-4f8d-86d4-74a0cd8adac7",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I12One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "222d2644-197d-4420-a01a-276b35ad61d1",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I1I12One2One",
                            plural: "I1I12One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "a5761a0e-5c10-407a-bd68-0c4f69d78968",
                        associationType: {
                            id: "b6cf882a-e27a-40e3-9a0d-43ade4d236b6",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "3950129b-6ac5-4eae-b5c2-de12500b0561",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I1I2One2One",
                            plural: "I1I2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b6e0fce0-14fc-46e3-995d-1b6e3699ed96",
                        associationType: {
                            id: "ddc18ebf-0b61-441f-854a-0f964859035e",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1C2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "3899bad1-d563-4f65-85b1-2b274b6a278f",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I1C2One2One",
                            plural: "I1C2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b89092f1-8775-4b6a-99ef-f8626bc770bd",
                        associationType: {
                            id: "d0b99a68-2104-4c4d-ba4c-73d725e406e9",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1C1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "6303d423-6cc4-4933-9546-4b6b39fa0ae4",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I1C1One2Many",
                            plural: "I1C1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b9c67658-4abc-41f3-9434-c8512a482179",
                        associationType: {
                            id: "ba4fa583-b169-4327-a60a-fc0d2c142b3f",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "bbd469af-25f5-47aa-86f6-80d3bba53ce5",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "I1AllorsBinary",
                            plural: "I1AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "bcc9eee6-fa07-4d37-be84-b691bfce24be",
                        associationType: {
                            id: "b6c7354a-4997-4764-826a-0c9989431d3b",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1C1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "7da3b7ea-2e1a-400c-adbf-436d35720ae9",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I1C1Many2Many",
                            plural: "I1C1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "cdb758bf-ecaf-4d99-88fb-58df9258c13c",
                        associationType: {
                            id: "62961c44-f0ab-4edf-9aa7-63312643e6b4",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "e33e809e-bbd3-4ecc-b46e-e233c5c93ce6",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "I1AllorsDouble",
                            plural: "I1AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "e1b13216-7210-4c24-a668-83b40162a21b",
                        associationType: {
                            id: "f14f50da-635f-47d0-9f3d-28364b767637",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "911abf5b-ea84-4ffe-b6fb-558b4af50503",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I1I1One2One",
                            plural: "I1I1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "e3126228-342a-4415-a2e8-d52eceaeaf89",
                        associationType: {
                            id: "202575b6-aaff-46ce-9e8a-e976a8a9d263",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1C1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "2598d7df-a764-4b6e-bf91-5234404b97c2",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I1C1Many2One",
                            plural: "I1C1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "e386cca6-e738-4c37-8bfc-b23057d7a0be",
                        associationType: {
                            id: "a3af5653-20c0-410c-9a6f-160e10e2fe69",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1I12One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "6c708f4b-9fb1-412b-84c8-02f03efede5e",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I1I12One2Many",
                            plural: "I1I12One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "ef1a0a5e-1794-4478-9d0a-517182355206",
                        associationType: {
                            id: "7b80b14e-dd35-4e7c-ba85-ac7860a5dc28",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1sWhereI1C2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "1d51d303-f68b-4dca-9299-a6376e13c90e",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I1C2Many2One",
                            plural: "I1C2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f9d7411e-7993-4e43-a7e2-726f1e44e29c",
                        associationType: {
                            id: "84ae4441-5f83-4196-8439-483311b05055",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            name: "I1WhereI1AllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "5ebf419f-1c7f-46f2-844c-0f54321888ee",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "I1AllorsUnique",
                            plural: "I1AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "042d1311-1c06-4d7c-b68e-eb734f9c7327",
                        associationType: {
                            id: "0d3f0f95-aaa2-47bb-9e2b-654d2747b2b1",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "f7809a25-1b10-4eb0-9309-aeea6efcd7cb",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "I12AllorsBinary",
                            plural: "I12AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "107c212d-cc1c-41b2-9c1d-b40c0102072c",
                        associationType: {
                            id: "0a1b3b66-6bb2-4062-b3bb-991987dd5194",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12C2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "4c448b25-b56c-4486-b0c8-ac04a3249677",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I12C2One2One",
                            plural: "I12C2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "1611cb5d-4676-4e85-bfc5-5572e8ff1138",
                        associationType: {
                            id: "4af20cc8-a610-4493-9420-7cd110cc6755",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "5f2eff86-71bf-480d-a6ad-1c93fc68b08d",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "I12AllorsDouble",
                            plural: "I12AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "167b53c0-644c-467e-9f7c-fcb9415d02c6",
                        associationType: {
                            id: "d039c8f9-217a-46cc-b428-7480d4991e1e",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "2e3dc9b9-3700-4090-bafa-2c60050d52d5",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I12I1Many2One",
                            plural: "I12I1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "199a84c4-c7cb-4f23-8b6c-078b14525e18",
                        associationType: {
                            id: "65ed1ff6-eb81-410d-8817-62d61765408a",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "c778c7a7-9cf7-4a7e-8408-e4eb1ca94ce8",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "I12AllorsString",
                            plural: "I12AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "1bf2abe0-9273-4fb9-b491-020320f1f8db",
                        associationType: {
                            id: "732fc964-194e-4ece-bd39-bb3c47b83ff9",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I12Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "b311c57d-9565-48c1-80d8-1d3cf5a498ea",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I12I12Many2Many",
                            plural: "I12I12Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "41a74fec-cfbc-43ca-a6e7-890f0dd1eddb",
                        associationType: {
                            id: "7293e939-ad0b-4b62-935d-44a5309f2515",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "295a4e46-3133-4aff-a1dc-5101e584fb8a",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "I12AllorsDecimal",
                            plural: "I12AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "4a2b2f43-037d-4149-8a1e-401e5df963ba",
                        associationType: {
                            id: "cd90d290-95da-4137-aaf1-bcb59f10e9cb",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "f266759c-34c5-49a8-8d92-e2bbcb41c86a",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I12I2Many2Many",
                            plural: "I12I2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "51ebb024-c847-4165-b216-b3b6e8883961",
                        associationType: {
                            id: "04bca123-7c45-43f4-a5ed-8691b0cbb0e3",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12C2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "f5928b47-5a57-4be8-a0a9-a729e8e467bb",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I12C2Many2Many",
                            plural: "I12C2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "59ae05e3-573c-4ea4-9181-2c545236ed1e",
                        associationType: {
                            id: "064f5e1b-b5c8-45ee-baf1-094f6a723ede",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "397b339e-0277-4700-a5d1-d9d0ac23c362",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I12I1Many2Many",
                            plural: "I12I1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "5e473f63-b1d7-4530-b64f-26435fb5063c",
                        associationType: {
                            id: "83e23750-52eb-4b3f-a675-bfe32570357b",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I12One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "d786aeb4-03bb-419a-90c9-e6ddaa940e93",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I12I12One2Many",
                            plural: "I12I12One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "6daafb16-1bc3-4f15-8e25-1a982c5bb3c5",
                        associationType: {
                            id: "d39d3782-71a6-4b63-aaeb-0a6da0db153d",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereName",
                            isOne: true
                        },
                        roleType: {
                            id: "a89707e2-e3e1-4d24-9c56-180671e3409c",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "Name",
                            plural: "Names",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "7827af95-147f-4803-865a-b418d567da68",
                        associationType: {
                            id: "7e707f89-6dd2-44a4-8f85-e00666af4d00",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12C1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "a4c1f678-a3ae-4707-81e9-b3f3411a5d93",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I12C1Many2Many",
                            plural: "I12C1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "7f6fdb73-3e19-40e7-8feb-6ddbdf2e745a",
                        associationType: {
                            id: "644f55c6-8d39-4602-89bb-5797c9c8e1fd",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "2073096f-8918-4432-8fa2-42f4fd1a53a1",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I12I2Many2One",
                            plural: "I12I2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "93a59d0a-278d-435b-967e-551523f0cb85",
                        associationType: {
                            id: "9c700ad0-e33e-4731-ac3a-4063c2da655b",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "839c7aaa-f044-4b93-97aa-00beeed8f3eb",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "I12AllorsUnique",
                            plural: "I12AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "95551e3a-bad2-4136-923f-c8e5f0f2aec7",
                        associationType: {
                            id: "f57afc9e-3832-4ae1-b3a0-659d7f00604c",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "cbd73ad2-a4cd-4b65-a3cd-55bb7c6f52bc",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "I12AllorsInteger",
                            plural: "I12AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "95c77a0f-7f4c-4142-a93f-f688cfd554af",
                        associationType: {
                            id: "870af1ab-075f-4e19-a283-6e6875d362bb",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "29f38fb4-8e6a-4f70-9ee9-f6819b9d759e",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I12I1One2Many",
                            plural: "I12I1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "9aefdda0-e547-4c9b-bf28-431669f8ea2e",
                        associationType: {
                            id: "f4399c8b-3394-4c2a-9ff0-16b2ece87fdf",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12C1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "ee9379c4-ef6a-4c6e-8190-bc71c36ac009",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I12C1One2One",
                            plural: "I12C1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "a89b4c06-bba5-4b05-bd6f-c32bc195c32f",
                        associationType: {
                            id: "8dd3e2b6-805f-4c93-98d8-4864e6807760",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I12One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "e68fba09-6113-4b49-a6fa-a09e46a004f1",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I12I12One2One",
                            plural: "I12I12One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "ac920d1d-290b-484b-9283-3829337182bc",
                        associationType: {
                            id: "991e5b73-a9b0-40a4-8230-b3fb7cc46761",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "07702752-2c97-4b44-8c43-7c1f2a5e3d0d",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I12I2One2One",
                            plural: "I12I2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b2e3ddda-0cc3-4cfd-a114-9040882ec58a",
                        associationType: {
                            id: "014cf60e-595f-42d5-9146-e7d860396f4d",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereDependency",
                            isOne: false
                        },
                        roleType: {
                            id: "d5c22b99-6984-4042-98fd-93fe60dfe5d7",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "Dependency",
                            plural: "Dependencies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b2f568a1-51ba-4b6b-a1f1-b82bdec382b5",
                        associationType: {
                            id: "6f37656a-21d0-4574-8eac-5342f7c6850d",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "09a2a7a1-4713-4c5c-828d-8be40f33d1ae",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I12I2One2Many",
                            plural: "I12I2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "c018face-b292-455c-a2c0-8f71377fb6cb",
                        associationType: {
                            id: "3239eb17-dc55-465f-854c-1d2d024bca94",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12C2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "2ff52878-3ade-4afe-9961-8f79336bb0a2",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I12C2Many2One",
                            plural: "I12C2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "c6ecc142-0fbd-48b7-98ae-994fa9b5b814",
                        associationType: {
                            id: "c7469ffd-ffd7-4913-962c-8a7a0b4df3dd",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12I12Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "1d091625-ec4a-486d-a9be-8f87fe300967",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I12I12Many2One",
                            plural: "I12I12Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "ccdd1ae2-263e-4221-9841-4cff1907ee8d",
                        associationType: {
                            id: "55be99e6-71fd-4483-b211-c3080e6ffa05",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "79723949-b9ad-40bf-baee-96d001942855",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "I12AllorsBoolean",
                            plural: "I12AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "ce0f7d58-b415-43f3-989b-9d8b34754e4b",
                        associationType: {
                            id: "33bd508e-d754-4533-9ecd-9c8ce8d10c88",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12I1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "72545574-d138-467c-8f21-0c5d15b1d793",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I12I1One2One",
                            plural: "I12I1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f302dd07-1abc-409e-aa71-ec9f7ac439aa",
                        associationType: {
                            id: "99b3bf26-3437-4b5b-a786-28c095975a48",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12C1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "ee291df6-6a3e-4d92-a779-879679e1b688",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I12C1One2Many",
                            plural: "I12C1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f6436bc9-e307-4001-8f1f-5b37553ab3c6",
                        associationType: {
                            id: "63297178-60c1-4cbc-a68d-2842385ba266",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12sWhereI12C1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "6e5b98b0-1af3-4e99-8781-37ea97792a24",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I12C1Many2One",
                            plural: "I12C1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "fa6656dc-3a7a-4701-bc6b-3cd06aaa4483",
                        associationType: {
                            id: "6e4d05f3-52e3-4937-b8d2-8d9d52e7c8bf",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            name: "I12WhereI12AllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "823e8329-0a90-49ed-9b2c-4bfb9db2ee02",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "I12AllorsDateTime",
                            plural: "I12AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                                isRequired: false
                            },
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "01d9ff41-d503-421e-93a6-5563e1787543",
                        associationType: {
                            id: "359ca62a-c74c-4936-a62d-9b8774174e8d",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "141b832f-7321-43b8-8033-dbad3f80edc3",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I2I2Many2One",
                            plural: "I2I2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "1f763206-c575-4e34-9e6b-997d434d3f42",
                        associationType: {
                            id: "923f6373-cbf8-46b1-9b4b-185015ff59ac",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2C1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "9edd1eb9-2b9a-4375-a669-68c1859eace2",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I2C1Many2One",
                            plural: "I2C1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "23e9c15f-097f-4452-9bac-d7cf2a65134a",
                        associationType: {
                            id: "278afe09-d0e7-4a41-a60b-b3a01fd14c93",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I12Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "e538ab5e-80f2-4a34-81e7-c9b92414dda1",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I2I12Many2One",
                            plural: "I2I12Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "35040d7c-ab7f-4a99-9d09-e01e24ca3cb9",
                        associationType: {
                            id: "d1f0ae79-1820-47a5-8869-496c3578a53d",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsBoolean",
                            isOne: true
                        },
                        roleType: {
                            id: "0d2c6dbe-9bb2-414c-8f19-5381fe69ac64",
                            objectTypeId: "b5ee6cea-4e2b-498e-a5dd-24671d896477",
                            singular: "I2AllorsBoolean",
                            plural: "I2AllorsBooleans",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "40b8edb3-e8c4-46c0-855b-4b18e0e8d7f3",
                        associationType: {
                            id: "078e1b17-f239-44b2-87d6-6350dd37ac1d",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2C1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "805d7871-bc51-4572-be01-e47ac8fef22a",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I2C1One2Many",
                            plural: "I2C1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "49736daf-d0bd-4216-97fa-958cfa21a4f0",
                        associationType: {
                            id: "02a80ccd-31c9-422c-8ad9-d96916dd7741",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2C1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "6ac5d426-9156-4467-8a04-85ccb6c964e2",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I2C1One2One",
                            plural: "I2C1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "4f095abd-8803-4610-87f0-2847ddd5e9f4",
                        associationType: {
                            id: "5371c058-628e-4a1c-b654-ad0b7013eb17",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsDecimal",
                            isOne: true
                        },
                        roleType: {
                            id: "ec80b71e-a933-4eb3-ab14-00b26c3bc805",
                            objectTypeId: "da866d8e-2c40-41a8-ae5b-5f6dae0b89c8",
                            singular: "I2AllorsDecimal",
                            plural: "I2AllorsDecimals",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "5ebbc734-23dd-494f-af2d-8e75caaa3e26",
                        associationType: {
                            id: "4d6c09d6-5644-47bb-a50a-464350053833",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "3aab87f3-2eab-4f81-9c1b-fd2e162a93b8",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I2I2Many2Many",
                            plural: "I2I2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "62a8a93d-3744-49de-9f9a-9997b6ef4da6",
                        associationType: {
                            id: "f9be65e7-6e36-42df-bb85-5198d0c12b74",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsBinary",
                            isOne: true
                        },
                        roleType: {
                            id: "e3ae23bc-5934-4c0d-a709-adb00110772d",
                            objectTypeId: "c28e515b-cae8-4d6b-95bf-062aec8042fc",
                            singular: "I2AllorsBinary",
                            plural: "I2AllorsBinaries",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "663559c4-ef64-4e78-89b4-bfa00691c627",
                        associationType: {
                            id: "9513c57f-478a-423e-ba15-b9132bc28cd0",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsUnique",
                            isOne: true
                        },
                        roleType: {
                            id: "3f03fb6f-b0ba-4c78-b86a-9c4a1c574dd4",
                            objectTypeId: "6dc0a1a8-88a4-4614-adb4-92dd3d017c0e",
                            singular: "I2AllorsUnique",
                            plural: "I2AllorsUniques",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "6bb406bc-627b-444c-9c16-df9878e05e9c",
                        associationType: {
                            id: "16647879-8af1-4f1c-8ef5-2cec85aa31f4",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I1Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "edee2f1c-3e94-45b5-80f4-160faa2074c4",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I2I1Many2One",
                            plural: "I2I1Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "81d9eb2f-55a7-4d1c-853d-4369eb691ba5",
                        associationType: {
                            id: "db4d3b11-77bd-408e-ad41-4a03272a88e1",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsDateTime",
                            isOne: true
                        },
                        roleType: {
                            id: "bdcffe2b-ffa7-4eb1-be24-8d8ab0b4dce2",
                            objectTypeId: "c4c09343-61d3-418c-ade2-fe6fd588f128",
                            singular: "I2AllorsDateTime",
                            plural: "I2AllorsDateTimes",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "83dc0581-e04a-4f51-a44e-4fef63d44356",
                        associationType: {
                            id: "b1c5cbb7-3d5f-48b8-b182-aa8a0cc3e72a",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I12One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "9598153e-9c1c-438a-a8a8-9822092a6a07",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I2I12One2Many",
                            plural: "I2I12One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "87499e99-ed77-44c1-89d6-b4f570b6f217",
                        associationType: {
                            id: "e5201e06-3fbf-4b9c-aa65-1ee4ee9fabfb",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I12One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "e4c9f00e-7c3d-4b58-92f0-ccce24b55589",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I2I12One2One",
                            plural: "I2I12One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "92fdb313-0b90-48f6-b054-a4ab38f880ba",
                        associationType: {
                            id: "a45ffec8-5e4e-4b21-9d68-9b0050472ed2",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2C2Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "17e159a2-f5a6-4828-9fef-796fcc9085e8",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I2C2Many2Many",
                            plural: "I2C2Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "9bed0518-1946-4e23-9d4b-e4cda439984c",
                        associationType: {
                            id: "7b4a8937-258c-4129-a282-89d5ab924d68",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "2e78a543-949f-4130-b659-80a9a60ad6ab",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I2I1Many2Many",
                            plural: "I2I1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "9f361b97-0b04-496d-ac60-718760c2a4e2",
                        associationType: {
                            id: "c51f6fd4-c290-41b6-b594-19e9bcbbee6a",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2C2Many2One",
                            isOne: false
                        },
                        roleType: {
                            id: "f60f8fa4-4e73-472d-b0b0-67f202c1e969",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I2C2Many2One",
                            plural: "I2C2Many2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "9f91841c-f63f-4ffa-bee6-62e100f3cd15",
                        associationType: {
                            id: "3164fd30-297e-4e2a-86d6-fad6754f1d59",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsString",
                            isOne: true
                        },
                        roleType: {
                            id: "7afb53c1-2fe3-44b6-b1d2-d5a9f6100076",
                            objectTypeId: "ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9",
                            singular: "I2AllorsString",
                            plural: "I2AllorsStrings",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b39fdd23-d7dd-473f-9705-df2f29be5ffe",
                        associationType: {
                            id: "8ddc9cbf-8e5c-4166-a2b0-6127c142da78",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2C2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "7cdd2b76-6c35-4e81-a1da-f5d0a300014b",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I2C2One2Many",
                            plural: "I2C2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "b640bf16-0dc0-4203-aa76-f456371239ae",
                        associationType: {
                            id: "257fa0c6-43ea-4fe9-8142-dbc172d1e138",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I1One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "26deb364-bd5e-4b5d-b28a-19689ab3c00d",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I2I1One2One",
                            plural: "I2I1One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "bbb01166-2671-4ca1-8b1e-12e6ae8aeb03",
                        associationType: {
                            id: "ee0766c7-0ef6-4ca0-b4a1-c399bc8df823",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I1One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "d8f011c4-3057-4384-9045-9c34b13db5c3",
                            objectTypeId: "fefcf1b6-ac8f-47b0-bed5-939207a2833e",
                            singular: "I2I1One2Many",
                            plural: "I2I1One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "cb9f21e0-a841-45de-8ba4-991b4ceca616",
                        associationType: {
                            id: "1127ff1b-1657-4e18-bdc9-bc90cd8a3c15",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2I12Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "d838e921-ff63-4e4f-afd8-42dc29d23555",
                            objectTypeId: "b45ec13c-704f-413d-a662-bdc59a17bfe3",
                            singular: "I2I12Many2Many",
                            plural: "I2I12Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "cc4c704c-ab7e-45d4-baa9-b67cfff9448e",
                        associationType: {
                            id: "d15cb643-1ace-4dfe-b0af-e02e4273bbbb",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "12c2c263-7839-4734-9307-bcde6930a2b7",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I2I2One2One",
                            plural: "I2I2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "d30dd036-6d28-48df-873b-3a76da8c029e",
                        associationType: {
                            id: "012e0afc-ebc7-4ae4-9fa0-49c72f3daebf",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsInteger",
                            isOne: true
                        },
                        roleType: {
                            id: "69c063b7-156f-4b7f-89eb-10c7eaf39ad5",
                            objectTypeId: "ccd6f134-26de-4103-bff9-a37ec3e997a3",
                            singular: "I2AllorsInteger",
                            plural: "I2AllorsIntegers",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "deb9cbd3-386f-4599-802c-be50945b9f1d",
                        associationType: {
                            id: "3fcc8e73-5f3c-4ce0-8f45-daa813278d7e",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2I2One2Many",
                            isOne: true
                        },
                        roleType: {
                            id: "c7d68f0d-24b1-40c9-9431-78763b776bee",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            singular: "I2I2One2Many",
                            plural: "I2I2One2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f364c9fe-ad36-4305-80fd-4921451c70a5",
                        associationType: {
                            id: "db6935b0-684c-48ce-97d0-6b7183a73adb",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2sWhereI2C1Many2Many",
                            isOne: false
                        },
                        roleType: {
                            id: "6ed084f6-8809-46d9-a3ec-4b086ddafb0a",
                            objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                            singular: "I2C1Many2Many",
                            plural: "I2C1Many2Manies",
                            isUnit: false,
                            isOne: false,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "f85c2d97-10b9-478d-9b82-2700d95d5cb1",
                        associationType: {
                            id: "bfb08e5e-afc6-4f27-975f-5fb9af5bacc4",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2C2One2One",
                            isOne: true
                        },
                        roleType: {
                            id: "666c65ad-8bf7-40be-a51a-e69d3e0bfe01",
                            objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                            singular: "I2C2One2One",
                            plural: "I2C2One2Ones",
                            isUnit: false,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    },
                    {
                        id: "fbad33e7-ede1-41fc-97e9-ddf33a0f6459",
                        associationType: {
                            id: "c138d77b-e8bf-4945-962e-f74e338caad4",
                            objectTypeId: "19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0",
                            name: "I2WhereI2AllorsDouble",
                            isOne: true
                        },
                        roleType: {
                            id: "12ea1f33-0eed-4476-9cab-1fd62ed146a3",
                            objectTypeId: "ffcabd07-f35f-4083-bef6-f6c47970ca5d",
                            singular: "I2AllorsDouble",
                            plural: "I2AllorsDoubles",
                            isUnit: true,
                            isOne: true,
                            isRequired: false
                        },
                        concreteRoleTypes: [
                            {
                                objectTypeId: "72c07e8a-03f5-4da8-ab37-236333d4f74e",
                                isRequired: false
                            }
                        ],
                        isDerived: false
                    }
                ],
                methodTypes: [
                    {
                        id: "09a6a387-a1b5-4038-b074-3a01c81cbda2",
                        objectTypeId: "7041c691-d896-4628-8f50-1c24f5d03414",
                        name: "ClassMethod"
                    },
                    {
                        id: "1869873f-f2f0-4d03-a0f9-7dc73491c117",
                        objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                        name: "JustDoIt"
                    },
                    {
                        id: "2cd2ff48-93fc-4c7d-bf2f-3f411d0df7c3",
                        objectTypeId: "3a5dcec7-308f-48c7-afee-35d38415aa0b",
                        name: "ToggleCanWrite"
                    },
                    {
                        id: "430702d2-e02b-45ad-9b22-b8331dc75a3f",
                        objectTypeId: "9279e337-c658-4086-946d-03c75cdb1ad3",
                        name: "Delete"
                    },
                    {
                        id: "0158d8f3-3e9f-48b3-ad25-51bd7eabc27c",
                        objectTypeId: "b86d8407-c411-49e4-aae3-64192457c701",
                        name: "Approve"
                    },
                    {
                        id: "f68b3d21-0108-40ec-9455-98764eb74874",
                        objectTypeId: "b86d8407-c411-49e4-aae3-64192457c701",
                        name: "Reject"
                    }
                ]
            };
        })(Data = Meta.Data || (Meta.Data = {}));
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
// Allors generated file.
// Do not edit this file, changes will be overwritten.
// tslint:disable:object-literal-sort-keys
var Allors;
(function (Allors) {
    var Meta;
    (function (Meta) {
        Meta.ids = {
            Binary: 'c28e515b-cae8-4d6b-95bf-062aec8042fc',
            Boolean: 'b5ee6cea-4e2b-498e-a5dd-24671d896477',
            DateTime: 'c4c09343-61d3-418c-ade2-fe6fd588f128',
            Decimal: 'da866d8e-2c40-41a8-ae5b-5f6dae0b89c8',
            Float: 'ffcabd07-f35f-4083-bef6-f6c47970ca5d',
            Integer: 'ccd6f134-26de-4103-bff9-a37ec3e997a3',
            String: 'ad7f5ddc-bedb-4aaa-97ac-d6693a009ba9',
            Unique: '6dc0a1a8-88a4-4614-adb4-92dd3d017c0e',
            Deletable: '9279e337-c658-4086-946d-03c75cdb1ad3',
            Enumeration: 'b7bcc22f-03f0-46fd-b738-4e035921d445',
            UniquelyIdentifiable: '122ccfe1-f902-44c1-9d6c-6f6a0afa9469',
            Version: 'a6a3c79e-150b-4586-96ea-5ac0e2e638c6',
            Printable: '61207a42-3199-4249-baa4-9dd11dc0f5b1',
            Localised: '7979a17c-0829-46df-a0d4-1b01775cfaac',
            ApproveTask: 'b86d8407-c411-49e4-aae3-64192457c701',
            ObjectState: 'f991813f-3146-4431-96d0-554aa2186887',
            Task: '84eb0e6e-68e1-478c-a35f-6036d45792be',
            User: 'a0309c3b-6f80-4777-983e-6e69800df5be',
            WorkItem: 'fbea29c6-6109-4163-a088-9f0b4deac896',
            I1: 'fefcf1b6-ac8f-47b0-bed5-939207a2833e',
            I12: 'b45ec13c-704f-413d-a662-bdc59a17bfe3',
            I2: '19bb2bc3-d53a-4d15-86d0-b250fdbcb0a0',
            S1: '253b0d71-9eaa-4d87-9094-3b549d8446b3',
            Counter: '0568354f-e3d9-439e-baac-b7dce31b956a',
            Singleton: '313b97a5-328c-4600-9dd2-b5bc146fb13b',
            Media: 'da5b86a3-4f33-4c0d-965d-f4fbc1179374',
            MediaContent: '6c20422e-cb3e-4402-bb40-dacaf584405e',
            PrintDocument: '6161594b-8acf-4dfa-ae6d-a9bc96040714',
            Template: '93f8b97b-2d9a-42fc-a823-7bdcc5a92203',
            TemplateType: 'bdabb545-3b39-4f91-9d01-a589a5da670e',
            PreparedExtent: '645a4f92-f1f1-41c7-ba76-53a1cc4d2a61',
            PreparedFetch: '02c7569c-8f54-4f8d-ac09-1bacd9528f1f',
            Country: 'c22bf60e-6428-4d10-8194-94f7be396f28',
            Currency: 'fd397adf-40b4-4ef8-b449-dd5a24273df3',
            Language: '4a0eca4b-281f-488d-9c7e-497de882c044',
            Locale: '45033ae6-85b5-4ced-87ce-02518e6c27fd',
            LocalisedMedia: '2288e1f3-5dc5-458b-9f5e-076f133890c0',
            LocalisedText: '020f5d4d-4a59-4d7b-865a-d72fc70e4d97',
            AccessControl: 'c4d93d5e-34c3-4731-9d37-47a8e801d9a8',
            Login: 'ad7277a8-eda4-4128-a990-b47fe43d120a',
            Permission: '7fded183-3337-4196-afb0-3266377944bc',
            Role: 'af6fe5f4-e5bc-4099-bcd1-97528af6505d',
            SecurityToken: 'a53f1aed-0e3f-4c3c-9600-dc579cccf893',
            AutomatedAgent: '3587d2e1-c3f6-4c55-a96c-016e0501d99c',
            Notification: '73dcdc68-7571-4ed1-86db-77c914fe2f62',
            NotificationList: 'b6579993-4ff1-4853-b048-1f8e67419c00',
            Person: 'c799ca62-a554-467d-9aa2-1663293bb37f',
            TaskAssignment: '4092d0b4-c6f4-4b81-b023-66be3f4c90bd',
            UserGroup: '60065f5d-a3c2-4418-880d-1026ab607319',
            C1: '7041c691-d896-4628-8f50-1c24f5d03414',
            C2: '72c07e8a-03f5-4da8-ab37-236333d4f74e',
            Data: '0e82b155-208c-41fd-b7d0-731eadbb5338',
            Dependent: '0cb8d2a7-4566-432f-9882-893b05a77f44',
            Gender: '270f0dc8-1bc2-4a42-9617-45e93d5403c8',
            Order: '94be4938-77c1-488f-b116-6d4daeffcc8d',
            OrderLine: '721008c3-c87c-40ab-966b-094e1271ed5f',
            OrderLineVersion: 'ba589be8-049b-4107-9e20-fbfec19477c4',
            OrderState: '849393ed-cff6-40da-9b4d-483f045f2e99',
            OrderVersion: '6a3a9167-9a77-491e-a1c8-ccfe4572afb4',
            Organisation: '3a5dcec7-308f-48c7-afee-35d38415aa0b',
            PaymentState: '07e8f845-5ecc-4b42-83ef-bb86e6b10a69',
            ShipmentState: 'ce56a6e9-8e4b-4f40-8676-180f4b0513e2',
            UnitSample: '4e501cd6-807c-4f10-b60b-acd1d80042cd',
        };
        Meta.unitIdByTypeName = {
            Binary: Meta.ids.Binary,
            Boolean: Meta.ids.Boolean,
            DateTime: Meta.ids.DateTime,
            Decimal: Meta.ids.Decimal,
            Float: Meta.ids.Float,
            Integer: Meta.ids.Integer,
            String: Meta.ids.String,
            Unique: Meta.ids.Unique,
        };
    })(Meta = Allors.Meta || (Allors.Meta = {}));
})(Allors || (Allors = {}));
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
                var serviceName = _this.baseUrl + "allors/sync";
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
                var serviceName = _this.baseUrl + "allors/push";
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
                var serviceName = _this.baseUrl + "allors/invoke";
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
                    //var syncResponse: Data.SyncResponse = {
                    //    responseType: Data.ResponseType.Sync,
                    //    objects: [
                    //        {
                    //            i: id,
                    //            v: "",
                    //            t: newSessionObject.objectType.name,
                    //            roles: [],
                    //            methods: []
                    //        }
                    //    ]
                    //}
                    delete (_this.newSessionObjectById[newId]);
                    delete (newSessionObject.newId);
                    //this.workspace.sync(syncResponse);
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
        var ConcreteRoleType = /** @class */ (function () {
            function ConcreteRoleType(metaPopulation) {
                this.metaPopulation = metaPopulation;
            }
            Object.defineProperty(ConcreteRoleType.prototype, "id", {
                get: function () { return this.roleType.id; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "objectType", {
                get: function () { return this.roleType.objectType; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "name", {
                get: function () { return this.roleType.name; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "singular", {
                get: function () { return this.roleType.singular; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "plural", {
                get: function () { return this.roleType.plural; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "isOne", {
                get: function () { return this.roleType.isOne; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "isMany", {
                get: function () { return this.roleType.isMany; },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ConcreteRoleType.prototype, "isDerived", {
                get: function () { return this.roleType.isDerived; },
                enumerable: true,
                configurable: true
            });
            return ConcreteRoleType;
        }());
        Meta.ConcreteRoleType = ConcreteRoleType;
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
                    objectType.id = Meta.unitIdByTypeName[unitName];
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
                    var relationType = new Meta.RelationType(_this);
                    relationType.id = relationTypeData.id;
                    var dataAssociationType = relationTypeData.associationType;
                    var associationType = relationType.associationType;
                    associationType.id = dataAssociationType.id;
                    associationType.objectType = _this.metaObjectById.get(dataAssociationType.objectTypeId);
                    associationType.name = dataAssociationType.name;
                    associationType.isOne = dataAssociationType.isOne;
                    var dataRoleType = relationTypeData.roleType;
                    var roleType = relationType.roleType;
                    roleType.id = dataRoleType.id;
                    roleType.objectType = _this.metaObjectById.get(dataRoleType.objectTypeId);
                    roleType.singular = dataRoleType.singular;
                    roleType.plural = dataRoleType.plural;
                    roleType.isOne = dataRoleType.isOne;
                    roleType.isRequired = dataRoleType.isRequired;
                    roleType.name = roleType.isOne ? roleType.singular : roleType.plural;
                    if (relationTypeData.concreteRoleTypes) {
                        relationTypeData.concreteRoleTypes.forEach(function (dataConcreteRoleType) {
                            var concreteRoleType = new Meta.ConcreteRoleType(_this);
                            concreteRoleType.relationType = relationType;
                            concreteRoleType.roleType = roleType;
                            concreteRoleType.isRequired = dataConcreteRoleType.isRequired;
                            var objectType = _this.metaObjectById.get(dataConcreteRoleType.objectTypeId);
                            relationType.concreteRoleTypeByClass.set(objectType, concreteRoleType);
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
                // Derive ConcreteRoleTypes
                this.classes
                    .forEach(function (objectType) {
                    objectType.roleTypes = objectType.roleTypes.map(function (roleType) {
                        var relationType = roleType.relationType;
                        if (relationType.associationType.objectType.isInterface) {
                            var concreteRoleType = relationType.concreteRoleTypeByClass.get(objectType);
                            if (concreteRoleType) {
                                return concreteRoleType;
                            }
                        }
                        return roleType;
                    });
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
                    return this.id === Meta.ids.Binary;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isBoolean", {
                get: function () {
                    return this.id === Meta.ids.Boolean;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isDateTime", {
                get: function () {
                    return this.id === Meta.ids.DateTime;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isDecimal", {
                get: function () {
                    return this.id === Meta.ids.Decimal;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isFloat", {
                get: function () {
                    return this.id === Meta.ids.Float;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isInteger", {
                get: function () {
                    return this.id === Meta.ids.Integer;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isString", {
                get: function () {
                    return this.id === Meta.ids.String;
                },
                enumerable: true,
                configurable: true
            });
            Object.defineProperty(ObjectType.prototype, "isUnique", {
                get: function () {
                    return this.id === Meta.ids.Unique;
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
                this.associationType = new Meta.AssociationType(this);
                this.roleType = new Meta.RoleType(this);
                this.concreteRoleTypeByClass = new Map();
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
        var RoleType = /** @class */ (function () {
            function RoleType(relationType) {
                this.relationType = relationType;
                this.metaPopulation = relationType.metaPopulation;
            }
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
    var Data;
    (function (Data) {
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
        Data.Compressor = Compressor;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        Data.createMetaDecompressor = function (decompressor, metaPopulation) { return function (compressed) {
            return metaPopulation.metaObjectById.get(decompressor.read(compressed, function (v) { }));
        }; };
        var Decompressor = /** @class */ (function () {
            function Decompressor() {
                this.valueByKey = new Map();
            }
            Decompressor.prototype.read = function (compressed, first) {
                if (compressed !== undefined && compressed !== null) {
                    if (compressed[0] === Data.Compressor.indexMarker) {
                        var secondIndexMarkerIndex = compressed.indexOf(Data.Compressor.indexMarker, 1);
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
        Data.Decompressor = Decompressor;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var ResponseType;
        (function (ResponseType) {
            ResponseType[ResponseType["Invoke"] = 0] = "Invoke";
            ResponseType[ResponseType["Pull"] = 1] = "Pull";
            ResponseType[ResponseType["Sync"] = 2] = "Sync";
            ResponseType[ResponseType["Push"] = 3] = "Push";
            ResponseType[ResponseType["Security"] = 4] = "Security";
        })(ResponseType = Data.ResponseType || (Data.ResponseType = {}));
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var PushRequest = /** @class */ (function () {
            function PushRequest() {
            }
            return PushRequest;
        }());
        Data.PushRequest = PushRequest;
        var PushRequestNewObject = /** @class */ (function () {
            function PushRequestNewObject() {
            }
            return PushRequestNewObject;
        }());
        Data.PushRequestNewObject = PushRequestNewObject;
        var PushRequestObject = /** @class */ (function () {
            function PushRequestObject() {
            }
            return PushRequestObject;
        }());
        Data.PushRequestObject = PushRequestObject;
        var PushRequestRole = /** @class */ (function () {
            function PushRequestRole() {
            }
            return PushRequestRole;
        }());
        Data.PushRequestRole = PushRequestRole;
    })(Data = Allors.Data || (Allors.Data = {}));
})(Allors || (Allors = {}));
var Allors;
(function (Allors) {
    var Data;
    (function (Data) {
        var SecurityRequest = /** @class */ (function () {
            function SecurityRequest() {
            }
            return SecurityRequest;
        }());
        Data.SecurityRequest = SecurityRequest;
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
                var postfix = "/Pull";
                this.database = new Allors.Database(this.$http, this.$q, postfix, this.baseUrl);
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