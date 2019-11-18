import { Component, Input, OnChanges, Optional, SimpleChange, SimpleChanges, ViewChild, NgZone } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';

import * as marked from 'marked';

import { RoleField, humanize } from '../../../../../angular';
import { Locale, LocalisedText } from '../../../../../domain';
import { take } from 'rxjs/operators';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-localised-markdown',
  templateUrl: './localisedmarkdown.component.html',
})
export class AllorsMaterialLocalisedMarkdownComponent extends RoleField {

  @ViewChild('autosize', { static: false }) autosize: CdkTextareaAutosize;

  @Input()
  public locale: Locale;

  constructor(
    @Optional() parentForm: NgForm,
    private ngZone: NgZone) {
    super(parentForm);
  }

  get html(): string {
    if (this.localisedText) {
      return marked.parser(marked.lexer(this.localisedText));
    }
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
      this.object.add(this.roleType, localisedText);
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

  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this.ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
