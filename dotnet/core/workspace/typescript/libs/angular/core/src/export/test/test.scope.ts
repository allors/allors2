import { HostBinding, Directive } from '@angular/core';

@Directive()
export abstract class TestScope {
  @HostBinding('attr.data-test-scope')
  testScope = this.constructor.name;
}
