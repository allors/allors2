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
const material_1 = require("@angular/material");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const errorDialog_1 = require("./errorDialog");
let DefaultErrorService = class DefaultErrorService extends base_angular_1.ErrorService {
    constructor(dialogService, snackBar) {
        super();
        this.dialogService = dialogService;
        this.snackBar = snackBar;
    }
    message(error) {
        const message = error._body || error.message;
        this.snackBar.open(message, "close", { duration: 5000 });
    }
    dialog(error) {
        return errorDialog_1.errorDialog(this.dialogService, error);
    }
};
DefaultErrorService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [core_2.TdDialogService, material_1.MatSnackBar])
], DefaultErrorService);
exports.DefaultErrorService = DefaultErrorService;
