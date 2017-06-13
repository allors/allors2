import { Observable, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { PullRequest, Query, Equals, Like, TreeNode, Sort, Page } from '../../allors/domain';
import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Organisation } from '../../allors/domain';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit, OnDestroy {

  private scope: Scope;
  private subscription: Subscription;

  form: FormGroup;
  organisation: Organisation;

  constructor(private fb: FormBuilder, private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit() {
    const m = this.allors.meta;
    this.refresh();
  }

  protected refresh(): void {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.allors.meta;

    const query = new Query({
      name: 'organisations',
      objectType: m.Organisation
    });

    this.scope
      .load('Pull', new PullRequest({ query: [query] }))
      .subscribe(() => {
        this.organisation = (this.scope.collections.organisations as Organisation[])[0];
        this.form = this.fb.group({
          Name: {}
        })
      },
      (e) => {
        this.allors.onError(e);
      });
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
