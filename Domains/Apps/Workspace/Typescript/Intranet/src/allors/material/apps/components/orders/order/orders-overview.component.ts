import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { DialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './orders-overview.component.html',
})
export class OrdersOverviewComponent {
  public title = 'Orders Dashboard';

  constructor(private dialogService: DialogService, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }
}
