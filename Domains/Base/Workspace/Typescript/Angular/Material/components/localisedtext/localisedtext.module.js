"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const common_1 = require("@angular/common");
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const localisedtext_component_1 = require("./localisedtext.component");
var localisedtext_component_2 = require("./localisedtext.component");
exports.LocalisedTextComponent = localisedtext_component_2.LocalisedTextComponent;
var LocalisedTextModel_1 = require("./LocalisedTextModel");
exports.LocalisedTextModel = LocalisedTextModel_1.LocalisedTextModel;
let LocalisedTextModule = class LocalisedTextModule {
};
LocalisedTextModule = __decorate([
    core_1.NgModule({
        declarations: [
            localisedtext_component_1.LocalisedTextComponent,
        ],
        exports: [
            localisedtext_component_1.LocalisedTextComponent,
        ],
        imports: [
            forms_1.FormsModule,
            common_1.CommonModule,
            material_1.MatInputModule,
        ],
    })
], LocalisedTextModule);
exports.LocalisedTextModule = LocalisedTextModule;
