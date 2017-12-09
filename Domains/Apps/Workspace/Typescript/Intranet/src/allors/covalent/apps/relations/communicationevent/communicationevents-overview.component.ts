import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { MatSnackBar } from "@angular/material";
import { Title } from "@angular/platform-browser";
import { Router } from "@angular/router";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { TdDialogService, TdMediaService } from "@covalent/core";

import { ErrorService, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../angular";
import { Brand, Catalogue, CatScope, ContactMechanism, Currency, Facility, Good, InventoryItemKind, InventoryItemVariance, Locale, LocalisedText, Model, NonSerialisedInventoryItem, NonSerialisedInventoryItemState, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductCategory, ProductCharacteristic, ProductCharacteristicValue, ProductFeature, ProductType, SalesInvoice, SalesInvoiceItem, SalesOrder, Singleton, VarianceReason, VatRate, VatRegime, RequestForQuote, ProductQuote, QuoteItem, SalesOrderItem, ProcessFlow, Store, CommunicationEvent, CommunicationEventState, CommunicationEventPurpose } from "../../../../domain";
import { And, ContainedIn, Contains, Fetch, Like, Page, Path, Predicate, PullRequest, Query, Sort, TreeNode, Equals } from "../../../../framework";
import { MetaDomain } from "../../../../meta";

interface SearchData {
  subject: string;
  state: string;
  purpose: string;
  involved: string;
}

@Component({
  template: `
<mat-toolbar>
  <div layout="row" layout-align="start center" flex>
    <button mat-icon-button tdLayoutManageListOpen [hideWhenOpened]="true" style="display: none">
          <mat-icon>arrow_back</mat-icon>
        </button>
    <span>{{title}}</span>
    <span flex></span>
    <button mat-icon-button><mat-icon>settings</mat-icon></button>
  </div>
</mat-toolbar>

<mat-card *ngIf="communicationEventStates">
  <div class="pad-top-xs pad-left pad-right">
    <form novalidate [formGroup]="searchForm">
      <div class="grid-8_xs-1">
        <mat-input-container class="col">
          <input matInput placeholder="Subject" formControlName="subject">
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="state" name="state" [(ngModel)]="selectedCommunicationEventState" placeholder="State">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let objectState of communicationEventStates" [value]="objectState.Name">{{ objectState.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="purpose" name="purpose" [(ngModel)]="selectedPurpose" placeholder="Purpose">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let purpose of purposes" [value]="purpose.Name">{{ purpose.Name }}</mat-option>
          </mat-select>
        </mat-input-container>
        <mat-input-container class="col">
          <mat-select formControlName="involved" name="involved" [(ngModel)]="selectedInvolded" placeholder="Person Involved">
            <mat-option>None</mat-option>
            <mat-option *ngFor="let person of people" [value]="person.displayName">{{ person.displayName }}</mat-option>
          </mat-select>
          <mat-icon matSuffix>search</mat-icon>
          </mat-input-container>
      </div>
    </form>
  </div>

  <mat-divider></mat-divider>

  <mat-card-content>
    <ng-template tdLoading="data">
      <mat-list class="will-load">
        <div class="mat-padding" *ngIf="data && data.length === 0" layout="row" layout-align="center center">
          <h3>No communication events to display.</h3>
        </div>
        <ng-template let-communicationEvent let-last="last" ngFor [ngForOf]="data">
          <mat-list-item>
                 <div *ngIf="this.checkType(communicationEvent) === 'FaceToFaceCommunication'" mat-line class="mat-caption pointer"
                 [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/' + communicationEvent.id ]">
                  Meeting, {{ communicationEvent.CommunicationEventState.Name }}</div>

                <div *ngIf="this.checkType(communicationEvent) === 'PhoneCommunication'" mat-line class="mat-caption pointer"
                [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/' + communicationEvent.id ]">
                  Phone call, {{ communicationEvent.CommunicationEventState.Name }}</div>

                <div *ngIf="this.checkType(communicationEvent) === 'EmailCommunication'" mat-line class="mat-caption pointer"
                [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/' + communicationEvent.id ]">
                  Email, {{ communicationEvent.CommunicationEventState.Name }}</div>

                <div *ngIf="this.checkType(communicationEvent) === 'LetterCorrespondence'" mat-line class="mat-caption pointer"
                [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/' + communicationEvent.id ]">
                  Letter, {{ communicationEvent.CommunicationEventState.Name }}</div>

                <p mat-line class="mat-caption">State: {{ communicationEvent.CommunicationEventState.Name }} </p>
                <p mat-line class="mat-caption">Subject: {{ communicationEvent.Subject }} </p>
                <p mat-line class="mat-caption">Involved:
                  <span *ngFor="let party of communicationEvent.InvolvedParties">{{ party.displayName }} </span>
                </p>

                <span hide-xs hide-sm hide-md flex-gt-xs="60" flex-xs="40" layout-gt-xs="row">
                  <div class="mat-caption tc-grey-500" flex-gt-xs="50"> started: {{ communicationEvent.ActualStart | date}} </div>
                  <div class="mat-caption tc-grey-500" flex-gt-xs="50"> ended: {{ communicationEvent.ActualEnd | date }} </div>
                  </span>

                 <span>
                    <button mat-icon-button [mat-menu-trigger-for]="menu">
                      <mat-icon>more_vert</mat-icon>
                    </button>
                    <mat-menu x-position="before" #menu="matMenu">
                         <button mat-menu-item [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/' + communicationEvent.id ]">Details</button>
                        <button *ngIf="this.checkType(communicationEvent) === 'EmailCommunication'" mat-menu-item [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/emailcommunication/' + communicationEvent.id ]">Edit</button>
                        <button *ngIf="this.checkType(communicationEvent) === 'FaceToFaceCommunication'" mat-menu-item [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/facetofacecommunication/' + communicationEvent.id ]">Edit</button>
                        <button *ngIf="this.checkType(communicationEvent) === 'LetterCorrespondence'" mat-menu-item [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/lettercorrespondence/' + communicationEvent.id ]">Edit</button>
                        <button *ngIf="this.checkType(communicationEvent) === 'PhoneCommunication'" mat-menu-item [routerLink]="['/relations/party/' + communicationEvent.InvolvedParties[0].id + '/communicationevent/emailcommunication/' + communicationEvent.id ]">Edit</button>
                        <button [disabled]="!communicationEvent.CanExecuteDelete" mat-menu-item (click)="delete(communicationEvent)">Delete</button>
                        <button [disabled]="!communicationEvent.CanExecuteCancel" mat-menu-item (click)="cancel(communicationEvent)">Cancel</button>
                        <button [disabled]="!communicationEvent.CanExecuteClose" mat-menu-item (click)="close(communicationEvent)">Close</button>
                        <button [disabled]="!communicationEvent.CanExecuteReopen" mat-menu-item (click)="reopen(communicationEvent)">Reopen</button>
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
`,
})
export class CommunicationEventsOverviewComponent implements AfterViewInit, OnDestroy {

  public total: number;

  public title: string = "Communications";

  public searchForm: FormGroup;

  public data: CommunicationEvent[];

  public communicationEventStates: CommunicationEventState[];
  public selectedCommunicationEventState: CommunicationEventState;
  public communicationEventState: CommunicationEventState;

  public purposes: CommunicationEventPurpose[];
  public selectedPurpose: CommunicationEventPurpose;
  public purpose: CommunicationEventPurpose;

  public people: Person[];
  public selectedInvolved: Person;
  public involved: Person;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;

  private page$: BehaviorSubject<number>;

  constructor(
    private workspaceService: WorkspaceService,
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
    this.scope = this.workspaceService.createScope();
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.searchForm = this.formBuilder.group({
      involved: [""],
      purpose: [""],
      state: [""],
      subject: [""],
    });

    this.page$ = new BehaviorSubject<number>(50);

    const search$: Observable<SearchData> = this.searchForm.valueChanges
      .debounceTime(400)
      .distinctUntilChanged()
      .startWith({});

    const combined$: Observable<any> = Observable
    .combineLatest(search$, this.page$, this.refresh$)
    .scan(([previousData, previousTake, previousDate]: [SearchData, number, Date], [data, take, date]: [SearchData, number, Date]): [SearchData, number, Date] => {
      return [
        data,
        data !== previousData ? 50 : take,
        date,
      ];
    }, [] as [SearchData, number, Date]);

    this.subscription = combined$
      .switchMap(([data, take]: [SearchData, number]) => {
        const m: MetaDomain = this.workspaceService.metaPopulation.metaDomain;

        const objectStatesQuery: Query[] = [
          new Query(
            {
              name: "communicationEventStates",
              objectType: m.CommunicationEventState,
            }),
          new Query(
            {
              name: "purposes",
              objectType: m.CommunicationEventPurpose,
            }),
          new Query(
            {
              name: "persons",
              objectType: m.Person,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: objectStatesQuery }))
          .switchMap((loaded: Loaded) => {
            this.communicationEventStates = loaded.collections.communicationEventStates as CommunicationEventState[];
            this.communicationEventState = this.communicationEventStates.find((v: CommunicationEventState) => v.Name === data.state);

            this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
            this.purpose = this.purposes.find((v: CommunicationEventPurpose) => v.Name === data.purpose);

            this.people = loaded.collections.persons as Person[];
            this.involved = this.people.find((v: Person) => v.displayName === data.involved);

            const predicate: And = new And();
            const predicates: Predicate[] = predicate.predicates;

            if (data.subject) {
              const like: string = "%" + data.subject + "%";
              predicates.push(new Like({ roleType: m.CommunicationEvent.Subject, value: like }));
            }

            if (data.state) {
              predicates.push(new Equals({ roleType: m.CommunicationEvent.CommunicationEventState, value: this.communicationEventState }));
            }

            if (data.purpose) {
              predicates.push(new Contains({ roleType: m.CommunicationEvent.EventPurposes, object: this.purpose }));
            }

            if (data.involved) {
              predicates.push(new Contains({ roleType: m.CommunicationEvent.InvolvedParties, object: this.involved }));
            }

            const communicationsQuery: Query[] = [
              new Query(
                {
                  include: [
                    new TreeNode({ roleType: m.CommunicationEvent.CommunicationEventState }),
                    new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
                    new TreeNode({ roleType: m.CommunicationEvent.InvolvedParties }),
                  ],
                  name: "communicationEvents",
                  objectType: m.CommunicationEvent,
                  predicate,
                  page: new Page({ skip: 0, take }),
                }),
            ];

            return this.scope
              .load("Pull", new PullRequest({ query: communicationsQuery }));
          });
      })
      .subscribe((loaded: Loaded) => {

        this.scope.session.reset();

        this.data = loaded.collections.communicationEvents as CommunicationEvent[];
        this.total = loaded.values.communicationEvents_total;
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

  public ngAfterViewInit(): void {
    this.media.broadcast();
    this.changeDetectorRef.detectChanges();
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public cancel(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to cancel this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Cancel)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public close(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to close this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Close)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully closed.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public reopen(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to reopen this?" })
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Reopen)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully reopened.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
  }

  public delete(communicationEvent: CommunicationEvent): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to delete this communication event?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(communicationEvent.Delete)
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

  public checkType(obj: any): string {
    return obj.objectType.name;
  }

  public onView(person: Person): void {
    this.router.navigate(["/relations/person/" + person.id]);
  }
}
