"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const shared_module_1 = require("../../shared.module");
const salesorder_print_component_1 = require("./salesorder-print.component");
var salesorder_print_component_2 = require("./salesorder-print.component");
exports.SalesOrderPrintComponent = salesorder_print_component_2.SalesOrderPrintComponent;
let SalesOrderPrintModule = class SalesOrderPrintModule {
};
SalesOrderPrintModule = __decorate([
    core_1.NgModule({
        declarations: [
            salesorder_print_component_1.SalesOrderPrintComponent,
        ],
        exports: [
            salesorder_print_component_1.SalesOrderPrintComponent,
            shared_module_1.SharedModule,
        ],
        imports: [
            shared_module_1.SharedModule,
        ],
    })
], SalesOrderPrintModule);
exports.SalesOrderPrintModule = SalesOrderPrintModule;
