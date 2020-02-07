import { Component, Optional, ViewChild, NgZone, ElementRef } from '@angular/core';
import { NgForm } from '@angular/forms';
import { CdkTextareaAutosize } from '@angular/cdk/text-field';
import { take } from 'rxjs/operators';

import * as marked from 'marked';

import { Test, RoleField } from '../../../../../angular';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-markdown',
  templateUrl: './markdown.component.html',
})
@Test
export class AllorsMaterialMarkdownComponent extends RoleField {

  @ViewChild('autosize') autosize: CdkTextareaAutosize;

  @ViewChild('heightSource') heightSource: ElementRef;

  get height(): string {
    if (this.heightSource && this.heightSource.nativeElement) {
      return this.heightSource.nativeElement.getBoundingClientRect().height + 10;
    }
  }

  get html(): string {
    if (this.model) {
      return marked.parser(marked.lexer(this.model));
    }
  }

  constructor(
    @Optional() parentForm: NgForm,
    private ngZone: NgZone) {
    super(parentForm);
  }

  triggerResize() {
    // Wait for changes to be applied, then trigger textarea resize.
    this.ngZone.onStable.pipe(take(1))
      .subscribe(() => this.autosize.resizeToFitContent(true));
  }
}
