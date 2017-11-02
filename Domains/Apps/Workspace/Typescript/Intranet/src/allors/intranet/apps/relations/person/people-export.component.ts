import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { MatSnackBar, MatSnackBarConfig } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable, Subject, Subscription } from "rxjs/Rx";

import { TdDialogService, TdLoadingService, TdMediaService } from "@covalent/core";

import * as Papa from "papaparse";

import { And, ContainedIn, Contains, Equals, Like, Not, Or, Page, Predicate, PullRequest, Query, Sort, TreeNode } from "@allors";
import { MetaDomain } from "@allors";
import { OrganisationContactRelationship, Person } from "@allors";
import { AllorsService, ErrorService, Invoked, Loaded, Saved, Scope } from "@allors";

interface SearchData {
  firstName: string;
  lastName: string;
}

@Component({
  templateUrl: "./people-export.component.html",
})
export class PeopleExportComponent implements AfterViewInit, OnDestroy {

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;
  total: number;

  title: string = "Export People to CSV";
  subTitle: string;

  searchForm: FormGroup;

  data: Person[];
  csv: string;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private formBuilder: FormBuilder,
    private titleService: Title,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: TdDialogService,
    private snackBarService: MatSnackBar,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef) {

    titleService.setTitle(this.title);
    this.scope = new Scope(allors.database, allors.workspace);
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      firstName: [""],
      lastName: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable.combineLatest(search$, this.page$)
      .scan(([previousData, previousTake]: [SearchData, number], [data, take]: [SearchData, number]): [SearchData, number] => {
        return [
          data,
          data !== previousData ? 50 : take,
        ];
      }, [] as [SearchData, number]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.allors.meta;

        const predicate: And = new And();
        const predicates: Predicate[] = predicate.predicates;

        if (data.firstName) {
          const like: string = "%" + data.firstName + "%";
          predicates.push(new Like({ roleType: m.Person.FirstName, value: like }));
        }

        if (data.lastName) {
          const like: string = data.lastName.replace("*", "%") + "%";
          predicates.push(new Like({ roleType: m.Person.LastName, value: like }));
        }

        const query: Query[] = [new Query(
          {
            name: "people",
            objectType: m.Person,
            predicate,
            page: new Page({ skip: 0, take }),
            include: [
              new TreeNode({ roleType: m.Person.Picture }),
              new TreeNode({ roleType: m.Person.GeneralPhoneNumber }),
            ],
            sort: [new Sort({ roleType: m.Person.FirstName })],
          })];

        return this.scope.load("Pull", new PullRequest({ query }));

      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.people as Person[];
        this.total = loaded.values.people_total;

        this.csv = Papa.unparse({
          fields: ["FirstName", "LastName"],
          data: this.data.map((v: Person) => ([v.FirstName, v.LastName])),
        });
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      });
  }

  copy(): void {
    const result: boolean = document.execCommand("copy");
    window.getSelection().removeAllRanges();
  }

  goBack(): void {
    window.history.back();
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
