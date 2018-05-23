import { Component } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ErrorService } from '../../allors/angular';

@Component({
  templateUrl: './dashboard.component.html'
})
export class DashboardComponent {

  constructor(private titleService: Title, private errorService: ErrorService) {
    this.titleService.setTitle('Dashboard');
  }

  public handleError() {
    this.errorService.handle(new Error('Boom'))
      .subscribe((result) => {
        console.log('ok');
      });
  }
}
