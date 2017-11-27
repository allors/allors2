import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { Equals, Like, Page, PullRequest, Query, Sort, TreeNode } from "@allors/framework";
import { MetaDomain } from "@allors/workspace";
import { Organisation } from "@allors/workspace";

import { ErrorService, Loaded, Scope } from "@allors/base-angular";

import { AllorsService } from "../../../../../app/allors.service";

@Component({
  templateUrl: "./organisations.component.html",
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  public title: string;

  public data: Organisation[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private allorsService: AllorsService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    private loadingService: TdLoadingService,
    public media: TdMediaService) {

      this.title = "Organisation";
      this.titleService.setTitle(this.title);
      this.scope = new Scope(allorsService.database, allorsService.workspace);
  }

  public goBack(): void {
    this.router.navigate(["/"]);
  }

  public ngAfterViewInit(): void {
    this.search();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public search(criteria?: string): void {

    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const m: MetaDomain = this.allorsService.meta;

    const query: Query[] = [new Query(
      {
        include: [
          new TreeNode({ roleType: m.Organisation.Owner }),
          new TreeNode({ roleType: m.Organisation.Employees }),
        ],
        name: "organisations",
        objectType: m.Organisation,
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load("Pull", new PullRequest({ query }))
      .subscribe((loaded: Loaded) => {
        this.data = loaded.collections.organisations as Organisation[];
      },
      (error: any) => {
        alert(error);
        this.goBack();
      });
  }

  public delete(organisation: Organisation): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this organisation?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  public onView(organisation: Organisation): void {
    this.router.navigate(["/relations/organisations/" + organisation.id + "/overview"]);
  }
}
