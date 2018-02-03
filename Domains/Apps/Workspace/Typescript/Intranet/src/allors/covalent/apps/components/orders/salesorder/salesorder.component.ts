import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from "@angular/core";
import { MatSnackBar } from "@angular/material";
import { ActivatedRoute, Router, UrlSegment } from "@angular/router";
import { TdDialogService, TdMediaService } from "@covalent/core";

import { BehaviorSubject } from "rxjs/BehaviorSubject";
import { Observable } from "rxjs/Observable";
import { Subscription } from "rxjs/Subscription";

import "rxjs/add/observable/combineLatest";

import { ErrorService, Field, Filter, Invoked, Loaded, Saved, Scope, WorkspaceService } from "../../../../../angular";
import { ContactMechanism, Currency, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, ProductQuote, SalesOrder, Store, VatRate, VatRegime } from "../../../../../domain";
import { Contains, Fetch, Path, PullRequest, Query, Sort, TreeNode } from "../../../../../framework";
import { MetaDomain } from "../../../../../meta";

@Component({
  styles: [`
    .tabInlineComponent {
    border-bottom: 1px solid #CCCCCC;
    border-left: 1px solid #CCCCCC;
    border-right: 1px solid #CCCCCC;}
  `],
  templateUrl: "./salesorder.component.html",
})
export class SalesOrderEditComponent implements OnInit, OnDestroy {

  public m: MetaDomain;

