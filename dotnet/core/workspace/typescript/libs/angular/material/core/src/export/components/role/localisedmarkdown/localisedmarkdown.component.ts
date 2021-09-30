import { AfterViewInit, Component, ElementRef, ViewEncapsulation, Optional, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

import { LocalisedRoleField } from '@allors/angular/core';
import * as EasyMDE from 'easymde';

@Component({
  // tslint:disable-next-line: component-selector
  selector: 'a-mat-localised-markdown',
  template: `
    <h4>{{ localisedLabel }}</h4>
    <textarea #easymde></textarea>
  `,
  encapsulation: ViewEncapsulation.None,
})
export class AllorsMaterialLocalisedMarkdownComponent extends LocalisedRoleField implements AfterViewInit {
  @ViewChild('easymde', { static: true })
  elementRef: ElementRef;

  easyMDE: EasyMDE;

  constructor(@Optional() parentForm: NgForm) {
    super(parentForm);
  }

  ngAfterViewInit() {
    this.easyMDE = new EasyMDE({
      element: this.elementRef.nativeElement,
    });

    this.easyMDE.value(this.localisedText ?? '');
    this.easyMDE.codemirror.on('change', () => {
      this.localisedText = this.easyMDE.value();
    });
  }
}
