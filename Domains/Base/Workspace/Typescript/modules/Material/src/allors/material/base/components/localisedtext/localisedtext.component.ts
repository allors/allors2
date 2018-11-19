import { Component, Input, OnChanges, Optional , SimpleChange, SimpleChanges } from '@angular/core';
import { NgForm } from '@angular/forms';

import { Field, humanize } from '../../../../angular';
import { Locale, LocalisedText } from '../../../../domain';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-localised-text',
  templateUrl: './localisedtext.component.html',
})
export class AllorsMaterialLocalisedTextComponent extends Field {

  @Input()
  public locale: Locale;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  get localisedObject(): LocalisedText {
    const all: LocalisedText[] = this.model;
    if (all) {
      const filtered: LocalisedText[] = all.filter((v: LocalisedText) => (v.Locale === this.locale));
      return filtered ? filtered[0] : undefined;
    }
  }

  get localisedText(): string {
    return this.localisedObject ? this.localisedObject.Text : undefined;
  }

  set localisedText(value: string) {
    if (!this.localisedObject) {
      const localisedText: LocalisedText = this.object.session.create('LocalisedText') as LocalisedText;
      localisedText.Locale = this.locale;
      this.object.add(this.roleType.name, localisedText);
    }

    this.localisedObject.Text = value;
  }

  get localisedName(): string {
    return this.name + '_' + this.locale.Name;
  }

  get localisedLabel(): string {
    let name = this.roleType.name;
    const localised = 'Localised';
    if (name.indexOf(localised) === 0) {
      name = name.slice(localised.length);
      name = name.slice(0, name.length - 1);
    }

    const label = this.assignedLabel ? this.assignedLabel : humanize(name);
    return label + ' (' + this.locale.Language.Name + ')';
  }
}
