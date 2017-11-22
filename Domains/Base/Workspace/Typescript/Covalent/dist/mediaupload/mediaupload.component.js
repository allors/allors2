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
let MediaUploadComponent = class MediaUploadComponent extends base_angular_1.Field {
    constructor() {
        super(...arguments);
        this.selected = new core_1.EventEmitter();
    }
    get media() {
        return this.model;
    }
    get fileName() {
        return this.media ? this.media.FileName : undefined;
    }
    set fileName(value) {
        if (this.ExistObject) {
            this.media.FileName = value;
        }
    }
    delete() {
        this.model = undefined;
    }
    dropEvent(files) {
        this.selectEvent(files[0]);
    }
    selectEvent(file) {
        if (this.ExistObject) {
            if (!this.model) {
                const session = this.object.session;
                this.model = session.create("Media");
            }
        }
        const reader = new FileReader();
        const load = () => {
            this.media.FileName = file.name;
            this.media.InDataUri = reader.result;
            this.selected.emit(this.media.id);
        };
        reader.addEventListener("load", load, false);
        reader.readAsDataURL(file);
    }
};
__decorate([
    core_1.Output(),
    __metadata("design:type", core_1.EventEmitter)
], MediaUploadComponent.prototype, "selected", void 0);
__decorate([
    core_1.Input(),
    __metadata("design:type", String)
], MediaUploadComponent.prototype, "accept", void 0);
MediaUploadComponent = __decorate([
    core_1.Component({
        selector: "a-mat-media-upload",
        template: `
<div fxLayout="row">
  <mat-input-container tdFileDrop (fileDrop)="dropEvent($event)" flex>
      <input matInput [placeholder]="label" [value]="fileName" [disabled]="!this.ExistObject"/>
  </mat-input-container>

  <button mat-icon-button *ngIf="media" (click)="delete()" (keyup.enter)="delete()">
    <mat-icon>cancel</mat-icon>
  </button>

  <td-file-input #fileInput [(ngModel)]="file" color="primary" (select)="selectEvent($event)" accept="accept" [disabled]="!canWrite" [required]="required">
    <mat-icon>attach_file</mat-icon><span>Choose a file ...</span>
  </td-file-input>
</div>
`,
    })
], MediaUploadComponent);
exports.MediaUploadComponent = MediaUploadComponent;
