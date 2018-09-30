import { Component, Output, EventEmitter, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { MatDialog } from '@angular/material';

import { AllorsMaterialFilterDialogComponent } from './filter-dialog.component';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'a-mat-filter',
  templateUrl: './filter.component.html'
})
export class AllorsMaterialFilterComponent {

  @ViewChild('dialogPosition') dialogPosition: ElementRef;

  constructor(private dialog: MatDialog) {
  }

  public addFilter(event: MouseEvent) {

    const { offsetTop, offsetLeft, offsetWidth } = this.dialogPosition.nativeElement;

    const top = offsetTop;
    const middle = offsetLeft + offsetWidth / 2;

    // TODO: improve position algorithm
    const left = event.pageX < middle ? event.pageX > 60 ? event.pageX - 20 : 40 : middle;

    this.dialog.open(AllorsMaterialFilterDialogComponent, {
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
