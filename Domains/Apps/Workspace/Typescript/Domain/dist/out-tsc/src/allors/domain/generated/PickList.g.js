"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class PickList extends framework_1.SessionObject {
    get CanReadPickListState() {
        return this.canRead('PickListState');
    }
    get CanWritePickListState() {
        return this.canWrite('PickListState');
    }
    get PickListState() {
        return this.get('PickListState');
    }
    set PickListState(value) {
        this.set('PickListState', value);
    }
    get CanReadCurrentVersion() {
        return this.canRead('CurrentVersion');
    }
    get CanWriteCurrentVersion() {
        return this.canWrite('CurrentVersion');
    }
    get CurrentVersion() {
        return this.get('CurrentVersion');
    }
    set CurrentVersion(value) {
        this.set('CurrentVersion', value);
    }
    get CanReadAllVersions() {
        return this.canRead('AllVersions');
    }
    get CanWriteAllVersions() {
        return this.canWrite('AllVersions');
    }
    get AllVersions() {
        return this.get('AllVersions');
    }
    AddAllVersion(value) {
        return this.add('AllVersions', value);
    }
    RemoveAllVersion(value) {
        return this.remove('AllVersions', value);
    }
    set AllVersions(value) {
        this.set('AllVersions', value);
    }
    get CanReadCustomerShipmentCorrection() {
        return this.canRead('CustomerShipmentCorrection');
    }
    get CanWriteCustomerShipmentCorrection() {
        return this.canWrite('CustomerShipmentCorrection');
    }
    get CustomerShipmentCorrection() {
        return this.get('CustomerShipmentCorrection');
    }
    set CustomerShipmentCorrection(value) {
        this.set('CustomerShipmentCorrection', value);
    }
    get CanReadCreationDate() {
        return this.canRead('CreationDate');
    }
    get CanWriteCreationDate() {
        return this.canWrite('CreationDate');
    }
    get CreationDate() {
        return this.get('CreationDate');
    }
    set CreationDate(value) {
        this.set('CreationDate', value);
    }
    get CanReadPickListItems() {
        return this.canRead('PickListItems');
    }
    get CanWritePickListItems() {
        return this.canWrite('PickListItems');
    }
    get PickListItems() {
        return this.get('PickListItems');
    }
    AddPickListItem(value) {
        return this.add('PickListItems', value);
    }
    RemovePickListItem(value) {
        return this.remove('PickListItems', value);
    }
    set PickListItems(value) {
        this.set('PickListItems', value);
    }
    get CanReadPicker() {
        return this.canRead('Picker');
    }
    get CanWritePicker() {
        return this.canWrite('Picker');
    }
    get Picker() {
        return this.get('Picker');
    }
    set Picker(value) {
        this.set('Picker', value);
    }
    get CanReadShipToParty() {
        return this.canRead('ShipToParty');
    }
    get CanWriteShipToParty() {
        return this.canWrite('ShipToParty');
    }
    get ShipToParty() {
        return this.get('ShipToParty');
    }
    set ShipToParty(value) {
        this.set('ShipToParty', value);
    }
    get CanReadStore() {
        return this.canRead('Store');
    }
    get CanWriteStore() {
        return this.canWrite('Store');
    }
    get Store() {
        return this.get('Store');
    }
    set Store(value) {
        this.set('Store', value);
    }
    get CanReadPrintContent() {
        return this.canRead('PrintContent');
    }
    get CanWritePrintContent() {
        return this.canWrite('PrintContent');
    }
    get PrintContent() {
        return this.get('PrintContent');
    }
    set PrintContent(value) {
        this.set('PrintContent', value);
    }
}
exports.PickList = PickList;
//# sourceMappingURL=PickList.g.js.map