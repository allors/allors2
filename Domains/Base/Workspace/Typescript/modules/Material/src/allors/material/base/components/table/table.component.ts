import { Component, Input } from '@angular/core';

import { Action, ActionTarget } from '../../../../angular';

import { humanize } from '../../../../angular';
import { BaseTable } from './BaseTable';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-table',
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.scss']
})
export class AllorsMaterialTableComponent {
  @Input()
  public table: BaseTable;
}
