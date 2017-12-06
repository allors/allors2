import { Component, Input, OnChanges, SimpleChange , SimpleChanges } from "@angular/core";

import { Field } from "../../../../angular";
import { Locale } from "../../../../domain";

import { LocalisedTextModel } from "./LocalisedTextModel";

@Component({
  selector: "a-mat-localised-text",
  template: `
<div style="width: 100%;" *ngFor="let model of models">
  <mat-form-field fxLayout="row">
    <input matInput [(ngModel)]="model.text" [name]="model.name" [placeholder]="model.label">
  </mat-form-field>
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
