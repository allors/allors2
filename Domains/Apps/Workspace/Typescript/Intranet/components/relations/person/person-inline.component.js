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
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PersonInlineComponent = class PersonInlineComponent {
    constructor(workspaceService, errorService) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.saved = new core_1.EventEmitter();
        this.cancelled = new core_1.EventEmitter();
        this.scope = this.workspaceService.createScope();
        this.m = this.workspaceService.metaPopulation.metaDomain;
    }
    ngOnInit() {
        const query = [
            new framework_1.Query({
                name: "locales",
                objectType: this.m.Locale,
            }),
            new framework_1.Query({
                name: "genders",
                objectType: this.m.GenderType,
            }),
            new framework_1.Query({
                name: "salutations",
                objectType: this.m.Salutation,
            }),
            new framework_1.Query({
                name: "roles",
                objectType: this.m.PersonRole,
            }),
        ];
        this.scope
            .load("Pull", new framework_1.PullRequest({ query }))
            .subscribe((loaded) => {
            this.locales = loaded.collections.locales;
            this.genders = loaded.collections.genders;
            this.salutations = loaded.collections.salutations;
            this.roles = loaded.collections.roles;
            this.person = this.scope.session.create("Person");
        }, (error) => {
            this.cancelled.emit();
        });
    }
    cancel() {
        this.cancelled.emit();
    }
    save() {
        this.scope
            .save()
            .subscribe((saved) => {
            this.saved.emit(this.person.id);
        }, (error) => {
            this.errorService.dialog(error);
        });
    }
};
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], PersonInlineComponent.prototype, "saved", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], PersonInlineComponent.prototype, "cancelled", void 0);
PersonInlineComponent = __decorate([
    core_1.Component({
        selector: "person-inline",
        template: `
  <a-mat-select [object]="person" [roleType]="m.Person.PersonRoles" [options]="roles" display="Name"></a-mat-select>
  <a-mat-input [object]="person" [roleType]="m.Person.FirstName" ></a-mat-input>
  <a-mat-input [object]="person" [roleType]="m.Person.MiddleName" ></a-mat-input>
  <a-mat-input [object]="person" [roleType]="m.Person.LastName" ></a-mat-input>
  <a-mat-select [object]="person" [roleType]="m.Person.Gender" [options]="genders" display="Name" ></a-mat-select>
  <a-mat-select [object]="person" [roleType]="m.Person.Salutation" [options]="salutations" display="Name"></a-mat-select>
  <a-mat-select [object]="person" [roleType]="m.Person.Locale" [options]="locales" display="Name"></a-mat-select>

  <button mat-button color="primary" type="button" (click)="save()">Save</button>
  <button mat-button color="secondary" type="button"(click)="cancel()">Cancel</button>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService, base_angular_1.ErrorService])
], PersonInlineComponent);
exports.PersonInlineComponent = PersonInlineComponent;
