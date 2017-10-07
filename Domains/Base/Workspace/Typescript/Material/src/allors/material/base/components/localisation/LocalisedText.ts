import { Component, Input, OnChanges, SimpleChanges, SimpleChange , ChangeDetectorRef } from '@angular/core';
import { ISession, ISessionObject, Media } from '../../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../../allors/meta';

import { Field } from '../../../../angular';
import { Locale, LocalisedText } from '../../../../domain';

class LocalisedTextModel {
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
      const localisedText: LocalisedText = this.component.object.session.create('LocalisedText') as LocalisedText;
      localisedText.Locale = this.locale;
      this.component.object.add(this.component.roleType.name, localisedText);
    }

    this.object.Text = value;
  }

  get name(): string {
    return this.component.name + '_' + this.locale.Name;
  }

  get label(): string {
    return this.component.label + ' (' + this.locale.Name + ')';
  }
}

@Component({
  selector: 'a-mat-localised-text',
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
  locales: Locale[];

  models: LocalisedTextModel[];

  ngOnChanges(changes: SimpleChanges): void {
    const changedLocales: SimpleChange = changes.locales;
    if (changedLocales) {
      this.models = this.locales.map((v: Locale) => new LocalisedTextModel(this, v));
    }
  }
}
