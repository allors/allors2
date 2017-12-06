"use strict";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : new P(function (resolve) { resolve(result.value); }).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
Object.defineProperty(exports, "__esModule", { value: true });
const domain_1 = require("../../src/allors/domain");
const framework_1 = require("../../src/allors/framework");
const meta_1 = require("../../src/allors/meta");
const promise_1 = require("../../src/allors/promise");
const AxiosHttp_1 = require("../../src/allors/promise/base/http/AxiosHttp");
const chai_1 = require("chai");
require("mocha");
describe("People", () => {
    let scope;
    beforeEach(() => __awaiter(this, void 0, void 0, function* () {
        const metaPopulation = new framework_1.MetaPopulation(meta_1.data);
        const workspace = new framework_1.Workspace(metaPopulation, domain_1.constructorByName);
        const http = new AxiosHttp_1.AxiosHttp("http://localhost:5000/");
        yield http.login("TestAuthentication/Token", "administrator");
        const database = new promise_1.Database(http);
        scope = new promise_1.Scope(database, workspace);
    }));
    describe("Load", () => {
        it("should return all people", () => __awaiter(this, void 0, void 0, function* () {
            const loaded = yield scope.load("People");
            const people = loaded.collections.people;
            chai_1.assert.equal(7, people.length);
            // people.forEach((v) => console.log(v.FullName));
        }));
    });
});
//# sourceMappingURL=Person.spec.js.map