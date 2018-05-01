import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Invoked, Loaded, MediaService, PdfService, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { WorkEffort, WorkTask } from "../../../../../domain";
import { Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  templateUrl: "./worktask-overview.component.html",
})
export class WorkTaskOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string = "Task Overview";
  public task: WorkTask;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private dialogService: TdDialogService,
    private snackBar: MatSnackBar,
    public media: TdMediaService,
    public mediaService: MediaService,
    public pdfService: PdfService,
    private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$)
      .switchMap(([urlSegments, date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const fetches: Fetch[] = [
          new Fetch({
            id,
            include: [
              new TreeNode({ roleType: m.WorkTask.WorkEffortState }),
              new TreeNode({ roleType: m.WorkTask.Customer }),
              new TreeNode({ roleType: m.WorkTask.FullfillContactMechanism }),
              new TreeNode({ roleType: m.WorkTask.ContactPerson }),
            ],
            name: "task",
          }),
        ];

        const queries: Query[] = [
        ];

        return this.scope
          .load("Pull", new PullRequest({ fetches, queries }));
        })
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.task = loaded.objects.task as WorkTask;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public cancel(): void {
    this.scope.invoke(this.task.Cancel)
      .subscribe((invoked: Invoked) => {
        this.refresh();
        this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public print() {
    this.pdfService.display(this.task);
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  public checkType(obj: any): string {
    return obj.objectType.name;
  }
}
