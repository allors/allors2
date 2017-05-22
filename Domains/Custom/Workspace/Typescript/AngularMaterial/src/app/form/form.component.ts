import { Observable, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';

import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Person } from '../../allors/domain';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, OnDestroy {

  private scope: Scope;

  people: Person[];
  selected: Person;

  private subscription: Subscription;

  constructor(private allors: AllorsService) {
    this.scope = new Scope('People', allors.database, allors.workspace);
  }

  ngOnInit() {
    this.refresh().subscribe();
  }

  protected refresh(): Observable<any> {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    return this.scope
      .load({})
      .do(() => {
        this.people = this.scope.collections.people as Person[];
        this.people = this.people.filter(v => v.FirstName);
      })
      .catch((e) => {
        this.allors.onError(e);
        return Observable.empty();
      });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
