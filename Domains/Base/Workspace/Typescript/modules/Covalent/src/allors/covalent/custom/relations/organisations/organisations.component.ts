import { AfterViewInit, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";
import { Observable, Subject, Subscription } from "rxjs/Rx";

import { ErrorService, Loaded, Scope, WorkspaceService } from "../../../../angular";
import { Organisation } from "../../../../domain";
import { MetaDomain } from "../../../../meta";
import { Query, TreeNode, PullRequest } from "../../../../framework";

@Component({
  templateUrl: "./organisations.component.html",
})
export class OrganisationsComponent implements AfterViewInit, OnDestroy {

  public title: string;

  public m: MetaDomain;
  public data: Organisation[];

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private titleService: Title,
    private router: Router,
    private dialogService: TdDialogService,
    private loadingService: TdLoadingService,
    public media: TdMediaService) {

      this.title = "Organisation";
      this.scope = this.workspaceService.createScope();
      this.m = this.workspaceService.metaPopulation.metaDomain;
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

    const queries = [new Query(
      {
        include: [
          new TreeNode({ roleType: this.m.Organisation.Owner }),
          new TreeNode({ roleType: this.m.Organisation.Employees }),
        ],
        name: "organisations",
        objectType: this.m.Organisation,
      })];

    this.scope.session.reset();

    this.subscription = this.scope
      .load("Pull", new PullRequest({ queries }))
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
