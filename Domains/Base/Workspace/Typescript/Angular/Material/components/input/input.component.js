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
var __param = (this && this.__param) || function (paramIndex, decorator) {
    return function (target, key) { decorator(target, key, paramIndex); }
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const forms_1 = require("@angular/forms");
const base_angular_1 = require("@allors/base-angular");
let InputComponent = class InputComponent extends base_angular_1.Field {
    constructor(parentForm) {
        super();
        this.parentForm = parentForm;
    }
    ngAfterViewInit() {
        if (!!this.parentForm) {
            this.controls.forEach((control) => {
                this.parentForm.addControl(control);
            });
        }
    }
};
__decorate([
    core_1.ViewChildren(forms_1.NgModel),
    __metadata("design:type", core_1.QueryList)
], InputComponent.prototype, "controls", void 0);
InputComponent = __decorate([
    core_1.Component({
        selector: "a-mat-input",
        template: `
<mat-form-field style="width: 100%;" fxLayout="column" fxLayoutAlign="top stretch">
  <input matInput [type]="textType" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-form-field>
`,
    }),
    __param(0, core_1.Optional()),
    __metadata("design:paramtypes", [forms_1.NgForm])
], InputComponent);
exports.InputComponent = InputComponent;
