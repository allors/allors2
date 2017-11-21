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
const LocalisedTextModel_1 = require("./LocalisedTextModel");
let LocalisedTextComponent = class LocalisedTextComponent extends base_angular_1.Field {
    ngOnChanges(changes) {
        const changedLocales = changes.locales;
        if (changedLocales) {
            this.models = this.locales.map((v) => new LocalisedTextModel_1.LocalisedTextModel(this, v));
        }
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], LocalisedTextComponent.prototype, "locales", void 0);
LocalisedTextComponent = __decorate([
    core_1.Component({
        selector: "a-mat-localised-text",
        template: `
<div>
  <div *ngFor="let model of models">
    <mat-form-field fxLayout="row">
      <input matInput [(ngModel)]="model.text" [name]="model.name" [placeholder]="model.label">
    </mat-form-field>
  </div>
</div>
`,
    })
], LocalisedTextComponent);
exports.LocalisedTextComponent = LocalisedTextComponent;
