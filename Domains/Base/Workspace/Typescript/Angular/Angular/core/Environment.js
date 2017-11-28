"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
exports.ENVIRONMENT = new core_1.InjectionToken("environment");
// Use abstract class instead of interface.
// See https://github.com/angular/angular-cli/issues/2034#issuecomment-302666897
class Environment {
}
exports.Environment = Environment;
