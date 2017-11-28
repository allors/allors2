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
const Subject_1 = require("rxjs/Subject");
require("rxjs/add/operator/debounceTime");
require("rxjs/add/operator/distinctUntilChanged");
require("rxjs/add/operator/do");
const base_angular_1 = require("@allors/base-angular");
let ChipsComponent = class ChipsComponent extends base_angular_1.Field {
    constructor(parentForm) {
        super();
        this.parentForm = parentForm;
        this.display = "display";
        this.debounceTime = 400;
        this.onAdd = new core_1.EventEmitter();
        this.onRemove = new core_1.EventEmitter();
    }
    ngOnInit() {
        this.subject = new Subject_1.Subject();
        if (this.filter) {
            this.subscription = this.subject
                .debounceTime(this.debounceTime)
                .distinctUntilChanged()
                .switchMap((search) => {
                return this.filter(search);
            })
                .do((options) => {
                this.filteredOptions = options.filter((v) => this.model.indexOf(v) < 0);
            })
                .subscribe();
        }
        else {
            this.subscription = this.subject
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
            })
                .do((options) => {
                this.filteredOptions = options;
            })
                .subscribe();
        }
    }
    ngAfterViewInit() {
        this.controls.forEach((control) => {
            this.parentForm.addControl(control);
        });
    }
    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
    add(object) {
        this.onAdd.emit(object);
    }
    remove(object) {
        this.onRemove.emit(object);
    }
    inputChange(search) {
        this.subject.next(search);
    }
};
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], ChipsComponent.prototype, "display", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Number)
], ChipsComponent.prototype, "debounceTime", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Array)
], ChipsComponent.prototype, "options", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", Function)
], ChipsComponent.prototype, "filter", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ChipsComponent.prototype, "onAdd", void 0);
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], ChipsComponent.prototype, "onRemove", void 0);
__decorate([
    core_1.ViewChildren(forms_1.NgModel),
    __metadata("design:type", core_1.QueryList)
], ChipsComponent.prototype, "controls", void 0);
ChipsComponent = __decorate([
    core_1.Component({
        selector: "a-td-chips",
        template: `
<td-chips [name]="name"
          [items]="filteredOptions"
          [(ngModel)]="model"
          [placeholder]="label"
          (inputChange)="inputChange($event)"
          (add)="add($event)"
          (remove)="remove($event)"
          requireMatch
          [disabled]="disabled"
          [readOnly]="readonly"
          [required]="required">

  <ng-template td-chip let-chip="chip">
    {{chip[this.display]}}
  </ng-template>
  <ng-template td-autocomplete-option let-option="option">
    <div layout="row" layout-align="start center">
      {{option[this.display]}}
    </div>
  </ng-template>
  <mat-hint *ngIf="hint">{{hint}}</mat-hint>
</td-chips>
`,
    }),
    __metadata("design:paramtypes", [forms_1.NgForm])
], ChipsComponent);
exports.ChipsComponent = ChipsComponent;
