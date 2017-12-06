"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class Country extends framework_1.SessionObject {
    get CanReadCurrency() {
        return this.canRead('Currency');
    }
    get CanWriteCurrency() {
        return this.canWrite('Currency');
    }
    get Currency() {
        return this.get('Currency');
    }
    set Currency(value) {
        this.set('Currency', value);
    }
    get CanReadIsoCode() {
        return this.canRead('IsoCode');
    }
    get CanWriteIsoCode() {
        return this.canWrite('IsoCode');
    }
    get IsoCode() {
        return this.get('IsoCode');
    }
    set IsoCode(value) {
        this.set('IsoCode', value);
    }
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
    get CanReadLocalisedNames() {
        return this.canRead('LocalisedNames');
    }
    get CanWriteLocalisedNames() {
        return this.canWrite('LocalisedNames');
    }
    get LocalisedNames() {
        return this.get('LocalisedNames');
    }
    AddLocalisedName(value) {
        return this.add('LocalisedNames', value);
    }
    RemoveLocalisedName(value) {
        return this.remove('LocalisedNames', value);
    }
    set LocalisedNames(value) {
        this.set('LocalisedNames', value);
    }
}
exports.Country = Country;
//# sourceMappingURL=Country.g.js.map