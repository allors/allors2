import { HostBinding } from '@angular/core';

export class TestScope {
  @HostBinding('attr.data-test-scope')
  private testScope = this.constructor.name;
}
