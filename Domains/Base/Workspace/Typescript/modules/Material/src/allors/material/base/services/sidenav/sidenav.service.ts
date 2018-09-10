import { Component, Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class AllorsMaterialSideNavService {
  private toggleSource = new Subject<void>();
  public toggle$ = this.toggleSource.asObservable();

  private openSource = new Subject<void>();
  public open$ = this.openSource.asObservable();

  private closeSource = new Subject<void>();
  public close$ = this.closeSource.asObservable();

  public toggle() {
    this.toggleSource.next();
  }

  public open() {
    this.openSource.next();
  }

  public close() {
    this.closeSource.next();
  }
}
