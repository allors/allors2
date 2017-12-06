"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
class Loaded {
    constructor(session, response) {
        this.session = session;
        this.response = response;
        this.objects = {};
        this.collections = {};
        this.values = {};
        const namedObjects = response.namedObjects;
        const namedCollections = response.namedCollections;
        const namedValues = response.namedValues;
        Object.keys(namedObjects).map((key) => this.objects[key] = session.get(namedObjects[key]));
        Object.keys(namedCollections).map((key) => this.collections[key] = namedCollections[key].map((id) => session.get(id)));
        Object.keys(namedValues).map((key) => this.values[key] = namedValues[key]);
    }
}
exports.Loaded = Loaded;
//# sourceMappingURL=Loaded.js.map