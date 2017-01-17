var Allors;
(function (Allors) {
    var Bootstrap;
    (function (Bootstrap) {
        var Field = (function () {
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
                        });
                    }
                });
            };
            return Field;
        }());
        Bootstrap.Field = Field;
    })(Bootstrap = Allors.Bootstrap || (Allors.Bootstrap = {}));
})(Allors || (Allors = {}));
//# sourceMappingURL=Field.js.map