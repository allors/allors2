import { Locale, LocalisedText } from "../../../../domain";

import { LocalisedTextComponent } from "./localisedtext.component";

export class LocalisedTextModel {
  constructor(public component: LocalisedTextComponent, public locale: Locale) {
  }

  get object(): LocalisedText {
    const all: LocalisedText[] = this.component.model;
    if (all) {
      const filtered: LocalisedText[] = all.filter((v: LocalisedText) => (v.Locale === this.locale));
      return filtered ? filtered[0] : undefined;
    }
  }

  get text(): string {
    return this.object ? this.object.Text : undefined;
  }

  set text(value: string) {
    if (!this.object) {
      const localisedText: LocalisedText = this.component.object.session.create("LocalisedText") as LocalisedText;
      localisedText.Locale = this.locale;
      this.component.object.add(this.component.roleType.name, localisedText);
    }

    this.object.Text = value;
  }

  get name(): string {
    return this.component.name + "_" + this.locale.Name;
  }

  get label(): string {
    return this.component.label + " (" + this.locale.Language.Name + ")";
  }
}
