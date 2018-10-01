import { Component, Output, EventEmitter, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material';

import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';
import { AllorsMaterialFilterService } from './filter.service';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter',
  templateUrl: './filter.component.html'
})
export class AllorsMaterialFilterComponent {

  @ViewChild('toolBar') toolBar: ElementRef;

  constructor(
    private dialog: MatDialog,
    private filterService: AllorsMaterialFilterService) {
  }

  public addFilter(event: MouseEvent) {

    const { offsetTop, offsetLeft, offsetWidth } = this.toolBar.nativeElement;

    const top = offsetTop;
    const left = event.pageX < offsetLeft + 60 ? event.pageX - 20 : event.pageX - 40;

    this.dialog.open(AllorsMaterialFilterDialogComponent, {
      data: { filterService: this.filterService },
      autoFocus: true,
      backdropClass: 'blah',
      position: {
        'top': `${top}px`,
        'left': `${left}px`
      }
    })
      .afterClosed()
      .subscribe();
  }
}
