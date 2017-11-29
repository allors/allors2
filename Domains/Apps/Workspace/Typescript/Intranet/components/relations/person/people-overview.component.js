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
const BehaviorSubject_1 = require("rxjs/BehaviorSubject");
const Observable_1 = require("rxjs/Observable");
require("rxjs/add/observable/combineLatest");
const core_2 = require("@covalent/core");
const base_angular_1 = require("@allors/base-angular");
const framework_1 = require("@allors/framework");
let PeopleOverviewComponent = class PeopleOverviewComponent {
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
        this.title = "People";
        titleService.setTitle(this.title);
        this.scope = this.workspaceService.createScope();
        this.refresh$ = new BehaviorSubject_1.BehaviorSubject(undefined);
        this.searchForm = this.formBuilder.group({
            firstName: [""],
            lastName: [""],
        });
        this.page$ = new BehaviorSubject_1.BehaviorSubject(50);
        const search$ = this.searchForm.valueChanges
            .debounceTime(400)
            .distinctUntilChanged()
            .startWith({});
        const combined$ = Observable_1.Observable
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
        }, (error) => {
            this.errorService.message(error);
            this.goBack();
        });
    }
    more() {
        this.page$.next(this.data.length + 50);
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
    refresh() {
        this.refresh$.next(new Date());
    }
    delete(person) {
        this.dialogService
            .openConfirm({ message: "Are you sure you want to delete this person?" })
            .afterClosed()
            .subscribe((confirm) => {
            if (confirm) {
                this.scope.invoke(person.Delete)
                    .subscribe((invoked) => {
                    this.snackBar.open("Successfully deleted.", "close", { duration: 5000 });
                    this.refresh();
                }, (error) => {
                    this.errorService.dialog(error);
                });
            }
        });
    }
    onView(person) {
        this.router.navigate(["/relations/peson/" + person.id]);
    }
};
PeopleOverviewComponent = __decorate([
    core_1.Component({
        template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button matTooltip="Export to CSV" [routerLink]="['/export/people']"><mat-icon>cloud_download</mat-icon></button>
  </div>
</mat-toolbar>

<mat-card>
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

  <mat-card-content>
    <ng-template tdLoading="people">
      <mat-list class="will-load">
        <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
          <h3>No people to display.</h3>
        </div>
        <ng-template let-person let-last="last" ngFor [ngForOf]="data">
          <mat-list-item>

            <mat-icon *ngIf="!person.Picture" mat-list-avatar>person</mat-icon>
            <img *ngIf="person.Picture" mat-list-avatar src="http://localhost:5000/Media/Download/{{person.Picture.UniqueId}}?revision={{person.Picture.Revision}}"
            />


            <h3 mat-line [routerLink]="['/relations/person/' + person.id]"> {{person.PartyName}} </h3>
            <p *ngIf="person.GeneralPhoneNumber" mat-line> {{person.GeneralPhoneNumber.CountryCode}} {{person.GeneralPhoneNumber.AreaCode}} {{person.GeneralPhoneNumber.ContactNumber}}

              <p mat-line hide-gt-md class="mat-caption"> last login: {{ person.LastModifiedDate | timeAgo }} </p>

              <span flex></span>

              <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ person.CreationDate | date }} </div>
              <div class="mat-caption tc-grey-500" flex-gt-xs="50"> {{ person.LastModifiedDate | timeAgo }} </div>
            </span>

              <span>
              <button mat-icon-button [mat-menu-trigger-for]="menu">
              <mat-icon>more_vert</mat-icon>
              </button>
              <mat-menu x-position="before" #menu="matMenu">
              <a [routerLink]="['/relations/person/' + person.id]" mat-menu-item>Details</a>
              <button  mat-menu-item (click)="delete(person)" [disabled]="!person.CanExecuteDelete">Delete</button>
              </mat-menu>
            </span>

          </mat-list-item>
          <mat-divider *ngIf="!last" mat-inset></mat-divider>
        </ng-template>
      </mat-list>
    </ng-template>

  </mat-card-content>
</mat-card>

<mat-card body tdMediaToggle="gt-xs" [mediaClasses]="['push']" *ngIf="this.data && this.data.length !== total">
  <mat-card-content>
    <button mat-button (click)="more()">More</button> {{this.data?.length}}/{{total}}
  </mat-card-content>
</mat-card>

<a mat-fab color="accent" class="mat-fab-bottom-right fixed" [routerLink]="['/person']">
  <mat-icon>add</mat-icon>
</a>
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
], PeopleOverviewComponent);
exports.PeopleOverviewComponent = PeopleOverviewComponent;
