import { Component, Input, OnChanges, SimpleChange , SimpleChanges } from "@angular/core";

import { Field } from "@allors/base-angular";
import { Locale } from "@allors/generated";

import { LocalisedTextModel } from "./LocalisedTextModel";

@Component({
  selector: "a-mat-localised-text",
  template: `
<div>
  <div *ngFor="let model of models">
    <mat-form-field fxLayout="row">
      <input matInput [(ngModel)]="model.text" [name]="model.name" [placeholder]="model.label">
    </mat-form-field>
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
