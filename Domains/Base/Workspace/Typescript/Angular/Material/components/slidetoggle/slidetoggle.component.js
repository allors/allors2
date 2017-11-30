"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const base_angular_1 = require("@allors/base-angular");
let SlideToggleComponent = class SlideToggleComponent extends base_angular_1.Field {
};
SlideToggleComponent = __decorate([
    core_1.Component({
        selector: "a-mat-slide-toggle",
        template: `
<div style="width: 100%;" fxLayout="row">
  <mat-slide-toggle [(ngModel)]="model" [name]="name" [required]="required" [disabled]="disabled">
  {{label}}
  </mat-slide-toggle>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</div>
`,
    })
], SlideToggleComponent);
exports.SlideToggleComponent = SlideToggleComponent;
