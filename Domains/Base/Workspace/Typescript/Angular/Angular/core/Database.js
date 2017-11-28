"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const framework_1 = require("@allors/framework");
const framework_2 = require("@allors/framework");
class Database {
    constructor(http, url) {
        this.http = http;
        this.url = url;
    }
    pull(name, params) {
        const serviceName = this.fullyQualifiedUrl(name + "/Pull");
        return this.http
            .post(serviceName, params)
            .map((pullResponse) => {
            pullResponse.responseType = framework_1.ResponseType.Pull;
            return pullResponse;
        });
    }
    sync(syncRequest) {
        const serviceName = this.fullyQualifiedUrl("Database/Sync");
        return this.http
            .post(serviceName, syncRequest)
            .map((syncResponse) => {
            syncResponse.responseType = framework_1.ResponseType.Sync;
            return syncResponse;
        });
    }
    push(pushRequest) {
        const serviceName = this.fullyQualifiedUrl("Database/Push");
        return this.http
            .post(serviceName, pushRequest)
            .map((pushResponse) => {
            pushResponse.responseType = framework_1.ResponseType.Sync;
            if (pushResponse.hasErrors) {
                throw new framework_1.ResponseError(pushResponse);
            }
            return pushResponse;
        });
    }
    invoke(methodOrService, args) {
        if (methodOrService instanceof framework_2.Method) {
            return this.invokeMethod(methodOrService);
        }
        else {
            return this.invokeService(methodOrService, args);
        }
    }
    invokeMethod(method) {
        const invokeRequest = {
            i: method.object.id,
            m: method.name,
            v: method.object.version,
        };
        const serviceName = this.fullyQualifiedUrl("Database/Invoke");
        return this.http
            .post(serviceName, invokeRequest)
            .map((invokeResponse) => {
            invokeResponse.responseType = framework_1.ResponseType.Invoke;
            if (invokeResponse.hasErrors) {
                throw new framework_1.ResponseError(invokeResponse);
            }
            return invokeResponse;
        });
    }
    invokeService(methodOrService, args) {
        const service = this.fullyQualifiedUrl(methodOrService + "/Pull");
        return this.http
            .post(service, args)
            .map((invokeResponse) => {
            invokeResponse.responseType = framework_1.ResponseType.Invoke;
            if (invokeResponse.hasErrors) {
                throw new framework_1.ResponseError(invokeResponse);
            }
            return invokeResponse;
        });
    }
    fullyQualifiedUrl(localUrl) {
        return this.url + localUrl;
    }
}
exports.Database = Database;
