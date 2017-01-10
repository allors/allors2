namespace App.Pages {
    import Method = Allors.Method;
    import Data = Allors.Data;

    export abstract class Page extends Allors.Scope {

        toastr: angular.toastr.IToastrService;

        constructor(name: string, app: Services.AppService, $scope: angular.IScope) {
            super(name, app.database, app.workspace, app.$rootScope, $scope, app.$q, app.$log);

            this.toastr = app.toastr;
        }

        save(): angular.IPromise<any> {
            return this.$q((resolve, reject) => {
                super.save()
                    .then((saveResponse: Allors.Data.PushResponse) => {
                        this.toastr.info("Successfully saved.");
                        resolve(saveResponse);
                    })
                    .catch((e: Allors.Data.ErrorResponse) => {
                        if (e.responseType) {
                            this.errorResponse(e);
                            reject(e);
                        } else {
                            throw e;
                        }
                    });
            });
        }

        invoke(method: Method): angular.IPromise<Data.InvokeResponse>;
        invoke(service: string, args?: any): angular.IPromise<any>;
        invoke(methodOrService: Method | string, args?: any): angular.IPromise<any> {
            return this.$q((resolve, reject) => {

                var superInvoke: angular.IPromise<Allors.Data.InvokeResponse>;
                if (methodOrService instanceof Method) {
                    superInvoke = super.invoke(methodOrService);
                } else {
                    superInvoke = super.invoke(methodOrService as string, args);
                }

                superInvoke.then((invokeResponse: Allors.Data.InvokeResponse) => {
                    this.toastr.info("Successfully executed.");
                    resolve(invokeResponse);
                })
                    .catch((e: Allors.Data.ErrorResponse) => {
                        if (e.responseType) {
                            this.errorResponse(e);
                            reject(e);
                        } else {
                            throw e;
                        }
                    });

            });
        }

        saveAndInvoke(method: Method): angular.IPromise<Data.InvokeResponse>;
        saveAndInvoke(service: string, args?: any): angular.IPromise<any>;
        saveAndInvoke(methodOrService: Method | string, args?: any): angular.IPromise<any> {
            return this.$q((resolve, reject) => {

                var superSaveAndInvoke: angular.IPromise<Allors.Data.InvokeResponse>;
                if (methodOrService instanceof Method) {
                    superSaveAndInvoke = super.saveAndInvoke(methodOrService);
                } else {
                    superSaveAndInvoke = super.saveAndInvoke(methodOrService as string, args);
                }

                superSaveAndInvoke.then((invokeResponse: Allors.Data.InvokeResponse) => {
                    this.toastr.info("Successfully executed.");
                    resolve(invokeResponse);
                })
                    .catch((e: Allors.Data.ErrorResponse) => {
                        if (e.responseType) {
                            this.errorResponse(e);
                            reject(e);
                        } else {
                            throw e;
                        }
                    });

            });
        }

        protected errorResponse(error: Allors.Data.ErrorResponse) {
            let title = "Error";
            var message = `<div class=\"response-errors\">Error during ${error.responseType}.</div>`;

            if (error.errorMessage && error.errorMessage.length > 0) {
                title = "General Error";
                message += `<p>${error.errorMessage}</p>`;
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
                error.derivationErrors.map(derivationError => {
                    message += `<li>${derivationError.m}</li>`;
                });

                message += "</ul>";
            }

            message += "<div>";

            this.toastr.error(message, title, {
                allowHtml: true,
                closeButton: true,
                timeOut: 0
            });
        }
    }
}