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
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/catch");
require("rxjs/add/operator/map");
const authentication_config_1 = require("./authentication.config");
let AuthenticationService = class AuthenticationService {
    constructor(http, authenticationConfig) {
        this.http = http;
        this.authenticationConfig = authenticationConfig;
        this.tokenName = "ALLORS_JWT";
    }
    get token() {
        return sessionStorage.getItem(this.tokenName);
    }
    set token(value) {
        sessionStorage.setItem(this.tokenName, value);
    }
    login$(userName, password) {
        const url = this.authenticationConfig.url;
        const request = { userName, password };
        return this.http
            .post(url, request)
            .map((result) => {
            if (result.authenticated) {
                this.token = result.token;
            }
            return result;
        })
            .catch((error) => {
            const errMsg = error.message
                ? error.message
                : error.status
                    ? `${error.status} - ${error.statusText}`
                    : "Server error";
            return Observable_1.Observable.throw(errMsg);
        });
    }
};
AuthenticationService = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [http_1.HttpClient,
        authentication_config_1.AuthenticationConfig])
], AuthenticationService);
exports.AuthenticationService = AuthenticationService;
