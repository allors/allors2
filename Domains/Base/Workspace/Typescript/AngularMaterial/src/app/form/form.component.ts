import { Observable, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';

import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../allors/domain';
import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Organisation } from '../../allors/domain';

import { Form } from '../../allors/domain/base/forms'

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, OnDestroy {

  private scope: Scope;

  public form: Form;

  organisation: Organisation;

  private subscription: Subscription;

  constructor(private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit() {
    this.refresh().subscribe();
    this.form = new Form({

    });
  }

  protected refresh(): Observable<any> {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.allors.meta;

    const query = new Query({
      name: 'organisations',
      objectType: m.Organisation
    });

    return this.scope
      .load('Pull', new PullRequest({query: [query]}))
      .do(() => {
        this.organisation = (this.scope.collections.organisations as Organisation[])[0];
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
