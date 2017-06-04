import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Query, Equals } from '../../allors/domain';
import { Scope } from '../../allors/angular';
import { AllorsService } from '../allors.service';

@Component({
  templateUrl: './query.component.html'
})
export class QueryComponent implements OnInit, OnDestroy {
  scope: Scope;
  private subscription: Subscription;

  constructor(private title: Title, private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit() {
    this.title.setTitle('Query');

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const metaPopulation = this.allors.workspace.metaPopulation;

    const query = new Query();
    query.name = 'people';
    query.type = metaPopulation.objectTypeByName['Person'];

    const equals = new Equals();
    equals.roleType = query.type.roleTypeByName['LastName'];
    equals.value = 'Doe';
    query.predicate = equals;

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Query', [query])
      .subscribe();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
