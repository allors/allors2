import { Component, Input, OnChanges, SimpleChange , SimpleChanges } from "@angular/core";

import { Field } from "../../../../angular";
import { Locale } from "../../../../domain";

import { LocalisedTextModel } from "./LocalisedTextModel";

@Component({
  selector: "a-mat-localised-text",
  templateUrl: "./localisedtext.component.html",
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
