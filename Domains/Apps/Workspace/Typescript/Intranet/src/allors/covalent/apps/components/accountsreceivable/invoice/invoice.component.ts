import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, SalesInvoice, SalesOrder, VatRate, VatRegime } from "../../../../../domain";
import { Contains, Equals, Fetch, Path, PullRequest, Query, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";
import { StateService } from "../../../services/StateService";
import { Fetcher } from "../../Fetcher";

@Component({
  templateUrl: "./invoice.component.html",
})
export class InvoiceComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public scope: Scope;
  public title: string;
  public subTitle: string;
  public invoice: SalesInvoice;
  public order: SalesOrder;
  public internalOrganisations: InternalOrganisation[];
  public currencies: Currency[];
  public billToContactMechanisms: ContactMechanism[];
  public billToEndCustomerContactMechanisms: ContactMechanism[];
  public shipToContactMechanisms: ContactMechanism[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public billToContacts: Person[];
  public shipToContacts: Person[];

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  public addBillToContactPerson: boolean = false;
  public addShipToContactPerson: boolean = false;
  public addBillToContactMechanism: boolean = false;
  public addBillToEndCustomerContactMechanism: boolean = false;
  public addShipToContactMechanism: boolean = false;
  public addShipToCustomerContactMechanism: boolean = false;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private previousBillToCustomer: Party;
  private previousShipToCustomer: Party;
  private fetcher: Fetcher;

  get showBillToOrganisations(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Organisation);
  }
  get showBillToPeople(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Person);
  }

  get showShipToOrganisations(): boolean {
    return !this.invoice.ShipToCustomer || this.invoice.ShipToCustomer instanceof (Organisation);
  }
  get showShipToPeople(): boolean {
    return !this.invoice.ShipToCustomer || this.invoice.ShipToCustomer instanceof (Person);
  }

  constructor(
    private workspaceService: WorkspaceService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: TdDialogService,
    public media: TdMediaService,
    private changeDetectorRef: ChangeDetectorRef,
    private stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.peopleFilter = new Filter({ scope: this.scope, objectType: this.m.Person, roleTypes: [this.m.Person.FirstName, this.m.Person.LastName]});
    this.organisationsFilter = new Filter({ scope: this.scope, objectType: this.m.Organisation, roleTypes: [this.m.Organisation.Name]});
    this.currenciesFilter = new Filter({scope: this.scope, objectType: this.m.Currency, roleTypes: [this.m.Currency.Name]});

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.m);
  }

  public ngOnInit(): void {

    this.subscription = Observable.combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .switchMap(([urlSegments, date, internalOrganisationId]) => {

        const id: string = this.route.snapshot.paramMap.get("id");
        const m: MetaDomain = this.m;

        const rolesQuery: Query[] = [
          new Query(m.Currency),
          new Query(m.VatRate),
          new Query(m.VatRegime),
          new Query(
            {
              name: "internalOrganisations",
              objectType: this.m.Organisation,
              predicate: new Equals({ roleType: m.Organisation.IsInternalOrganisation, value: true }),
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded) => {
            this.scope.session.reset();
            this.vatRates = loaded.collections.VatRateQuery as VatRate[];
            this.vatRegimes = loaded.collections.VatRegimeQuery as VatRegime[];
            this.currencies = loaded.collections.CurrencyQuery as Currency[];
            this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];

            const fetch: Fetch[] = [
              this.fetcher.internalOrganisation,
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.SalesInvoice.BillToCustomer }),
                  new TreeNode({ roleType: m.SalesInvoice.BillToContactMechanism }),
                  new TreeNode({ roleType: m.SalesInvoice.BillToEndCustomerContactMechanism }),
                  new TreeNode({ roleType: m.SalesInvoice.ShipToCustomer }),
                  new TreeNode({ roleType: m.SalesInvoice.ShipToAddress }),
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

            return this.scope.load("Pull", new PullRequest({ fetch }));
          });
      })
      .subscribe((loaded) => {
        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.order = loaded.objects.order as SalesOrder;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.invoice) {
          this.invoice = this.scope.session.create("SalesInvoice") as SalesInvoice;
          this.invoice.BilledFrom = internalOrganisation;
          this.title = "Add Sales Invoice";
        } else {
          this.title = "Sales Invoice for: " + this.invoice.BillToCustomer.PartyName;
        }

        if (this.invoice.BillToCustomer) {
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        if (this.invoice.BillToEndCustomer) {
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        if (this.invoice.ShipToCustomer) {
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        this.previousBillToCustomer = this.invoice.BillToCustomer;
        this.previousShipToCustomer = this.invoice.ShipToCustomer;
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public billToPersonCancelled(): void {
    this.addBillToContactPerson = false;
  }

  public billToPersonAdded(id: string): void {
    this.addBillToContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
  }

  public shipToPersonCancelled(): void {
    this.addShipToContactPerson = false;
  }

  public shipToPersonAdded(id: string): void {
    this.addShipToContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToContacts.push(contact);
  }

  public billToContactMechanismCancelled() {
    this.addBillToContactMechanism = false;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addBillToContactMechanism = false;

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public billToEndCustomerContactMechanismCancelled() {
    this.addBillToContactMechanism = false;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addBillToEndCustomerContactMechanism = false;

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public shipToCustomerContactMechanismCancelled() {
    this.addShipToContactMechanism = false;
  }

  public shipToCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addShipToContactMechanism = false;

    this.shipToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
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

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(["/accountsreceivable/invoice/" + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public billToCustomerSelected(party: Party) {
    if (party) {
      this.updateBillToCustomer(party);
    }
  }

  public billToEndCustomerSelected(party: Party) {
    if (party) {
      this.updateBillToEndCustomer(party);
    }
  }

  public shipToCustomerSelected(party: Party) {
    if (party) {
      this.updateShipToCustomer(party);
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  private updateBillToCustomer(party: Party) {
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
      .subscribe((loaded) => {

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.ShipToAddress = null;
          this.previousBillToCustomer =  this.invoice.BillToCustomer;
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);

        if (!(this.invoice.BillToCustomer instanceof Organisation && this.invoice.BillToCustomer.IsInternalOrganisation)) {
          this.billToContacts = loaded.collections.currentContacts as Person[];
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  private updateBillToEndCustomer(party: Party) {
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
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  private updateShipToCustomer(party: Party) {
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
      .subscribe((loaded) => {

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToContactMechanisms = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === "PostalAddress").map((v: PartyContactMechanism) => v.ContactMechanism);

        if (!(this.invoice.ShipToCustomer instanceof Organisation && this.invoice.ShipToCustomer.IsInternalOrganisation)) {
          this.shipToContacts = loaded.collections.currentContacts as Person[];
        }
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }
}
