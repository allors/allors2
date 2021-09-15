/// <reference path="../../Core/Angular/Scope.ts" />
namespace App {

    export abstract class Page extends Allors.Scope {
        
        toastr: angular.toastr.IToastrService;

        constructor(name: string, allors: Services.AllorsService, $scope: angular.IScope) {
            super(name, allors.database, allors.workspace, allors.$rootScope, $scope, allors.$q, allors.$log);

            this.toastr = allors.toastr;
        }

        getPersonTypeAhead(criteria: string) {
            return this.queryResults("PersonTypeAhead", { criteria: criteria });
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
                            resolve(e);
                        }
                        else {
                            reject(e);
                        }
                    });
            });
        }

        invoke(method: Allors.Method): angular.IPromise<Allors.Data.InvokeResponse>;
        invoke(service: string, args?: any): angular.IPromise<any>;
        invoke(methodOrService: Allors.Method | string, args?: any): angular.IPromise<any> {
            return this.$q((resolve, reject) => {

                var superInvoke: angular.IPromise<Allors.Data.InvokeResponse>;
                if (methodOrService instanceof Allors.Method) {
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
                            resolve(e);
                        } else {
                            reject(e);
                        }
                    });

            });
        }

        saveAndInvoke(method: Allors.Method): angular.IPromise<Allors.Data.InvokeResponse>;
        saveAndInvoke(service: string, args?: any): angular.IPromise<any>;
        saveAndInvoke(methodOrService: Allors.Method | string, args?: any): angular.IPromise<any> {
            return this.$q((resolve, reject) => {

                var superSaveAndInvoke: angular.IPromise<Allors.Data.InvokeResponse>;
                if (methodOrService instanceof Allors.Method) {
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
                            resolve(e);
                        } else {
                            reject(e);
                        }
                    });

            });
        }

        public errorResponse(error: Allors.Data.ErrorResponse) {
            let title;
            let message = "<div>";

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
                error.derivationErrors.map(derivationError => {
                    message += `<li>${derivationError.m}</li>`;
                });

                message += "</ul>";
            }
            else  {
                title = "General Error";
                message += `<p>${error.errorMessage}</p>`;
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
