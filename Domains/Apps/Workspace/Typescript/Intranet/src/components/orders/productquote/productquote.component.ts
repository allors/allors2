import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { PostalAddress, MetaDomain, SalesOrder, SalesInvoice, Good, SalesInvoiceItem, Catalogue, Singleton, Locale, ProductCategory, CatScope, PartyContactMechanism, Enumeration, ContactMechanismType, TelecommunicationsNumber, WorkEffortAssignment, WorkEffortState, Priority, Person, WorkTask, WorkEffortPurpose, CommunicationEvent, Organisation, OrganisationContactRelationship, ContactMechanism, PersonRole, CustomerRelationship, Country, ProductCharacteristic, ProductQuote, RequestForQuote, Currency, Party, OrganisationRole } from "@allors/workspace";
import { Scope, WorkspaceService, Saved, ErrorService, Loaded, Invoked, Filter } from "@allors/base-angular";
import { Fetch, TreeNode, Path, Query, PullRequest, And, Predicate, Like, ContainedIn, Page, Sort, Equals, Contains } from "@allors/framework";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="quote" (submit)="save()">

    <div class="pad">
      <div *ngIf="quote.QuoteState">
        <a-mat-static [object]="quote" [roleType]="m.Quote.QuoteState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="quote.CanExecuteApprove" mat-button type="button" (click)="approve()">Approve</button>
        <button *ngIf="quote.CanExecuteReject" mat-button type="button" (click)="reject()">Reject</button>
      </div>

      <a-mat-static *ngIf="quote.Request" [object]="quote" [roleType]="m.ProductQuote.Request" display="RequestNumber"></a-mat-static>
      <a-mat-autocomplete *ngIf="showOrganisations" [object]="quote" [roleType]="m.ProductQuote.Receiver" [filter]="organisationsFilter.create()"
        display="Name" (onSelect)="receiverSelected($event)" label="Receiving organisation"></a-mat-autocomplete>
      <a-mat-autocomplete *ngIf="showPeople" [object]="quote" [roleType]="m.ProductQuote.Receiver" [filter]="peopleFilter.create()"
        display="displayName" (onSelect)="receiverSelected($event)" label="Receiving private person"></a-mat-autocomplete>

      <a-mat-select *ngIf="showOrganisations && !showPeople" [object]="quote" [roleType]="m.ProductQuote.ContactPerson" [options]="contacts" display="displayName"></a-mat-select>
      <button *ngIf="showOrganisations && !showPeople" type="button" mat-icon-button (click)="addPerson = true"><mat-icon>add</mat-icon></button>
      <div *ngIf="showOrganisations && addPerson" style="background: lightblue" class="pad">
        <person-inline (cancelled)="personCancelled($event)" (saved)="personAdded($event)">
        </person-inline>
      </div>

        <a-mat-select [object]="quote" [roleType]="m.ProductQuote.FullfillContactMechanism" [options]="contactMechanisms" display="displayName"></a-mat-select>
      <button *ngIf="quote.Receiver" type="button" mat-button (click)="addWebAddress = true">+Web</button>
      <button *ngIf="quote.Receiver" type="button" mat-button (click)="addEmailAddress = true">+Email</button>
      <button *ngIf="quote.Receiver" type="button" mat-button (click)="addPostalAddress = true">+Postal</button>
      <button *ngIf="quote.Receiver" type="button" mat-button (click)="addTeleCommunicationsNumber = true">+Telecom</button>

      <div *ngIf="addWebAddress && quote.Receiver" style="background: lightblue" class="pad">
        <party-contactmechanism-webaddress [scope]="scope" (cancelled)="webAddressCancelled($event)" (saved)="webAddressAdded($event)">
        </party-contactmechanism-webaddress>
      </div>
      <div *ngIf="addEmailAddress && quote.Receiver" style="background: lightblue" class="pad">
        <party-contactmechanism-emailAddress [scope]="scope" (cancelled)="emailAddressCancelled($event)" (saved)="emailAddressAdded($event)">
        </party-contactmechanism-emailAddress>
      </div>
      <div *ngIf="addPostalAddress && quote.Receiver" style="background: lightblue" class="pad">
        <party-contactmechanism-postaladdress [scope]="scope" (cancelled)="postalAddressCancelled($event)" (saved)="postalAddressAdded($event)">
        </party-contactmechanism-postaladdress>
      </div>
      <div *ngIf="addTeleCommunicationsNumber && quote.Receiver" style="background: lightblue" class="pad">
        <party-contactmechanism-telecommunicationsnumber [scope]="scope" (cancelled)="teleCommunicationsNumberCancelled($event)" (saved)="teleCommunicationsNumberAdded($event)">
        </party-contactmechanism-telecommunicationsnumber>
      </div>

      <a-mat-datepicker [object]="quote" [roleType]="m.ProductQuote.ValidFromDate"></a-mat-datepicker>
      <a-mat-datepicker [object]="quote" [roleType]="m.ProductQuote.ValidThroughDate"></a-mat-datepicker>
      <a-mat-datepicker [object]="quote" [roleType]="m.ProductQuote.IssueDate"></a-mat-datepicker>
      <a-mat-datepicker [object]="quote" [roleType]="m.ProductQuote.RequiredResponseDate"></a-mat-datepicker>
      <a-mat-input [object]="quote" [roleType]="m.ProductQuote.Description"></a-mat-input>
      <a-mat-autocomplete [object]="quote" [roleType]="m.ProductQuote.Currency" [filter]="currenciesFilter.create()" display="Name"></a-mat-autocomplete>
      <a-mat-static *ngIf="request?.Comment" [object]="request" [roleType]="m.Request.Comment" label="Request Comment"></a-mat-static>
      <a-mat-textarea [object]="quote" [roleType]="m.ProductQuote.Comment" label="Quote Comment"></a-mat-textarea>
      <a-mat-static *ngIf="request?.InternalComment" [object]="request" [roleType]="m.Request.InternalComment" label="Request Internal Comment"></a-mat-static>
      <a-mat-textarea [object]="quote" [roleType]="m.ProductQuote.InternalComment" label="Quote Internal Comment"></a-mat-textarea>
    </div>

    <mat-divider></mat-divider>

    <mat-card-actions>
      <button mat-button color="primary" type="submit" [disabled]="!form.form.valid">SAVE</button>
      <button mat-button (click)="goBack()" type="button">CANCEL</button>
    </mat-card-actions>

  </form>
