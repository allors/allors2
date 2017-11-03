import { Organisation } from "@generatedDomain/index";

import { Component, OnDestroy, OnInit } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { ActivatedRoute } from "@angular/router";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { AllorsService, Loaded, Scope } from "@allors";
import { Equals, Fetch, Like, Page, Path, PullRequest, Query, Sort, TreeNode } from "@allors";

@Component({
  templateUrl: "./fetch.component.html",
})
export class FetchComponent implements OnInit, OnDestroy {

  public organisation: Organisation;
  public organisations: Organisation[];

  private scope: Scope;
  private subscription: Subscription;

  constructor(
    private title: Title,
    private route: ActivatedRoute,
    private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  public ngOnInit() {
    this.title.setTitle("Fetch");
    this.fetch();
  }

  public fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m = this.allors.m;

    const id = this.route.snapshot.paramMap.get("id");

    const fetch = [new Fetch(
      {
        name: "organisation",
        id,
        include: [new TreeNode(
          {
            roleType: m.Organisation.Owner,
          })],
      }),
      new Fetch({
        name: "organisations",
        id,
        path: new Path({
          step: m.Organisation.Owner,
          next: new Path({
            step: m.Person.OrganisationsWhereOwner
          })
        }),
        include: [new TreeNode(
          {
            roleType: m.Organisation.Owner,
          })],
      })];

    this.scope.session.reset();
    this.subscription = this.scope
      .load("Pull", new PullRequest({
        fetch,
      }))
      .subscribe((loaded: Loaded) => {
        this.organisation = loaded.objects.organisation as Organisation;
        this.organisations = loaded.collections.organisations as Organisation[];
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
