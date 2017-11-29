import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";
import 'rxjs/add/observable/combineLatest';

import { MetaDomain, SalesInvoice, SalesOrder, Currency, ContactMechanism, VatRate, VatRegime, Party, OrganisationRole, OrganisationContactRelationship, PartyContactMechanism } from "@allors/workspace";
import { Organisation, Person } from "@allors/workspace";
import { Filter, Scope, WorkspaceService, ErrorService, Loaded, Invoked, Saved } from "@allors/base-angular";
import { Query, PullRequest, Contains, Fetch, TreeNode, Path } from "@allors/framework";

@Component({
  template: `
<td-layout-card-over [cardTitle]="title" [cardSubtitle]="subTitle">
  <form #form="ngForm" *ngIf="invoice" (submit)="save()">

    <div class="pad">
      <div *ngIf="invoice.SalesInvoiceState">
        <a-mat-static [object]="invoice" [roleType]="m.SalesInvoice.SalesInvoiceState" display="Name" label="Status"></a-mat-static>
        <button *ngIf="invoice.CanExecuteSend" mat-button type="button" (click)="send()">Send</button>
        <button *ngIf="invoice.CanExecuteSend" mat-button type="button" (click)="cancel()">Cancel</button>
        <button *ngIf="invoice.CanExecuteWriteOff" mat-button type="button" (click)="writeOff()">Write off</button>
      </div>

      <a-mat-static *ngIf="invoice.SalesOrder" [object]="invoice" [roleType]="m.SalesInvoice.SalesOrder"></a-mat-static>
      <a-mat-autocomplete *ngIf="showOrganisations" [object]="invoice" [roleType]="m.SalesInvoice.BillToCustomer" [filter]="organisationsFilter.create()"
        display="Name" (onSelect)="receiverSelected($event)" label="Bill to organisation"></a-mat-autocomplete>
      <a-mat-autocomplete *ngIf="showPeople" [object]="invoice" [roleType]="m.SalesInvoice.BillToCustomer" [filter]="peopleFilter.create()"
        display="displayName" (onSelect)="receiverSelected($event)" label="Bill to person"></a-mat-autocomplete>

      <a-mat-select *ngIf="showOrganisations && !showPeople" [object]="invoice" [roleType]="m.SalesInvoice.ContactPerson" [options]="contacts" display="displayName"></a-mat-select>
      <button *ngIf="showOrganisations && !showPeople" type="button" mat-icon-button (click)="addPerson = true"><mat-icon>add</mat-icon></button>
      <div *ngIf="showOrganisations && addPerson" style="background: lightblue" class="pad">
        <person-inline (cancelled)="personCancelled($event)" (saved)="personAdded($event)">
        </person-inline>
      </div>

      <a-mat-select [object]="invoice" [roleType]="m.SalesInvoice.BillToContactMechanism" [options]="contactMechanisms" display="displayName"></a-mat-select>
      <button *ngIf="invoice.BillToCustomer" type="button" mat-button (click)="addWebAddress = true">+Web</button>
      <button *ngIf="invoice.BillToCustomer" type="button" mat-button (click)="addEmailAddress = true">+Email</button>
      <button *ngIf="invoice.BillToCustomer" type="button" mat-button (click)="addPostalAddress = true">+Postal</button>
      <button *ngIf="invoice.BillToCustomer" type="button" mat-button (click)="addTeleCommunicationsNumber = true">+Telecom</button>

      <div *ngIf="addWebAddress && invoice.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-webaddress [scope]="scope" (cancelled)="webAddressCancelled($event)" (saved)="webAddressAdded($event)">
        </party-contactmechanism-webaddress>
      </div>
      <div *ngIf="addEmailAddress && invoice.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-emailAddress [scope]="scope" (cancelled)="emailAddressCancelled($event)" (saved)="emailAddressAdded($event)">
        </party-contactmechanism-emailAddress>
      </div>
      <div *ngIf="addPostalAddress && invoice.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-postaladdress [scope]="scope" (cancelled)="postalAddressCancelled($event)" (saved)="postalAddressAdded($event)">
        </party-contactmechanism-postaladdress>
      </div>
      <div *ngIf="addTeleCommunicationsNumber && invoice.BillToCustomer" style="background: lightblue" class="pad">
        <party-contactmechanism-telecommunicationsnumber [scope]="scope" (cancelled)="teleCommunicationsNumberCancelled($event)" (saved)="teleCommunicationsNumberAdded($event)">
        </party-contactmechanism-telecommunicationsnumber>
      </div>

      <a-mat-input [object]="invoice" [roleType]="m.SalesInvoice.Description"></a-mat-input>
      <a-mat-select [object]="invoice" [roleType]="m.SalesInvoice.VatRegime" [options]="vatRegimes" display="Name"></a-mat-select>
      <a-mat-static *ngIf="invoice.VatRegime" [object]="invoice.VatRegime" [roleType]="m.VatRegime.VatRate" display="Rate"></a-mat-static>

      <a-mat-static *ngIf="order?.Comment" [object]="order" [roleType]="m.SalesOrder.Comment"></a-mat-static>
      <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.Comment"></a-mat-textarea>
      <a-mat-static *ngIf="order?.InternalComment" [object]="order" [roleType]="m.SalesOrder.InternalComment"></a-mat-static>
      <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.InternalComment"></a-mat-textarea>
      <br/>
      <a-mat-textarea [object]="invoice" [roleType]="m.SalesInvoice.Message"></a-mat-textarea>
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
export class InvoiceComponent implements OnInit, AfterViewInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public invoice: SalesInvoice;
  public order: SalesOrder;
  public people: Person[];
  public organisations: Organisation[];
  public currencies: Currency[];
  public contactMechanisms: ContactMechanism[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public contacts: Person[];

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  public addEmailAddress: boolean = false;
  public addPostalAddress: boolean = false;
  public addTeleCommunicationsNumber: boolean = false;
  public addWebAddress: boolean = false;
  public addPerson: boolean = false;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  private previousBillToCustomer: Party;

  get showOrganisations(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Person);
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

    this.peopleFilter = new Filter({ scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
    this.organisationsFilter = new Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
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
          new Query(
            {
              name: "vatRates",
              objectType: m.VatRate,
            }),
          new Query(
            {
              name: "vatRegimes",
              objectType: m.VatRegime,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();
            this.currencies = loaded.collections.currencies as Currency[];
            this.vatRates = loaded.collections.vatRates as VatRate[];
            this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];

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
                  new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
                  new TreeNode({ roleType: m.SalesInvoice.BillToContactMechanism }),
                  new TreeNode({ roleType: m.SalesInvoice.SalesInvoiceState }),
                  new TreeNode({ roleType: m.SalesInvoice.SalesOrder }),
                ],
                name: "salesInvoice",
              }),
              new Fetch({
                id,
                name: "order",
                path: new Path({ step: m.SalesInvoice.SalesOrder }),
              }),
            ];

            return this.scope.load("Pull", new PullRequest({ fetch, query }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.order = loaded.objects.order as SalesOrder;

        if (!this.invoice) {
          this.invoice = this.scope.session.create("SalesInvoice") as SalesInvoice;
        }

        if (this.invoice.BillToCustomer) {
          this.receiverSelected(this.invoice.BillToCustomer);
        }

        this.previousBillToCustomer = this.invoice.BillToCustomer;
        this.organisations = loaded.collections.organisations as Organisation[];
        this.people = loaded.collections.parties as Person[];
        this.title = "Sales Invoice for: " + this.invoice.BillToCustomer.PartyName;
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
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
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
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public emailAddressCancelled(): void {
    this.addEmailAddress = false;
  }

  public emailAddressAdded(id: string): void {
    this.addEmailAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public postalAddressCancelled(): void {
    this.addPostalAddress = false;
  }

  public postalAddressAdded(id: string): void {
    this.addPostalAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public teleCommunicationsNumberCancelled(): void {
    this.addTeleCommunicationsNumber = false;
  }

  public teleCommunicationsNumberAdded(id: string): void {
    this.addTeleCommunicationsNumber = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.contactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public send(): void {
    const sendFn: () => void = () => {
      this.scope.invoke(this.invoice.Send)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully send.", "close", { duration: 5000 });
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
                sendFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            sendFn();
          }
        });
    } else {
      sendFn();
    }
  }

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.invoice.CancelInvoice)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully cancelled.", "close", { duration: 5000 });
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
                cancelFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            cancelFn();
          }
        });
    } else {
      cancelFn();
    }
  }

  public writeOff(): void {
    const writeOffFn: () => void = () => {
      this.scope.invoke(this.invoice.WriteOff)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully written off.", "close", { duration: 5000 });
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
                writeOffFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            writeOffFn();
          }
        });
    } else {
      writeOffFn();
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
        this.router.navigate(["/ar/invoice/" + this.invoice.id]);
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

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.ShipToAddress = null;
          this.invoice.ContactPerson = null;
          this.previousBillToCustomer =  this.invoice.BillToCustomer;
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
