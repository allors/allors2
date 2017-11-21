"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
class Field {
    get ExistObject() {
        return !!this.object;
    }
    get model() {
        return this.ExistObject ? this.object.get(this.roleType.name) : undefined;
    }
    set model(value) {
        if (this.ExistObject) {
            this.object.set(this.roleType.name, value);
        }
    }
    get canRead() {
        if (this.ExistObject) {
            return this.object.canRead(this.roleType.name);
        }
    }
    get canWrite() {
        if (this.ExistObject) {
            return this.object.canWrite(this.roleType.name);
        }
    }
    get textType() {
        if (this.roleType.objectType.name === "Integer" ||
            this.roleType.objectType.name === "Decimal" ||
            this.roleType.objectType.name === "Float") {
            return "number";
        }
        return "text";
    }
    get name() {
        return this.roleType.name;
    }
    get label() {
        return this.assignedLabel ? this.assignedLabel : this.humanize(this.roleType.name);
    }
    get required() {
        return this.assignedRequired ? this.assignedRequired : this.roleType.isRequired;
    }
    get disabled() {
        return !this.canWrite || this.assignedDisabled;
    }
    humanize(input) {
        return input ? input.replace(/([a-z\d])([A-Z])/g, "$1 $2")
            .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, "$1 $2")
            : undefined;
    }
}
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], Field.prototype, "object", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Object)
], Field.prototype, "roleType", void 0);
__decorate([
    core_1.Input("disabled"),
    __metadata("design:type", Boolean)
], Field.prototype, "assignedDisabled", void 0);
__decorate([
    core_1.Input("required"),
    __metadata("design:type", Boolean)
], Field.prototype, "assignedRequired", void 0);
__decorate([
    core_1.Input("label"),
    __metadata("design:type", String)
], Field.prototype, "assignedLabel", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], Field.prototype, "readonly", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], Field.prototype, "hint", void 0);
exports.Field = Field;
