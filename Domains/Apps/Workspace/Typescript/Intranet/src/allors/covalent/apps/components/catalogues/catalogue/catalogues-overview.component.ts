import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Invoked, Loaded, MediaService, Scope, WorkspaceService } from "../../../../../angular";
import { Catalogue, InternalOrganisation } from "../../../../../domain";
import { And, Equals, Fetch, Like, Page, Path, Predicate, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";

interface SearchData {
  name: string;
}

@Component({
  templateUrl: "./catalogues-overview.component.html",
})
export class CataloguesOverviewComponent implements OnDestroy {

  public title: string = "Catalogues";
  public total: number;
  public searchForm: FormGroup;
  public data: Catalogue[];
  public filtered: Catalogue[];

  private refresh$: BehaviorSubject<Date>;
  private page$: BehaviorSubject<number>;

  private subscription: Subscription;
  private scope: Scope;

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    public media: TdMediaService,
    public mediaService: MediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.titleService.setTitle(this.title);

    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      name: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$ = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$ = Observable.combineLatest(search$, this.page$, this.refresh$, this.stateService.internalOrganisationId$)
    .scan(([previousData, previousTake, previousDate, previousInternalOrganisationId], [data, take, date, internalOrganisationId]) => {
      return [
        data,
        data !== previousData ? 50 : take,
        date,
        internalOrganisationId,
      ];
    }, [] as [SearchData, number, Date, InternalOrganisation]);

    this.subscription = combined$
      .switchMap(([data, take, , internalOrganisationId]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        predicates.push(new Equals({ roleType: m.Catalogue.InternalOrganisation, value: internalOrganisationId }));

        if (data.name) {
          const like: string = data.name.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.Catalogue.Name, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: "catalogues",
            objectType: m.Catalogue,
            page: new Page({ skip: 0, take }),
            predicate,
            include: [
              new TreeNode({ roleType: m.Catalogue.CatalogueImage }),
              new TreeNode({ roleType: m.Catalogue.ProductCategories }),
            ],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));
    })
      .subscribe((loaded) => {
        this.data = loaded.collections.catalogues as Catalogue[];
        this.total = loaded.values.catalogues_total;
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  public more(): void {
    this.page$.next(this.data.length + 50);
  }

  public goBack(): void {
    window.history.back();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public delete(catalogue: Catalogue): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this catalogue?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(catalogue.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public onView(catalogue: Catalogue): void {
    this.router.navigate(["/catalogue/" + catalogue.id]);
  }
}
