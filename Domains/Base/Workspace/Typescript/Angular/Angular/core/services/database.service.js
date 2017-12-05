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
const http_1 = require("@angular/common/http");
const core_1 = require("@angular/core");
const Database_1 = require("../Database");
const database_config_1 = require("./database.config");
let DatabaseService = class DatabaseService {
    constructor(http, databaseConfig) {
        this.http = http;
        this.databaseConfig = databaseConfig;
        this.database = new Database_1.Database(http, databaseConfig.url);
    }
};
DatabaseService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.HttpClient,
        database_config_1.DatabaseConfig])
], DatabaseService);
exports.DatabaseService = DatabaseService;
