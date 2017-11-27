import { Component, OnDestroy, OnInit } from "@angular/core";
import { Subscription } from "rxjs/Rx";

import { PullRequest, Query } from "@allors/framework";
import { MetaDomain, Organisation } from "@allors/workspace";

import { Scope } from "@allors/base-angular";

import { AllorsService } from "../allors.service";

@Component({
  selector: "app-form",
  templateUrl: "./form.component.html",
})
export class FormComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public organisation: Organisation;

  private scope: Scope;
  private subscription: Subscription;

  constructor(private allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
    this.m = allors.meta;
  }

  public ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public ngOnInit() {
    const m = this.allors.meta;
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

    const m = this.allors.meta;

    const query = new Query({
      name: "organisations",
      objectType: m.Organisation,
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
