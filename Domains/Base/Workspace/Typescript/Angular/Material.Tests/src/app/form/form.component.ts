import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subscription } from "rxjs/Rx";

import { ObjectTyped, PullRequest, Query } from "@allors/framework";
import { MetaDomain, Organisation } from "@allors/workspace";

import { Scope, WorkspaceService } from "@allors/base-angular";

@Component({
  selector: "app-form",
  templateUrl: "./form.component.html",
})
export class FormComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public organisation: Organisation;

  private scope: Scope;
  private subscription: Subscription;

  constructor(private workspaceService: WorkspaceService) {
    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public ngOnInit() {
    this.refresh();
  }

  public save(): void {
    this.scope.save().subscribe(() => {
      alert("saved");
    });
  }

  protected refresh(): void {
    this.scope.session.reset();

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const query = new Query({
      name: "organisations",
      objectType: this.m.Organisation as ObjectTyped,
    });

    this.scope
      .load("Pull", new PullRequest({ query: [query] }))
      .subscribe((loaded) => {
        this.organisation = (loaded.collections.organisations as Organisation[])[0];
      },
      (e) => {
        // TODO:
        // this.allors.onError(e);
      });
  }
}
