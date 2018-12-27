import { Component, ViewChild, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material';

import { AllorsFilterService } from '../../../../angular/base/filter';
import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';
import { FilterField } from '../../../../angular/base/filter/FilterField';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter',
  templateUrl: './filter.component.html'
})
export class AllorsMaterialFilterComponent {

  @ViewChild('toolBar') toolBar: ElementRef;

  constructor(
    public filterService: AllorsFilterService,
    private dialog: MatDialog) {
  }

  get hasFields() {
    return this.filterService.filterFields.length > 0;
  }

  clear() {
    this.filterService.clearFilterFields();
  }

  remove(field: FilterField) {
    this.filterService.removeFilterField(field);
  }

  add(event: MouseEvent) {

    const { offsetTop, offsetLeft, offsetWidth } = this.toolBar.nativeElement;

    const top = offsetTop;
    const twoThirds = offsetLeft + offsetWidth * 2 / 3;

    let left = event.pageX < offsetLeft + 60 ? event.pageX - 20 : event.pageX - 40;
    left = left < twoThirds ? left : twoThirds;

    this.dialog.open(AllorsMaterialFilterDialogComponent, {
      data: { filterService: this.filterService },
      autoFocus: true,
      backdropClass: 'blah',
      position: {
        'top': `${top}px`,
        'left': `${left}px`
      }
    });
  }
}
