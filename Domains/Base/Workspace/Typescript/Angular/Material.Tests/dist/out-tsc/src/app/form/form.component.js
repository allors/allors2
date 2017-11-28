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
var core_1 = require("@angular/core");
var framework_1 = require("@allors/framework");
var base_angular_1 = require("@allors/base-angular");
var FormComponent = /** @class */ (function () {
    function FormComponent(workspaceService) {
        this.workspaceService = workspaceService;
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
    }
    FormComponent.prototype.ngOnDestroy = function () {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    };
    FormComponent.prototype.ngOnInit = function () {
        this.refresh();
    };
    FormComponent.prototype.save = function () {
        this.scope.save().subscribe(function () {
            alert("saved");
        });
    };
    FormComponent.prototype.refresh = function () {
        var _this = this;
        this.scope.session.reset();
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
        var query = new framework_1.Query({
            name: "organisations",
            objectType: this.m.Organisation,
        });
        this.scope
            .load("Pull", new framework_1.PullRequest({ query: [query] }))
            .subscribe(function (loaded) {
            _this.organisation = loaded.collections.organisations[0];
        }, function (e) {
            // TODO:
            // this.allors.onError(e);
        });
    };
    FormComponent = __decorate([
        core_1.Component({
            selector: "app-form",
            templateUrl: "./form.component.html",
        }),
        __metadata("design:paramtypes", [base_angular_1.WorkspaceService])
    ], FormComponent);
    return FormComponent;
}());
exports.FormComponent = FormComponent;
//# sourceMappingURL=form.component.js.map