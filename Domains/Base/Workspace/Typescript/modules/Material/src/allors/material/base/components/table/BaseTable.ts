import { MatTableDataSource, PageEvent, Sort } from '@angular/material';
import { SelectionModel } from '@angular/cdk/collections';

import { Action } from '../../../../angular';
import { Column } from './Column';
import { BehaviorSubject } from 'rxjs';
import { TableRow } from './TableRow';
import { ISessionObject } from 'src/allors/framework';

export abstract class BaseTable {

  columns: Column[];
  dataSource: MatTableDataSource<TableRow>;
  selection: SelectionModel<TableRow>;
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

  get anySelected() {
    return !this.selection.isEmpty();
  }

  get allSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  get selected(): ISessionObject[] {
    return this.selection.selected.map((v => v.object));
  }

  masterToggle() {
    this.allSelected ?
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
