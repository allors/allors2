import { Component, Self, Input, Output, EventEmitter, OnDestroy } from '@angular/core';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-search',
  templateUrl: './search.component.html',
  styles: [

    `
    ::ng-deep .mat-form-field.mat-focused {
      outline: unset !important;
    }
    ::ng-deep .mat-form-field-underline {
      background: none !important;
      border-bottom: 1px dotted #e0e0e0;
    }
`
  ]
})
export class AllorsMaterialSearchComponent {
  @Output() public filter: EventEmitter<string> = new EventEmitter<string>();
}