</td-layout-card-over>
`,
})
export class ProductQuoteEditComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public quote: ProductQuote;
  public request: RequestForQuote;
  public people: Person[];
  public organisations: Organisation[];
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public contacts: Person[];

  public addEmailAddress: boolean = false;
  public addPostalAddress: boolean = false;
  public addTeleCommunicationsNumber: boolean = false;
  public addWebAddress: boolean = false;
  public addPerson: boolean = false;

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  private previousReceiver: Party;

  get showOrganisations(): boolean {
    return !this.quote.Receiver || this.quote.Receiver instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.quote.Receiver || this.quote.Receiver instanceof (Person);
  }

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService, private changeDetectorRef: ChangeDetectorRef) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.peopleFilter = new Filter({scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
    this.organisationsFilter = new Filter({scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.currenciesFilter = new Filter({scope: this.scope, objectType: this.m.Currency, roleTypes: [this.m.Currency.Name]});
  }

  public ngOnInit(): void {
    const route$: Observable<UrlSegment[]> = this.route.url;

    const combined$: Observable<[UrlSegment[], Date]> = Observable.combineLatest(route$, this.refresh$);

    this.subscription = combined$
      .switchMap(([urlSegments, date]: [UrlSegment[], Date]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const rolesQuery: Query[] = [
          new Query(
            {
              name: "organisationRoles",
              objectType: m.OrganisationRole,
            }),
          new Query(
            {
              name: "personRoles",
              objectType: m.PersonRole,
            }),
          new Query(
            {
              name: "currencies",
              objectType: this.m.Currency,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();
            this.currencies = loaded.collections.currencies as Currency[];

            const organisationRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const oCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Customer");

            const personRoles: OrganisationRole[] = loaded.collections.organisationRoles as OrganisationRole[];
            const pCustomerRole: OrganisationRole = organisationRoles.find((v: OrganisationRole) => v.Name === "Customer");

            const query: Query[] = [
              new Query(
                {
                  name: "organisations",
                  objectType: this.m.Organisation,
                  predicate: new Contains({ roleType: m.Organisation.OrganisationRoles, object: oCustomerRole }),
                }),
              new Query(
                {
                  name: "persons",
                  objectType: this.m.Person,
                  predicate: new Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                }),
            ];

            const fetch: Fetch[] = [
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.ProductQuote.Receiver }),
                  new TreeNode({ roleType: m.ProductQuote.FullfillContactMechanism }),
                  new TreeNode({ roleType: m.ProductQuote.QuoteState }),
                  new TreeNode({ roleType: m.ProductQuote.Request }),
                ],
                name: "productQuote",
              }),
            ];

            return this.scope.load("Pull", new PullRequest({ fetch, query }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.quote = loaded.objects.productQuote as ProductQuote;

        if (!this.quote) {
          this.quote = this.scope.session.create("ProductQuote") as ProductQuote;
          this.quote.IssueDate = new Date();
          this.title = "Add Quote";
        } else {
          this.title = "Quote " + this.quote.QuoteNumber;
        }

        if (this.quote.Receiver) {
          this.title = this.title + " from: " + this.quote.Receiver.PartyName;
        }

        this.receiverSelected(this.quote.Receiver);

        this.previousReceiver = this.quote.Receiver;
        this.organisations = loaded.collections.organisations as Organisation[];
        this.people = loaded.collections.parties as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public personCancelled(): void {
    this.addPerson = false;
  }

  public personAdded(id: string): void {
    this.addPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.quote.Receiver as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
  }

  public webAddressCancelled(): void {
    this.addWebAddress = false;
  }

  public webAddressAdded(id: string): void {
    this.addWebAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public emailAddressCancelled(): void {
    this.addEmailAddress = false;
  }

  public emailAddressAdded(id: string): void {
    this.addEmailAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public postalAddressCancelled(): void {
    this.addPostalAddress = false;
  }

  public postalAddressAdded(id: string): void {
    this.addPostalAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public teleCommunicationsNumberCancelled(): void {
    this.addTeleCommunicationsNumber = false;
  }

  public teleCommunicationsNumberAdded(id: string): void {
    this.addTeleCommunicationsNumber = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.request.Originator.AddPartyContactMechanism(partyContactMechanism);
  }

  public approve(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.quote.Approve)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully approved.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                submitFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            submitFn();
          }
        });
    } else {
      submitFn();
    }
  }

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.quote.Reject)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully rejected.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                rejectFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
  }

  public Order(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.quote.Order)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("SalesOrder successfully created.", "close", { duration: 5000 });
        },
        (error: Error) => {
          this.errorService.dialog(error);
        });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .openConfirm({ message: "Save changes?" })
        .afterClosed().subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                rejectFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            rejectFn();
          }
        });
    } else {
      rejectFn();
    }
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

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/orders/productQuote/" + this.quote.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public receiverSelected(party: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        id: party.id,
        include: [
          new TreeNode({
            nodes: [
              new TreeNode({
                nodes: [
                  new TreeNode({ roleType: this.m.PostalBoundary.Country }),
                ],
                roleType: this.m.PostalAddress.PostalBoundary,
              }),
            ],
            roleType: this.m.PartyContactMechanism.ContactMechanism,
          }),
        ],
        name: "partyContactMechanisms",
        path: new Path({ step: this.m.Party.CurrentPartyContactMechanisms }),
      }),
      new Fetch({
        id: party.id,
        name: "currentContacts",
        path: new Path({ step: this.m.Party.CurrentContacts }),
      }),
    ];

    this.scope
      .load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded: Loaded) => {

        if (this.quote.Receiver !== this.previousReceiver) {
          this.quote.ContactPerson = null;
          this.quote.FullfillContactMechanism = null;
          this.previousReceiver = this.quote.Receiver;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.contactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.contacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
}
