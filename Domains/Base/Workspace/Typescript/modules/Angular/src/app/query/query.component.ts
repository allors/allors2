import { Component, OnDestroy, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { Loaded, Scope, WorkspaceService } from "../../allors/angular";
import { Organisation, Person } from "../../allors/domain";
import { Equals, Like, Page, PullRequest, Query, Sort, TreeNode } from "../../allors/framework";

@Component({
  templateUrl: "./query.component.html",
})
export class QueryComponent implements OnInit, OnDestroy {

  public organisations: Organisation[];

  public organisationCount: number;
  public skip = 5;
  public take = 5;

  private scope: Scope;
  private subscription: Subscription;

  constructor(private title: Title, private workspaceService: WorkspaceService) {
    this.scope = workspaceService.createScope();
  }

  public ngOnInit() {
    this.title.setTitle("Query");
    this.query();
  }

  public query() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.workspaceService.metaPopulation.metaDomain;

    // tslint:disable:object-literal-sort-keys
    const query = new Query(
      {
        name: "organisations",
        objectType: m.Organisation,
        predicate: new Like(
          {
            roleType: m.Organisation.Name,
            value: "Org%",
          }),
        include: [new TreeNode(
          {
            roleType: m.Organisation.Owner,
          })],
        sort: [new Sort(
          {
            roleType: m.Organisation.Name,
            direction: "Asc",
          })],
        page: new Page({
          skip: this.skip || 0,
          take: this.take || 10,
        }),
      });

    this.scope.session.reset();
    this.subscription = this.scope
      .load("Pull", new PullRequest({
        query: [query],
      }))
      .subscribe((loaded: Loaded) => {
        this.organisations = loaded.collections.organisations as Organisation[];
        this.organisationCount = loaded.values.organisations_count;
      },
        (error) => {
          alert(error);
        });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
