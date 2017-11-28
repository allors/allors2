"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const inline_module_1 = require("../../inline.module");
const shared_module_1 = require("../../shared.module");
const invoiceitem_component_1 = require("./invoiceitem.component");
var invoiceitem_component_2 = require("./invoiceitem.component");
exports.InvoiceItemEditComponent = invoiceitem_component_2.InvoiceItemEditComponent;
let InvoiceItemEditModule = class InvoiceItemEditModule {
};
InvoiceItemEditModule = __decorate([
    core_1.NgModule({
        declarations: [
            invoiceitem_component_1.InvoiceItemEditComponent,
        ],
        exports: [
            invoiceitem_component_1.InvoiceItemEditComponent,
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
        imports: [
            inline_module_1.InlineModule,
            shared_module_1.SharedModule,
        ],
    })
], InvoiceItemEditModule);
exports.InvoiceItemEditModule = InvoiceItemEditModule;
