import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, PostalAddress, PurchaseInvoice, PurchaseOrder, VatRate, VatRegime } from "../../../../../domain";
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
  public invoice: PurchaseInvoice;
  public order: PurchaseOrder;
  public currencies: Currency[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  public billedFromContacts: Person[];
  public billToContactMechanisms: ContactMechanism[];
  public billToContacts: Person[];
  public shipToEndCustomerAddresses: ContactMechanism[];
  public shipToEndCustomerContacts: Person[];

  public addBilledFromContactPerson: boolean = false;
  public addBillToContactMechanism: boolean = false;
  public addBillToContactPerson: boolean = false;
  public addShipToEndCustomerAddress: boolean = false;
  public addShipToEndCustomerContactPerson: boolean = false;

  private previousBilledFrom: Party;
  private previousBillToCustomer: Party;
  private previousShipToEndCustomer: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;

  get showBillToOrganisations(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Organisation);
  }
  get showBillToPeople(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer instanceof (Person);
  }

  get showShipToEndCustomerOrganisations(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer instanceof (Organisation);
  }
  get showShipToEndCustomerPeople(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer instanceof (Person);
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
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded) => {
            this.scope.session.reset();
            this.vatRates = loaded.collections.VatRateQuery as VatRate[];
            this.vatRegimes = loaded.collections.VatRegimeQuery as VatRegime[];
            this.currencies = loaded.collections.CurrencyQuery as Currency[];

            const fetch: Fetch[] = [
              this.fetcher.internalOrganisation,
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.PurchaseInvoice.BilledFrom }),
                  new TreeNode({ roleType: m.PurchaseInvoice.BilledFromContactPerson }),
                  new TreeNode({ roleType: m.PurchaseInvoice.BillToCustomer }),
                  new TreeNode({ roleType: m.PurchaseInvoice.BillToCustomerContactMechanism }),
                  new TreeNode({ roleType: m.PurchaseInvoice.BillToCustomerContactPerson }),
                  new TreeNode({ roleType: m.PurchaseInvoice.ShipToEndCustomer }),
                  new TreeNode({ roleType: m.PurchaseInvoice.ShipToEndCustomerAddress }),
                  new TreeNode({ roleType: m.PurchaseInvoice.ShipToEndCustomerContactPerson }),
                  new TreeNode({ roleType: m.PurchaseInvoice.PurchaseInvoiceState }),
                  new TreeNode({ roleType: m.PurchaseInvoice.PurchaseOrder }),
                ],
                name: "PurchaseInvoice",
              }),
              new Fetch({
                id,
                name: "order",
                path: new Path({ step: m.PurchaseInvoice.PurchaseOrder }),
              }),
            ];

            return this.scope.load("Pull", new PullRequest({ fetch }));
          });
      })
      .subscribe((loaded) => {
        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
        this.order = loaded.objects.order as PurchaseOrder;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.invoice) {
          this.invoice = this.scope.session.create("PurchaseInvoice") as PurchaseInvoice;
          this.invoice.BilledFrom = internalOrganisation;
        }

        if (this.invoice.BilledFrom) {
          this.updateBilledFrom(this.invoice.BilledFrom);
        }

        if (this.invoice.BillToCustomer) {
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        if (this.invoice.ShipToEndCustomer) {
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        this.title = "Purchase Invoice from " + this.invoice.BilledFrom.PartyName;

        this.previousBilledFrom = this.invoice.BilledFrom;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        this.previousBillToCustomer = this.invoice.BillToCustomer;
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  public billedFromContactPersonCancelled(): void {
    this.addBilledFromContactPerson = false;
  }

  public billedFromContactPersonAdded(id: string): void {
    this.addBilledFromContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billedFromContacts.push(contact);
    this.invoice.BilledFromContactPerson = contact;
  }

  public billToContactPersonCancelled(): void {
    this.addBillToContactPerson = false;
  }

  public billToContactPersonAdded(id: string): void {
    this.addBillToContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
    this.invoice.BillToCustomerContactPerson = contact;
  }

  public shipToEndCustomerContactPersonCancelled(): void {
    this.addShipToEndCustomerContactPerson = false;
  }

  public shipToEndCustomerContactPersonAdded(id: string): void {
    this.addShipToEndCustomerContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToEndCustomerContacts.push(contact);
    this.invoice.ShipToEndCustomerContactPerson = contact;
  }

  public billToContactMechanismCancelled() {
    this.addBillToContactMechanism = false;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addBillToContactMechanism = false;

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.BillToCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToEndCustomerAddressCancelled() {
    this.addShipToEndCustomerAddress = false;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addShipToEndCustomerAddress = false;

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.ShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
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

  public approve(): void {
    const approveFn: () => void = () => {
      this.scope.invoke(this.invoice.Approve)
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
                approveFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            approveFn();
          }
        });
    } else {
      approveFn();
    }
  }

  public finish(invoice: PurchaseInvoice): void {
    this.dialogService
      .openConfirm({ message: "Are you sure you want to finish this invoice?" })
      .afterClosed()
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoice.Finish)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open("Successfully finished.", "close", { duration: 5000 });
              this.refresh();
            },
            (error: Error) => {
              this.errorService.dialog(error);
            });
        }
      });
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
        this.router.navigate(["/accountspayable/invoice/" + this.invoice.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public billedFromSelected(party: Party) {
    if (party) {
      this.updateBilledFrom(party);
    }
  }

  public billToCustomerSelected(party: Party) {
    if (party) {
      this.updateBillToCustomer(party);
    }
  }

  public shipToEndCustomerSelected(party: Party) {
    if (party) {
      this.updateShipToEndCustomer(party);
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }
  private updateBilledFrom(party: Party): void {

    const fetch: Fetch[] = [
      new Fetch({
        id: party.id,
        name: "currentContacts",
        path: new Path({ step: this.m.Party.CurrentContacts }),
      }),
    ];

    this.scope
      .load("Pull", new PullRequest({ fetch }))
      .subscribe((loaded) => {

        if (this.invoice.BilledFrom !== this.previousBilledFrom) {
          this.invoice.BilledFromContactPerson = null;
          this.previousBilledFrom =  this.invoice.BilledFrom;
        }

        this.billedFromContacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
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
          this.invoice.BillToCustomerContactMechanism = null;
          this.invoice.BillToCustomerContactPerson = null;
          this.previousBillToCustomer =  this.invoice.BillToCustomer;
        }

        if (this.invoice.BillToCustomer !== null && this.invoice.ShipToEndCustomer === null) {
          this.invoice.ShipToEndCustomer = this.invoice.BillToCustomer;
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  private updateShipToEndCustomer(party: Party) {

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

        if (this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.invoice.ShipToEndCustomerAddress = null;
          this.invoice.ShipToEndCustomerContactPerson = null;
          this.previousShipToEndCustomer =  this.invoice.ShipToEndCustomer;
        }

        if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToCustomer === null) {
          this.invoice.BillToCustomer = this.invoice.ShipToEndCustomer;
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === "PostalAddress").map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      },
      (error: Error) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }
}
