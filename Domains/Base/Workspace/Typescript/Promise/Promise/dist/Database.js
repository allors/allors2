"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const base_domain_1 = require("@allors/base-domain");
const base_domain_2 = require("@allors/base-domain");
class Database {
    constructor(http) {
        this.http = http;
    }
    pull(name, params) {
        return new Promise((resolve, reject) => {
            const serviceName = `${name}/Pull`;
            this.http.post(serviceName, params || {})
                .then((httpResponse) => {
                const response = httpResponse.data;
                response.responseType = base_domain_2.ResponseType.Pull;
                resolve(response);
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    sync(syncRequest) {
        return new Promise((resolve, reject) => {
            const serviceName = `Database/Sync`;
            this.http.post(serviceName, syncRequest)
                .then((httpResponse) => {
                const response = httpResponse.data;
                response.responseType = base_domain_2.ResponseType.Sync;
                resolve(response);
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    push(pushRequest) {
        return new Promise((resolve, reject) => {
            const serviceName = `Database/Push`;
            this.http.post(serviceName, pushRequest)
                .then((httpResponse) => {
                const response = httpResponse.data;
                response.responseType = base_domain_2.ResponseType.Sync;
                if (response.hasErrors) {
                    reject(response);
                }
                else {
                    resolve(response);
                }
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    invoke(methodOrService, args) {
        return new Promise((resolve, reject) => {
            if (methodOrService instanceof base_domain_1.Method) {
                const method = methodOrService;
                const invokeRequest = {
                    i: method.object.id,
                    m: method.name,
                    v: method.object.version,
                };
                const serviceName = `Database/Invoke`;
                this.http.post(serviceName, invokeRequest)
                    .then((httpResponse) => {
                    const response = httpResponse.data;
                    response.responseType = base_domain_2.ResponseType.Invoke;
                    if (response.hasErrors) {
                        reject(response);
                    }
                    else {
                        resolve(response);
                    }
                })
                    .catch((e) => {
                    reject(e);
                });
            }
            else {
                const serviceName = `${methodOrService}/Pull`;
                this.http.post(serviceName, args)
                    .then((httpResponse) => {
                    const response = httpResponse.data;
                    response.responseType = base_domain_2.ResponseType.Invoke;
                    if (response.hasErrors) {
                        reject(response);
                    }
                    else {
                        resolve(response);
                    }
                })
                    .catch((e) => {
                    reject(e);
                });
            }
        });
    }
}
exports.Database = Database;
