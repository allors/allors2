"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
// Allors generated file.
// Do not edit this file, changes will be overwritten.
/* tslint:disable */
const framework_1 = require("../../framework");
class Dependent extends framework_1.SessionObject {
    get CanExecuteDelete() {
        return this.canExecute('Delete');
    }
    get Delete() {
        return new framework_1.Method(this, 'Delete');
    }
}
exports.Dependent = Dependent;
//# sourceMappingURL=Dependent.g.js.map