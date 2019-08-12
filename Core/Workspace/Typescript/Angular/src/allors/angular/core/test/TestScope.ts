import { HostBinding, Component } from '@angular/core';

// See https://github.com/angular/angular/issues/30080
@Component({ template: '' })
export abstract class TestScope {
  @HostBinding('attr.data-test-scope')
  private testScope = this.constructor.name;
}
