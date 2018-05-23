import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';


@Component({
  templateUrl: './orders-overview.component.html',
})
export class OrdersOverviewComponent {
  public title = 'Orders Dashboard';

  constructor(private changeDetectorRef: ChangeDetectorRef, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }
}
