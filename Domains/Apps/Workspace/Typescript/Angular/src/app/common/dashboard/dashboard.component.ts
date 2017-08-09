import { Component, AfterViewInit, Input , ChangeDetectorRef } from '@angular/core';
import { Router } from '@angular/router';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './dashboard.component.html',
})
export class DashboardComponent implements AfterViewInit {

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
