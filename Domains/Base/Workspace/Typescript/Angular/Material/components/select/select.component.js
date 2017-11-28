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
let SelectComponent = class SelectComponent extends base_angular_1.Field {
    constructor(parentForm) {
        super();
        this.parentForm = parentForm;
        this.display = "display";
        this.onSelect = new core_1.EventEmitter();
    }
    ngAfterViewInit() {
        if (!!this.parentForm) {
            this.controls.forEach((control) => {
                this.parentForm.addControl(control);
            });
        }
    }
    selected(option) {
        this.onSelect.emit(option);
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], SelectComponent.prototype, "display", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], SelectComponent.prototype, "options", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], SelectComponent.prototype, "onSelect", void 0);
__decorate([
    core_1.ViewChildren(forms_1.NgModel),
    __metadata("design:type", core_1.QueryList)
], SelectComponent.prototype, "controls", void 0);
SelectComponent = __decorate([
    core_1.Component({
        selector: "a-mat-select",
        template: `
<mat-form-field>
    <mat-select [(ngModel)]="model" [name]="name" [placeholder]="label" [multiple]="roleType.isMany" [required]="required" [disabled]="disabled">
    <mat-option *ngIf="!required">None</mat-option>
    <mat-option *ngFor="let option of options" [value]="option" (onSelectionChange)="selected(option)">
        {{option[display]}}
      </mat-option>
    </mat-select>
</mat-form-field>
`,
    }),
    __param(0, core_1.Optional()),
    __metadata("design:paramtypes", [forms_1.NgForm])
], SelectComponent);
exports.SelectComponent = SelectComponent;
