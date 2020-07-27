import { Component, ViewChild, ElementRef, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';

import { FilterBuilder } from '../../../../angular/core/filter';
import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';
import { FilterField } from '../../../../angular/core/filter/FilterField';
import { ObjectType } from 'src/allors/framework';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter',
  templateUrl: './filter.component.html',
})
export class AllorsMaterialFilterComponent {
  @Input() builder: FilterBuilder;

  @ViewChild('toolBar', { static: true }) toolBar: ElementRef;

  constructor(private dialog: MatDialog) {}

  get hasFields() {
    return this.builder.filterFields.length > 0;
  }

  clear() {
    this.builder.clearFilterFields();
  }

  remove(field: FilterField) {
    this.builder.removeFilterField(field);
  }

  add(event: MouseEvent) {
    const { offsetTop, offsetLeft, offsetWidth } = this.toolBar.nativeElement;

    const top = offsetTop;
    const twoThirds = offsetLeft + (offsetWidth * 2) / 3;

    let left = event.pageX < offsetLeft + 60 ? event.pageX - 20 : event.pageX - 40;
    left = left < twoThirds ? left : twoThirds;

    this.dialog.open(AllorsMaterialFilterDialogComponent, {
      data: { filterBuilder: this.builder },
      autoFocus: true,
      backdropClass: 'nada',
      position: {
        top: `${top}px`,
        left: `${left}px`,
      },
      maxHeight: '90vh',
    });
  }
}
