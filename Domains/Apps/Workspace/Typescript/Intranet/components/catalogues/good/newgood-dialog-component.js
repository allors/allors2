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
const material_1 = require("@angular/material");
let NewGoodDialogComponent = class NewGoodDialogComponent {
    constructor(dialogRef, data) {
        this.dialogRef = dialogRef;
        this.data = data;
    }
    onCancelClick() {
        this.dialogRef.close();
    }
};
NewGoodDialogComponent = __decorate([
    core_1.Component({
        selector: "newgood-dialog",
        template: `
  <h4 mat-dialog-title>Choose Product</h4>
  <p>Select 'Serialised' if the prodcut is unique and has a serialnumber.</p>
  <p>Select 'NonSerialised' if the product is a group of items and you want to keep stock.</p>

  <div mat-dialog-content>
  <mat-form-field>
    <mat-select [value]="data.chosenGood" [(ngModel)]="data.chosenGood">
      <mat-option value="Serialised">Serialised</mat-option>
      <mat-option value="NonSerialised">NonSerialised</mat-option>
    </mat-select>
    </mat-form-field>
  </div>

  <div mat-dialog-actions align="end">
    <button mat-button (click)="onCancelClick()">Cancel</button>
    <button mat-button [mat-dialog-close]="data.chosenGood" >OK</button>
  </div>
  `,
    }),
    __param(1, core_1.Inject(material_1.MAT_DIALOG_DATA)),
    __metadata("design:paramtypes", [material_1.MatDialogRef, Object])
], NewGoodDialogComponent);
exports.NewGoodDialogComponent = NewGoodDialogComponent;
