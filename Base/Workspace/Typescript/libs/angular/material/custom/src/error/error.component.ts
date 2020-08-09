import { Component } from '@angular/core';
import { TestScope } from '@allors/angular/core';


@Component({
  templateUrl: './error.component.html',
})
export class ErrorComponent extends TestScope {

  restart() {
    location.href = '/';
  }
}
