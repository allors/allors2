import { MatTableDataSource } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { Action, ActionTarget } from '../../../../angular';
import { TableConfig } from './TableConfig';
import { Column } from './Column';

export abstract class BaseTable {
  columns: Column[];
  dataSource: MatTableDataSource<ActionTarget>;
  selection: SelectionModel<ActionTarget>;
  actions: Action[];

  constructor(config?: TableConfig) {
    this.actions = (config && config.actions) || [];
  }

  get hasActions(): boolean {
    return this.actions && this.actions.length > 0;
  }

  get columnNames(): string[] {
    let result = this.columns.map((v) => v.name);
    if (this.selection) {
      result = ['select', ...result];
    }

    if (this.hasActions) {
      result = [...result, 'menu'];
    }

    return result;
  }

  get isAnySelectd() {
    return !this.selection.isEmpty();
  }

  get isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  public masterToggle() {
    this.isAllSelected ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }
}
