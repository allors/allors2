import { MatTableDataSource, PageEvent, Sort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { Action, ActionTarget } from '../../../../angular';
import { Column } from './Column';
import { BehaviorSubject } from 'rxjs';

export abstract class BaseTable {

  columns: Column[];
  dataSource: MatTableDataSource<ActionTarget>;
  selection: SelectionModel<ActionTarget>;
  actions: Action[];

  sort$: BehaviorSubject<Sort>;
  pager$: BehaviorSubject<PageEvent>;

  total: number;

  get sortValue(): Sort {
    return this.sort$.getValue();
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

  masterToggle() {
    this.isAllSelected ?
      this.selection.clear() :
      this.dataSource.data.forEach(row => this.selection.select(row));
  }

  page(event: PageEvent): void {
    this.pager$.next(event);
  }

  sort(event: Sort): void {
    this.sort$.next(event);
  }
}
