"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class Locale extends framework_1.SessionObject {
    get CanReadName() {
        return this.canRead('Name');
    }
    get CanWriteName() {
        return this.canWrite('Name');
    }
    get Name() {
        return this.get('Name');
    }
    set Name(value) {
        this.set('Name', value);
    }
    get CanReadLanguage() {
        return this.canRead('Language');
    }
    get CanWriteLanguage() {
        return this.canWrite('Language');
    }
    get Language() {
        return this.get('Language');
    }
    set Language(value) {
        this.set('Language', value);
    }
    get CanReadCountry() {
        return this.canRead('Country');
    }
    get CanWriteCountry() {
        return this.canWrite('Country');
    }
    get Country() {
        return this.get('Country');
    }
    set Country(value) {
        this.set('Country', value);
    }
}
exports.Locale = Locale;
//# sourceMappingURL=Locale.g.js.map