  public title: string;
  public subTitle: string;
  public order: SalesOrder;
  public quote: ProductQuote;
  public people: Person[];
  public organisations: Organisation[];
  public currencies: Currency[];
  public billToContactMechanisms: ContactMechanism[];
  public ShipToAddresses: ContactMechanism[];
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];
  public contacts: Person[];
  public stores: Store[];

  public addShipToAddress: boolean = false;
  public addContactPerson: boolean = false;

  public peopleFilter: Filter;
  public organisationsFilter: Filter;
  public currenciesFilter: Filter;

  public addContactMechanism: boolean;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private scope: Scope;
  private previousShipToCustomer: Party;
  private previousBillToCustomer: Party;

  get showOrganisations(): boolean {
    return !this.order.ShipToCustomer || this.order.ShipToCustomer instanceof (Organisation);
  }
  get showPeople(): boolean {
    return !this.order.ShipToCustomer || this.order.ShipToCustomer instanceof (Person);
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
          new Query(
            {
            include: [
              new TreeNode({ roleType: m.Store.BillingProcess }),
            ],
            name: "stores",
              objectType: m.Store,
            }),
        ];

        return this.scope
          .load("Pull", new PullRequest({ query: rolesQuery }))
          .switchMap((loaded: Loaded) => {
            this.scope.session.reset();
            this.currencies = loaded.collections.currencies as Currency[];
            this.vatRates = loaded.collections.vatRates as VatRate[];
            this.vatRegimes = loaded.collections.vatRegimes as VatRegime[];
            this.stores = loaded.collections.stores as Store[];

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
                  name: "people",
                  objectType: this.m.Person,
                  predicate: new Contains({ roleType: m.Person.PersonRoles, object: pCustomerRole }),
                }),
            ];

            const fetch: Fetch[] = [
              new Fetch({
                id,
                include: [
                  new TreeNode({ roleType: m.SalesOrder.ShipToCustomer }),
                  new TreeNode({ roleType: m.SalesOrder.ShipToAddress }),
                  new TreeNode({ roleType: m.SalesOrder.SalesOrderState }),
                  new TreeNode({ roleType: m.SalesOrder.BillToContactMechanism }),
                  new TreeNode({ roleType: m.SalesOrder.Quote }),
                  new TreeNode({
                    nodes: [new TreeNode({ roleType: m.VatRegime.VatRate })],
                    roleType: m.SalesOrder.VatRegime,
                  }),
                ],
                name: "salesOrder",
              }),
            ];

            return this.scope.load("Pull", new PullRequest({ fetch, query }));
          });
      })
      .subscribe((loaded: Loaded) => {
        this.order = loaded.objects.salesOrder as SalesOrder;

        if (!this.order) {
          this.order = this.scope.session.create("SalesOrder") as SalesOrder;

          if (this.stores.length === 1) {
            this.order.Store =  this.stores[0];
          }

          this.title = "Add Sales Order";
        } else {
          this.title = "Sales Order " + this.order.OrderNumber;
        }

        if (this.order.ShipToCustomer) {
            this.updateShipToCustomer(this.order.ShipToCustomer);
        }

        if (this.order.BillToCustomer) {
          this.updateBillToCustomer(this.order.BillToCustomer);
    }

        this.previousShipToCustomer = this.order.ShipToCustomer;
        this.previousBillToCustomer = this.order.BillToCustomer;
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
    this.addContactPerson = false;
  }

  public personAdded(id: string): void {
    this.addContactPerson = false;

    const contact: Person = this.scope.session.get(id) as Person;

    const organisationContactRelationship = this.scope.session.create("OrganisationContactRelationship") as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.order.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.contacts.push(contact);
  }

  public partyContactMechanismCancelled() {
    this.addContactMechanism = false;
  }

  public partyContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addContactMechanism = false;

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.order.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public shipToAddressCancelled(): void {
    this.addShipToAddress = false;
  }

  public shipToAddressAdded(id: string): void {
    this.addShipToAddress = false;

    const partyContactMechanism: PartyContactMechanism = this.scope.session.get(id) as PartyContactMechanism;
    this.ShipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.order.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
  }

  public approve(): void {
    const submitFn: () => void = () => {
      this.scope.invoke(this.order.Approve)
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

  public cancel(): void {
    const cancelFn: () => void = () => {
      this.scope.invoke(this.order.Reject)
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

  public reject(): void {
    const rejectFn: () => void = () => {
      this.scope.invoke(this.order.Reject)
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

  public hold(): void {
    const holdFn: () => void = () => {
      this.scope.invoke(this.order.Hold)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully put on hold.", "close", { duration: 5000 });
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
                holdFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            holdFn();
          }
        });
    } else {
      holdFn();
    }
  }

  public continue(): void {
    const continueFn: () => void = () => {
      this.scope.invoke(this.order.Continue)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully removed from hold.", "close", { duration: 5000 });
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
                continueFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            continueFn();
          }
        });
    } else {
      continueFn();
    }
  }

  public confirm(): void {
    const confirmFn: () => void = () => {
      this.scope.invoke(this.order.Confirm)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully confirmed.", "close", { duration: 5000 });
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
                confirmFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            confirmFn();
          }
        });
    } else {
      confirmFn();
    }
  }

  public finish(): void {
    const finishFn: () => void = () => {
      this.scope.invoke(this.order.Continue)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open("Successfully finished.", "close", { duration: 5000 });
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
                finishFn();
              },
              (error: Error) => {
                this.errorService.dialog(error);
              });
          } else {
            finishFn();
          }
        });
    } else {
      finishFn();
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
        this.router.navigate(["/orders/salesOrder/" + this.order.id]);
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  public shipToCustomerSelected(party: Party) {
    if (party) {
      this.updateShipToCustomer(party);
    }
  }

  public billToCustomerSelected(party: Party) {
    if (party) {
      this.updateBillToCustomer(party);
    }
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
  }

  private updateShipToCustomer(party: Party): void {

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

          if (this.order.ShipToCustomer !== this.previousShipToCustomer) {
            this.order.ShipToAddress = null;
            this.order.ContactPerson = null;
            this.previousShipToCustomer =  this.order.ShipToCustomer;
          }

          if (this.order.ShipToCustomer !== null && this.order.BillToCustomer === null) {
            this.order.BillToCustomer = this.order.ShipToCustomer;
            this.updateBillToCustomer(this.order.BillToCustomer);
          }

          const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
          this.ShipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === "PostalAddress").map((v: PartyContactMechanism) => v.ContactMechanism);
          this.contacts = loaded.collections.currentContacts as Person[];
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
      ];

      this.scope
        .load("Pull", new PullRequest({ fetch }))
        .subscribe((loaded: Loaded) => {

          if (this.order.BillToCustomer !== this.previousBillToCustomer) {
            this.order.BillToContactMechanism = null;
            this.previousBillToCustomer =  this.order.BillToCustomer;
          }

          if (this.order.BillToCustomer !== null && this.order.ShipToCustomer === null) {
            this.order.ShipToCustomer = this.order.BillToCustomer;
            this.updateShipToCustomer(this.order.ShipToCustomer);
          }

          const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
          this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        },
        (error: Error) => {
          this.errorService.message(error);
          this.goBack();
        },
      );
    }
}
