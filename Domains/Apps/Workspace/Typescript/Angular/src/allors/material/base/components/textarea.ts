import { Component, Input, ChangeDetectorRef, ViewChildren, QueryList, AfterViewInit } from '@angular/core';
import { NgModel, NgForm } from '@angular/forms';
import { ISessionObject } from '../../../../allors/domain';
import { MetaDomain, RoleType } from '../../../../allors/meta';

import { Field } from '../../../angular';

@Component({
  selector: 'a-md-textarea',
  template: `
<md-input-container fxLayout="column" fxLayoutAlign="top stretch">
  <textarea fxFlex mdInput [(ngModel)]="model" [name]="name" [placeholder]="label" [required]="required" [disabled]="disabled" [readonly]="readonly">
  </textarea>
  <md-hint *ngIf="hint">{{hint}}</md-hint>
</md-input-container>
`,
})
export class TextareaComponent extends Field implements AfterViewInit {
  @ViewChildren(NgModel) controls: QueryList<NgModel>;

  constructor(private parentForm: NgForm) {
    super();
  }

  ngAfterViewInit(): void {
    this.controls.forEach((control: NgModel) => {
      this.parentForm.addControl(control);
    });
  }
}
