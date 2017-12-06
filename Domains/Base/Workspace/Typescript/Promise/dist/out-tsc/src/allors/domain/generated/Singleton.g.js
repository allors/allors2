"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class Singleton extends framework_1.SessionObject {
    get CanReadDefaultLocale() {
        return this.canRead('DefaultLocale');
    }
    get CanWriteDefaultLocale() {
        return this.canWrite('DefaultLocale');
    }
    get DefaultLocale() {
        return this.get('DefaultLocale');
    }
    set DefaultLocale(value) {
        this.set('DefaultLocale', value);
    }
    get CanReadLocales() {
        return this.canRead('Locales');
    }
    get CanWriteLocales() {
        return this.canWrite('Locales');
    }
    get Locales() {
        return this.get('Locales');
    }
    AddLocale(value) {
        return this.add('Locales', value);
    }
    RemoveLocale(value) {
        return this.remove('Locales', value);
    }
    set Locales(value) {
        this.set('Locales', value);
    }
    get CanReadGuest() {
        return this.canRead('Guest');
    }
    get CanWriteGuest() {
        return this.canWrite('Guest');
    }
    get Guest() {
        return this.get('Guest');
    }
    set Guest(value) {
        this.set('Guest', value);
    }
}
exports.Singleton = Singleton;
//# sourceMappingURL=Singleton.g.js.map