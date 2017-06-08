import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Title } from '@angular/platform-browser';

import { PullRequest, Fetch, Query, Equals, Like, TreeNode, Sort, Page } from '../../allors/domain';
import { Scope } from '../../allors/angular';
import { AllorsService } from '../allors.service';

import { Organisation, Person } from '../../allors/domain';

@Component({
  templateUrl: './fetch.component.html'
})
export class FetchComponent implements OnInit, OnDestroy {

  organisation: Organisation;

  private scope: Scope;
  private subscription: Subscription;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit() {
    this.title.setTitle('Fetch');
    this.fetch();
  }

  fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.allors.meta;

    const id = this.route.snapshot.paramMap.get('id');

    const fetch = new Fetch(
      {
        name: 'organisation',
        id: id,
        include: [new TreeNode(
          {
            roleType: m.Organisation.Owner,
          })],
      });

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Pull', new PullRequest({
        fetch: [fetch],
      }))
      .subscribe(() => {
        this.organisation = this.scope.objects.organisation as Organisation;
      },
      (error) => {
        alert(error);
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
