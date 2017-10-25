import { ChangeDetectorRef, Component, Input, OnChanges, SimpleChange , SimpleChanges } from "@angular/core";
import { Field } from "@baseAngular";
import { ISession, ISessionObject } from "@baseDomain";
import {  RoleType } from "@baseMeta";

import { Locale, LocalisedText } from "@generatedDomain/index";

import { LocalisedTextModel } from "./LocalisedTextModel"

@Component({
  selector: "a-mat-localised-text",
  template: `
<div>
  <div *ngFor="let model of models">
    <mat-input-container fxLayout="row">
      <input matInput [(ngModel)]="model.text" [name]="model.name" [placeholder]="model.label">
    </mat-input-container>
  </div>
</div>
`,
})
export class LocalisedTextComponent extends Field implements OnChanges {
  @Input()
  public locales: Locale[];

  public models: LocalisedTextModel[];

  public ngOnChanges(changes: SimpleChanges): void {
    const changedLocales: SimpleChange = changes.locales;
    if (changedLocales) {
      this.models = this.locales.map((v: Locale) => new LocalisedTextModel(this, v));
    }
  }
}
