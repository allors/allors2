import { HostBinding, Injectable } from '@angular/core';

@Injectable()
export abstract class TestScope {
  @HostBinding('attr.data-test-scope')
  testScope = this.constructor.name;
}
