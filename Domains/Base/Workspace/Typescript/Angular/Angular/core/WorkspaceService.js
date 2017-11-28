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
const framework_1 = require("@allors/framework");
const workspace_1 = require("@allors/workspace");
const core_1 = require("@angular/core");
let WorkspaceService = class WorkspaceService {
    constructor() {
        this.metaPopulation = new framework_1.MetaPopulation(workspace_1.data);
        this.workspace = new framework_1.Workspace(this.metaPopulation, workspace_1.constructorByName);
    }
};
WorkspaceService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [])
], WorkspaceService);
exports.WorkspaceService = WorkspaceService;
