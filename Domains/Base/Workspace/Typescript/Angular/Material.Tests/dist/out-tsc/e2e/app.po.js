"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var protractor_1 = require("protractor");
var AngularMaterialPage = /** @class */ (function () {
    function AngularMaterialPage() {
    }
    AngularMaterialPage.prototype.navigateTo = function () {
        return protractor_1.browser.get('/');
    };
    AngularMaterialPage.prototype.getParagraphText = function () {
        return protractor_1.element(protractor_1.by.css('app-root h1')).getText();
    };
    return AngularMaterialPage;
}());
exports.AngularMaterialPage = AngularMaterialPage;
//# sourceMappingURL=app.po.js.map