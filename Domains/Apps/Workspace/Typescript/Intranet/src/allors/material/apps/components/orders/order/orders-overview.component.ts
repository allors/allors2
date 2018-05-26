import { AfterViewInit, ChangeDetectorRef, Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './orders-overview.component.html',
})
export class OrdersOverviewComponent {
  public title = 'Orders Dashboard';

  constructor(private dialogService: AllorsMaterialDialogService, private titleService: Title) {
    this.titleService.setTitle(this.title);
  }
}
