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
const base_angular_1 = require("@allors/base-angular");
let StaticComponent = class StaticComponent extends base_angular_1.Field {
    get static() {
        if (this.ExistObject) {
            if (this.roleType.objectType.isUnit) {
                return this.model;
            }
            else {
                if (this.roleType.isOne) {
                    if (this.model) {
                        return this.model[this.display];
                    }
                }
                else {
                    const roles = this.model;
                    if (roles && roles.length > 0) {
                        return roles
                            .map((v) => v[this.display])
                            .reduce((acc, cur) => acc + ", " + cur);
                    }
                }
            }
        }
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], StaticComponent.prototype, "display", void 0);
StaticComponent = __decorate([
    core_1.Component({
        selector: "a-mat-static",
        template: `
<mat-form-field style="width: 100%;" fxLayout="column" fxLayoutAlign="top stretch">
  <input matInput type="type" [ngModel]="static" [name]="name" [placeholder]="label" readonly>
</mat-form-field>
`,
    })
], StaticComponent);
exports.StaticComponent = StaticComponent;
