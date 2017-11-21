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
const authentication_service_1 = require("./authentication.service");
let AuthenticationInterceptor = class AuthenticationInterceptor {
    constructor(injector) {
        this.injector = injector;
    }
    intercept(req, next) {
        // Lazy inject AuthenticationService to prevent cyclic dependency on HttpClient
        const authenticationService = this.injector.get(authentication_service_1.AuthenticationService);
        const token = authenticationService.token;
        if (token) {
            const authReq = req.clone({
                headers: req.headers.set("Authorization", "Bearer " + token),
            });
            return next.handle(authReq);
        }
        return next.handle(req);
    }
};
AuthenticationInterceptor = __decorate([
    core_1.Injectable(),
    __metadata("design:paramtypes", [core_1.Injector])
], AuthenticationInterceptor);
exports.AuthenticationInterceptor = AuthenticationInterceptor;
