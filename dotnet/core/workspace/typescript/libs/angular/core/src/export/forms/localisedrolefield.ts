import { Input,  Directive } from '@angular/core';
import { assert, humanize } from '@allors/meta/system';
import { Locale, LocalisedText } from '@allors/domain/generated';
import { RoleField } from './RoleField';

@Directive()
// tslint:disable-next-line: directive-class-suffix
export abstract class LocalisedRoleField extends RoleField {

  @Input()
  public locale: Locale;

  get localisedObject(): LocalisedText | null {
    const all: LocalisedText[] = this.model;
    if (all) {
      const filtered: LocalisedText[] = all.filter((v: LocalisedText) => (v.Locale === this.locale));
      return filtered?.[0] ?? null;
    }

    return null;
  }

  get localisedText(): string | null {
    return this.localisedObject?.Text ?? null;
  }

  set localisedText(value: string | null) {
    if (!this.localisedObject) {
      const localisedText: LocalisedText = this.object.session.create('LocalisedText') as LocalisedText;
      localisedText.Locale = this.locale;
      this.object.add(this.roleType, localisedText);
    }

    assert(this.localisedObject);
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
    return label + ' (' + this.locale.Language?.Name + ')';
  }
}
