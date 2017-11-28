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
const forms_1 = require("@angular/forms");
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/operator/concat");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/distinctUntilChanged");
const base_angular_1 = require("@allors/base-angular");
let AutocompleteComponent = class AutocompleteComponent extends base_angular_1.Field {
    constructor() {
        super(...arguments);
        this.display = "display";
        this.debounceTime = 400;
        this.onSelect = new core_1.EventEmitter();
        this.searchControl = new forms_1.FormControl();
    }
    ngOnInit() {
        if (this.filter) {
            this.filteredOptions = Observable_1.Observable.of(new Array())
                .concat(this.searchControl
                .valueChanges
                .debounceTime(this.debounceTime)
                .distinctUntilChanged()
                .switchMap((search) => {
                return this.filter(search);
            }));
        }
        else {
            this.filteredOptions = Observable_1.Observable.of(new Array())
                .concat(this.searchControl
                .valueChanges
                .debounceTime(this.debounceTime)
                .distinctUntilChanged()
                .map((search) => {
                const lowerCaseSearch = search.trim().toLowerCase();
                return this.options
                    .filter((v) => {
                    const optionDisplay = v[this.display] ? v[this.display].toString().toLowerCase() : undefined;
                    if (optionDisplay) {
                        return optionDisplay.indexOf(lowerCaseSearch) !== -1;
                    }
                })
                    .sort((a, b) => a[this.display] !== b[this.display] ? a[this.display] < b[this.display] ? -1 : 1 : 0);
            }));
        }
        this.searchControl.setValue(this.model);
    }
    displayFn() {
        return (val) => {
            if (val) {
                return val ? val[this.display] : "";
            }
        };
    }
    selected(option) {
        this.model = option;
        this.onSelect.emit(option);
    }
    focusout() {
        if (!this.searchControl.value) {
            this.model = undefined;
            this.onSelect.emit(undefined);
        }
        else {
            // TODO:
        }
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], AutocompleteComponent.prototype, "display", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], AutocompleteComponent.prototype, "debounceTime", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], AutocompleteComponent.prototype, "options", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Function)
], AutocompleteComponent.prototype, "filter", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], AutocompleteComponent.prototype, "onSelect", void 0);
AutocompleteComponent = __decorate([
    core_1.Component({
        selector: "a-mat-autocomplete",
        template: `
<mat-form-field fxLayout="column" fxLayoutAlign="top stretch">
  <input type="text" matInput (focusout)="focusout($event)" [formControl]="searchControl" [matAutocomplete]="usersComp" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</mat-form-field>

<mat-autocomplete #usersComp="matAutocomplete" [displayWith]="displayFn()">
  <mat-option *ngFor="let option of filteredOptions | async" [value]="option" (onSelectionChange)="selected(option)">
  {{option[this.display]}}
  </mat-option>
</mat-autocomplete>
`,
    })
], AutocompleteComponent);
exports.AutocompleteComponent = AutocompleteComponent;
