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
let RadioGroupComponent = class RadioGroupComponent extends base_angular_1.Field {
    get keys() {
        return Object.keys(this.options);
    }
    optionLabel(option) {
        return option.label ? option.label : this.humanize(option.value.toString());
    }
    optionValue(option) {
        return option.value;
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], RadioGroupComponent.prototype, "options", void 0);
RadioGroupComponent = __decorate([
    core_1.Component({
        selector: "a-mat-radio-group",
        template: `
<mat-form-field style="width: 100%;" fxLayout="row">
  <mat-radio-group [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
    <mat-radio-button *ngFor="let option of options" [value]="optionValue(option)">{{optionLabel(option)}}</mat-radio-button>
  </mat-radio-group>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-form-field>
`,
    })
], RadioGroupComponent);
exports.RadioGroupComponent = RadioGroupComponent;
