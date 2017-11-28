"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const material_1 = require("@angular/material");
const shared_module_1 = require("../../shared.module");
const newgood_dialog_component_1 = require("./newgood-dialog-component");
var newgood_dialog_component_2 = require("./newgood-dialog-component");
exports.NewGoodDialogComponent = newgood_dialog_component_2.NewGoodDialogComponent;
let NewGoodDialogModule = class NewGoodDialogModule {
};
NewGoodDialogModule = __decorate([
    core_1.NgModule({
        declarations: [
            newgood_dialog_component_1.NewGoodDialogComponent,
        ],
        entryComponents: [
            newgood_dialog_component_1.NewGoodDialogComponent,
        ],
        exports: [
            newgood_dialog_component_1.NewGoodDialogComponent,
            material_1.MatDialogModule,
            shared_module_1.SharedModule,
        ],
        imports: [
            material_1.MatDialogModule,
            shared_module_1.SharedModule,
        ],
    })
], NewGoodDialogModule);
exports.NewGoodDialogModule = NewGoodDialogModule;
