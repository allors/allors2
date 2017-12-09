"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class ProductCategory extends framework_1.SessionObject {
    get CanReadPackage() {
        return this.canRead('Package');
    }
    get CanWritePackage() {
        return this.canWrite('Package');
    }
    get Package() {
        return this.get('Package');
    }
    set Package(value) {
        this.set('Package', value);
    }
    get CanReadCode() {
        return this.canRead('Code');
    }
    get CanWriteCode() {
        return this.canWrite('Code');
    }
    get Code() {
        return this.get('Code');
    }
    set Code(value) {
        this.set('Code', value);
    }
    get CanReadParents() {
        return this.canRead('Parents');
    }
    get CanWriteParents() {
        return this.canWrite('Parents');
    }
    get Parents() {
        return this.get('Parents');
    }
    AddParent(value) {
        return this.add('Parents', value);
    }
    RemoveParent(value) {
        return this.remove('Parents', value);
    }
    set Parents(value) {
        this.set('Parents', value);
    }
    get CanReadChildren() {
        return this.canRead('Children');
    }
    get Children() {
        return this.get('Children');
    }
    get CanReadDescription() {
        return this.canRead('Description');
    }
    get Description() {
        return this.get('Description');
    }
    get CanReadName() {
        return this.canRead('Name');
    }
    get Name() {
        return this.get('Name');
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
    get CanReadLocalisedDescriptions() {
        return this.canRead('LocalisedDescriptions');
    }
    get CanWriteLocalisedDescriptions() {
        return this.canWrite('LocalisedDescriptions');
    }
    get LocalisedDescriptions() {
        return this.get('LocalisedDescriptions');
    }
    AddLocalisedDescription(value) {
        return this.add('LocalisedDescriptions', value);
    }
    RemoveLocalisedDescription(value) {
        return this.remove('LocalisedDescriptions', value);
    }
    set LocalisedDescriptions(value) {
        this.set('LocalisedDescriptions', value);
    }
    get CanReadCategoryImage() {
        return this.canRead('CategoryImage');
    }
    get CanWriteCategoryImage() {
        return this.canWrite('CategoryImage');
    }
    get CategoryImage() {
        return this.get('CategoryImage');
    }
    set CategoryImage(value) {
        this.set('CategoryImage', value);
    }
    get CanReadSuperJacent() {
        return this.canRead('SuperJacent');
    }
    get SuperJacent() {
        return this.get('SuperJacent');
    }
    get CanReadCatScope() {
        return this.canRead('CatScope');
    }
    get CanWriteCatScope() {
        return this.canWrite('CatScope');
    }
    get CatScope() {
        return this.get('CatScope');
    }
    set CatScope(value) {
        this.set('CatScope', value);
    }
    get CanReadAllProducts() {
        return this.canRead('AllProducts');
    }
    get AllProducts() {
        return this.get('AllProducts');
    }
    get CanReadUniqueId() {
        return this.canRead('UniqueId');
    }
    get CanWriteUniqueId() {
        return this.canWrite('UniqueId');
    }
    get UniqueId() {
        return this.get('UniqueId');
    }
    set UniqueId(value) {
        this.set('UniqueId', value);
    }
    get CanExecuteDelete() {
        return this.canExecute('Delete');
    }
    get Delete() {
        return new framework_1.Method(this, 'Delete');
    }
}
exports.ProductCategory = ProductCategory;
//# sourceMappingURL=ProductCategory.g.js.map