"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const base_domain_1 = require("@allors/base-domain");
function errorDialog(dialogService, error) {
    let title = "";
    let message = "";
    if (error instanceof base_domain_1.ResponseError) {
        const responseError = error;
        const response = error.response;
        if (response.accessErrors && response.accessErrors.length > 0) {
            title = "Access Error";
            message = "You do not have the required rights.";
        }
        else if ((response.versionErrors && response.versionErrors.length > 0) ||
            (response.missingErrors && response.missingErrors.length > 0)) {
            title = "Concurrency Error";
            message += "Modifications were detected since last access.";
        }
        else if (response.derivationErrors && response.derivationErrors.length > 0) {
            title = "Derivation Errors";
            response.derivationErrors.map((derivationError) => {
                message += `\n* ${derivationError.m}`;
            });
        }
        else {
            title = "Error";
            message = responseError.message;
        }
    }
    else {
        title = "Error";
        if (error.message) {
            message = error.message;
        }
        else {
            message = JSON.stringify(error);
        }
    }
    return dialogService.openAlert({
        message,
        title,
        closeButton: "Close",
    });
}
exports.errorDialog = errorDialog;
