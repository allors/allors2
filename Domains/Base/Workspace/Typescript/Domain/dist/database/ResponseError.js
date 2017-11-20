"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class ResponseError extends Error {
    constructor(response) {
        super();
        this.response = response;
        // Fix for extending builtin objects for es5
        Object.setPrototypeOf(this, ResponseError.prototype);
    }
}
exports.ResponseError = ResponseError;
