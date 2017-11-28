"use strict";
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
const core_1 = require("@angular/core");
const forms_1 = require("@angular/forms");
const material_1 = require("@angular/material");
const platform_browser_1 = require("@angular/platform-browser");
const router_1 = require("@angular/router");
const Rx_1 = require("rxjs/Rx");
const core_2 = require("@covalent/core");
const Papa = require("papaparse");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PeopleExportComponent = class PeopleExportComponent {
    constructor(workspaceService, errorService, formBuilder, titleService, snackBar, router, dialogService, snackBarService, media, changeDetectorRef) {
        this.workspaceService = workspaceService;
        this.errorService = errorService;
        this.formBuilder = formBuilder;
        this.titleService = titleService;
        this.snackBar = snackBar;
        this.router = router;
        this.dialogService = dialogService;
        this.snackBarService = snackBarService;
        this.media = media;
        this.changeDetectorRef = changeDetectorRef;
        this.title = "Export People to CSV";
        titleService.setTitle(this.title);
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new Rx_1.BehaviorSubject(undefined);
        this.searchForm = this.formBuilder.group({
            firstName: [""],
            lastName: [""],
        });
        this.page$ = new Rx_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Rx_1.Observable
            .combineLatest(search$, this.page$, this.refresh$)
            .scan(([previousData, previousTake, previousDate], [data, take, date]) => {
            return [
                data,
                data !== previousData ? 50 : take,
                date,
            ];
        }, []);
        this.subscription = combined$
            .switchMap(([data, take]) => {
            const m = this.workspaceService.metaPopulation.metaDomain;
            const predicate = new framework_1.And();
            const predicates = predicate.predicates;
            if (data.firstName) {
                const like = "%" + data.firstName + "%";
                predicates.push(new framework_1.Like({ roleType: m.Person.FirstName, value: like }));
            }
            if (data.lastName) {
                const like = data.lastName.replace("*", "%") + "%";
                predicates.push(new framework_1.Like({ roleType: m.Person.LastName, value: like }));
            }
            const query = [new framework_1.Query({
                    name: "people",
                    objectType: m.Person,
                    predicate,
                    page: new framework_1.Page({ skip: 0, take }),
                    include: [
                        new framework_1.TreeNode({ roleType: m.Person.Picture }),
                        new framework_1.TreeNode({ roleType: m.Person.GeneralPhoneNumber }),
                    ],
                    sort: [new framework_1.Sort({ roleType: m.Person.FirstName })],
                })];
            return this.scope.load("Pull", new framework_1.PullRequest({ query }));
        })
            .subscribe((loaded) => {
            this.scope.session.reset();
            this.data = loaded.collections.people;
            this.total = loaded.values.people_total;
            this.csv = Papa.unparse({
                fields: ["FirstName", "LastName"],
                data: this.data.map((v) => ([v.FirstName, v.LastName])),
            });
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    copy() {
        const result = document.execCommand("copy");
        window.getSelection().removeAllRanges();
    }
    goBack() {
        window.history.back();
    }
    ngAfterViewInit() {
        this.media.broadcast();
        this.changeDetectorRef.detectChanges();
    }
    ngOnDestroy() {
        if (this.subscription) {
            this.subscription.unsubscribe();
        }
    }
};
PeopleExportComponent = __decorate([
    core_1.Component({
        template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">

  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <mat-input-container>
        <input fxFlex matInput placeholder="First Name" formControlName="firstName">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
      <mat-input-container>
        <input fxFlex matInput placeholder="Last Name" formControlName="lastName">
        <mat-icon matSuffix>search</mat-icon>
      </mat-input-container>
    </form>
  </div>

  <mat-divider></mat-divider>

  <mat-card-content style="min-height: 60vh">
    <ng-template tdLoading="csv">
      <mat-list class="will-load" fxLayout="column">

        <textarea #clipSource style="min-height: 60vh" [ngModel]="csv"></textarea>

        <a mat-icon-button matTooltip="Copy to clipboard" (click)="clipSource.select(); copy()">
          <mat-icon>content_copy</mat-icon>
        </a>

      </mat-list>
    </ng-template>
  </mat-card-content>

</td-layout-card-over>
`,
    }),
    __metadata("design:paramtypes", [base_angular_1.WorkspaceService,
        base_angular_1.ErrorService,
        forms_1.FormBuilder,
        platform_browser_1.Title,
        material_1.MatSnackBar,
        router_1.Router,
        core_2.TdDialogService,
        material_1.MatSnackBar,
        core_2.TdMediaService,
        core_1.ChangeDetectorRef])
], PeopleExportComponent);
exports.PeopleExportComponent = PeopleExportComponent;
