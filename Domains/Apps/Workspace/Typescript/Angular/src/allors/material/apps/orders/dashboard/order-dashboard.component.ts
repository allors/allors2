import { Component, AfterViewInit, ChangeDetectorRef } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { TdMediaService } from '@covalent/core';

@Component({
  templateUrl: './order-dashboard.component.html',
})
export class OrderDashboardComponent implements AfterViewInit {
  title: string = 'Order Dashboard';

  constructor(public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }
}
