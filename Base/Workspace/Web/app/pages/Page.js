var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var App;
(function (App) {
    var Pages;
    (function (Pages) {
        var Method = Allors.Method;
        var Page = (function (_super) {
            __extends(Page, _super);
            function Page(name, app, $scope) {
                var _this = _super.call(this, name, app.database, app.workspace, app.$rootScope, $scope, app.$q, app.$log) || this;
                _this.toastr = app.toastr;
                return _this;
            }
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
                            reject(e);
                        }
                        else {
                            throw e;
                        }
                    });
                });
            };
            Page.prototype.invoke = function (methodOrService, args) {
                var _this = this;
                return this.$q(function (resolve, reject) {
                    var superInvoke;
                    if (methodOrService instanceof Method) {
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
                            reject(e);
                        }
                        else {
                            throw e;
                        }
                    });
                });
            };
            Page.prototype.saveAndInvoke = function (methodOrService, args) {
                var _this = this;
                return this.$q(function (resolve, reject) {
                    var superSaveAndInvoke;
                    if (methodOrService instanceof Method) {
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
                            reject(e);
                        }
                        else {
                            throw e;
                        }
                    });
                });
            };
            Page.prototype.errorResponse = function (error) {
                var title = "Error";
                var message = "<div class=\"response-errors\">Error during " + error.responseType + ".</div>";
                if (error.errorMessage && error.errorMessage.length > 0) {
                    title = "General Error";
                    message += "<p>" + error.errorMessage + "</p>";
                }
                if ((error.versionErrors && error.versionErrors.length > 0) ||
                    (error.missingErrors && error.missingErrors.length > 0)) {
                    title = "Concurrency Error";
                    message += "<p>Modifications were detected since last access.</p>";
                }
                if (error.accessErrors && error.accessErrors.length > 0) {
                    title = "Access Error";
                    message += "<p>You do not have the required rights.</p>";
                }
                if (error.derivationErrors && error.derivationErrors.length > 0) {
                    title = "Derivation Errors";
                    message += "<ul>";
                    error.derivationErrors.map(function (derivationError) {
                        message += "<li>" + derivationError.m + "</li>";
                    });
                    message += "</ul>";
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
        Pages.Page = Page;
    })(Pages = App.Pages || (App.Pages = {}));
})(App || (App = {}));
//# sourceMappingURL=Page.js.map