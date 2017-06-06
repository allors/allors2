import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Title } from '@angular/platform-browser';

import { Query, Equals, Like, TreeNode, Sort } from '../../allors/domain';
import { Scope } from '../../allors/angular';
import { AllorsService } from '../allors.service';

import { Organisation, Person } from '../../allors/domain';

@Component({
  templateUrl: './query.component.html'
})
export class QueryComponent implements OnInit, OnDestroy {

  organisations: Organisation[];

  private scope: Scope;
  private subscription: Subscription;

  constructor(private title: Title, private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit() {
    this.title.setTitle('Query');

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const organisation = this.allors.workspace.metaPopulation.objectTypeByName['Organisation'];
    const person = this.allors.workspace.metaPopulation.objectTypeByName['Person'];

    const query = new Query(
      {
        name: 'organisations',
        objectType: organisation,
        predicate: new Like(
          {
            roleType: organisation.roleTypeByName['Name'],
            value: 'Org%'
          }),
        fetch: [new TreeNode(
          {
            roleType: organisation.roleTypeByName['Owner'],
          })],
        sort: [new Sort(
          {
            roleType: organisation.roleTypeByName['Name'],
            direction: 'Desc'
          })]
      });

    this.scope.session.reset();
    this.subscription = this.scope
      .load('Query', [query])
      .subscribe(() => {
        this.organisations = this.scope.collections.organisations as Organisation[];
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
