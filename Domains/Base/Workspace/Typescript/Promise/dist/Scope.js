"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const base_domain_1 = require("@allors/base-domain");
const Invoked_1 = require("./responses/Invoked");
const Loaded_1 = require("./responses/Loaded");
const Saved_1 = require("./responses/Saved");
class Scope {
    constructor(database, workspace) {
        this.database = database;
        this.workspace = workspace;
        this.session = new base_domain_1.Session(this.workspace);
    }
    load(service, params) {
        return new Promise((resolve, reject) => {
            this.database
                .pull(service, params)
                .then((pullResponse) => {
                const requireLoadIds = this.workspace.diff(pullResponse);
                if (requireLoadIds.objects.length > 0) {
                    return this.database
                        .sync(requireLoadIds)
                        .then((syncResponse) => {
                        this.workspace.sync(syncResponse);
                        const loaded = new Loaded_1.Loaded(this.session, pullResponse);
                        resolve(loaded);
                    })
                        .catch((e) => {
                        reject(e);
                    });
                }
                else {
                    const loaded = new Loaded_1.Loaded(this.session, pullResponse);
                    resolve(loaded);
                }
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    save() {
        return new Promise((resolve, reject) => {
            const pushRequest = this.session.pushRequest();
            return this.database
                .push(pushRequest)
                .then((pushResponse) => {
                this.session.pushResponse(pushResponse);
                const syncRequest = new base_domain_1.SyncRequest();
                syncRequest.objects = pushRequest.objects.map((v) => v.i);
                if (pushResponse.newObjects) {
                    for (const newObject of pushResponse.newObjects) {
                        syncRequest.objects.push(newObject.i);
                    }
                }
                return this.database
                    .sync(syncRequest)
                    .then((syncResponse) => {
                    this.workspace.sync(syncResponse);
                    const saved = new Saved_1.Saved(this.session, pushResponse);
                    resolve(saved);
                })
                    .catch((e) => {
                    reject(e);
                });
            })
                .catch((e) => {
                reject(e);
            });
        });
    }
    invoke(methodOrService, args) {
        return this.database
            .invoke(methodOrService, args)
            .then((invokeResponse) => new Invoked_1.Invoked(this.session, invokeResponse));
    }
}
exports.Scope = Scope;
