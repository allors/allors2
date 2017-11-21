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
const forms_1 = require("@angular/forms");
const base_angular_1 = require("@allors/base-angular");
let DatepickerComponent = class DatepickerComponent extends base_angular_1.Field {
    constructor(parentForm) {
        super();
        this.parentForm = parentForm;
    }
    ngAfterViewInit() {
        this.controls.forEach((control) => {
            this.parentForm.addControl(control);
        });
    }
    get hours() {
        if (this.model) {
            return this.model.getHours();
        }
    }
    set hours(value) {
        if (this.model) {
            this.model.setHours(value);
        }
    }
    get minutes() {
        if (this.model) {
            return this.model.getMinutes();
        }
    }
    set minutes(value) {
        if (this.model) {
            this.model.setMinutes(value);
        }
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", Boolean)
], DatepickerComponent.prototype, "useTime", void 0);
__decorate([
    core_1.ViewChildren(forms_1.NgModel),
    __metadata("design:type", typeof (_a = typeof core_1.QueryList !== "undefined" && core_1.QueryList) === "function" && _a || Object)
], DatepickerComponent.prototype, "controls", void 0);
DatepickerComponent = __decorate([
    core_1.Component({
        selector: "a-mat-datepicker",
        template: `
<div fxLayout="row">
  <mat-form-field fxLayoutGap="1em">
    <input matInput [matDatepicker]="picker" [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
    <mat-datepicker-toggle matSuffix [for]="picker"></mat-datepicker-toggle>
    <mat-datepicker #picker></mat-datepicker>
    <mat-hint *ngIf="hint">{{hint}}</mat-hint>
  </mat-form-field>

  <mat-form-field *ngIf="this.model && useTime">
    <input matInput matInput type="number" min="0" max="23" [(ngModel)]="hours" [name]="hours" placeholder="hours" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </mat-form-field>

  <mat-form-field *ngIf="this.model && useTime">
    <input matInput matInput type="number" min="0" max="59" [(ngModel)]="minutes" [name]="minutes" placeholder="minutes" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </mat-form-field>
</div>
<mat-datepicker #picker></mat-datepicker>
`,
    }),
    __metadata("design:paramtypes", [typeof (_b = typeof forms_1.NgForm !== "undefined" && forms_1.NgForm) === "function" && _b || Object])
], DatepickerComponent);
exports.DatepickerComponent = DatepickerComponent;
var _a, _b;
