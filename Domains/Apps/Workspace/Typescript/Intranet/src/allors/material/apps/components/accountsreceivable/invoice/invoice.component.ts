import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Field, Invoked, Loaded, Saved, Scope, WorkspaceService, x, Allors } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, PostalAddress, SalesInvoice, SalesOrder, VatRate, VatRegime } from '../../../../../domain';
import { Equals, Fetch, PullRequest, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './invoice.component.html',
  providers: [Allors]
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
  public vatRates: VatRate[];
  public vatRegimes: VatRegime[];

  public billToContactMechanisms: ContactMechanism[];
  public billToContacts: Person[];
  public billToEndCustomerContactMechanisms: ContactMechanism[];
  public billToEndCustomerContacts: Person[];
  public shipToAddresses: ContactMechanism[];
  public shipToContacts: Person[];
  public shipToEndCustomerAddresses: ContactMechanism[];
  public shipToEndCustomerContacts: Person[];

  public addBillToContactMechanism = false;
  public addBillToContactPerson = false;
  public addBillToEndCustomerContactMechanism = false;
  public addBillToEndCustomerContactPerson = false;
  public addShipToAddress = false;
  public addShipToContactPerson = false;
  public addShipToEndCustomerAddress = false;
  public addShipToEndCustomerContactPerson = false;

  private previousShipToCustomer: Party;
  private previousShipToEndCustomer: Party;
  private previousBillToCustomer: Party;
  private previousBillToEndCustomer: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;

  get showBillToOrganisations(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer.objectType.name === 'Organisation';
  }
  get showBillToPeople(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer.objectType.name === 'Person';
  }

  get showShipToOrganisations(): boolean {
    return !this.invoice.ShipToCustomer || this.invoice.ShipToCustomer.objectType.name === 'Organisation';
  }
  get showShipToPeople(): boolean {
    return !this.invoice.ShipToCustomer || this.invoice.ShipToCustomer.objectType.name === 'Person';
  }

  get showBillToEndCustomerOrganisations(): boolean {
    return !this.invoice.BillToEndCustomer || this.invoice.BillToEndCustomer.objectType.name === 'Organisation';
  }
  get showBillToEndCustomerPeople(): boolean {
    return !this.invoice.BillToEndCustomer || this.invoice.BillToEndCustomer.objectType.name === 'Person';
  }

  get showShipToEndCustomerOrganisations(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer.objectType.name === 'Organisation';
  }
  get showShipToEndCustomerPeople(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer.objectType.name === 'Person';
  }

  constructor(
    @Self() private allors: Allors,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.m = this.allors.m;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.allors.pull);
  }

  public ngOnInit(): void {

    const { m, pull, scope } = this.allors;

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({
              sort: new Sort(m.Currency.Name),
            }),
            pull.Organisation({
              predicate: new Equals({ propertyType: m.Organisation.IsInternalOrganisation, value: true }),
              sort: [
                new Sort(m.Organisation.PartyName),
              ],
            })
          ];

          return scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                scope.session.reset();
                this.vatRates = loaded.collections.VatRates as VatRate[];
                this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
                this.currencies = loaded.collections.currencies as Currency[];
                this.internalOrganisations = loaded.collections.internalOrganisations as InternalOrganisation[];

                const fetches = [
                  this.fetcher.internalOrganisation,
                  pull.SalesInvoice({
                    object: id,
                    include: {
                      BillToCustomer: x,
                      BillToContactMechanism: x,
                      BillToContactPerson: x,
                      ShipToCustomer: x,
                      ShipToAddress: x,
                      ShipToContactPerson: x,
                      BillToEndCustomer: x,
                      BillToEndCustomerContactMechanism: x,
                      BillToEndCustomerContactPerson: x,
                      ShipToEndCustomer: x,
                      ShipToEndCustomerAddress: x,
                      ShipToEndCustomerContactPerson: x,
                      SalesInvoiceState: x,
                      SalesOrder: x,
                    },
                  }),
                  pull.SalesInvoice({
                    object: id,
                    fetch: {
                      SalesOrder: x
                    }
                  })
                ];

                return scope.load('Pull', new PullRequest({ pulls: fetches }));
              })
            );
        })

      )
      .subscribe((loaded) => {
        this.invoice = loaded.objects.salesInvoice as SalesInvoice;
        this.order = loaded.objects.order as SalesOrder;
        const internalOrganisation = loaded.objects.internalOrganisation as InternalOrganisation;

        if (!this.invoice) {
          this.invoice = scope.session.create('SalesInvoice') as SalesInvoice;
          this.invoice.BilledFrom = internalOrganisation;
          this.invoice.AdvancePayment = 0;
          this.title = 'Add Sales Invoice';
        } else {
          this.title = 'Sales Invoice for: ' + this.invoice.BillToCustomer.PartyName;
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

        if (this.invoice.ShipToEndCustomer) {
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        this.previousShipToCustomer = this.invoice.ShipToCustomer;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        this.previousBillToCustomer = this.invoice.BillToCustomer;
        this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
      }, this.errorService.handler);
  }

  public billToContactPersonCancelled(): void {
    this.addBillToContactPerson = false;
  }

  public billToContactPersonAdded(id: string): void {
    const { scope } = this.allors;

    this.addBillToContactPerson = false;

    const contact: Person = scope.session.get(id) as Person;

    const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
    this.invoice.BillToContactPerson = contact;
  }

  public billToEndCustomerContactPersonCancelled(): void {
    this.addBillToEndCustomerContactPerson = false;
  }

  public billToEndCustomerContactPersonAdded(id: string): void {
    const { scope } = this.allors;

    this.addBillToEndCustomerContactPerson = false;

    const contact: Person = scope.session.get(id) as Person;

    const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToEndCustomerContacts.push(contact);
    this.invoice.BillToEndCustomerContactPerson = contact;
  }

  public shipToContactPersonCancelled(): void {
    this.addShipToContactPerson = false;
  }

  public shipToContactPersonAdded(id: string): void {
    const { scope } = this.allors;

    this.addShipToContactPerson = false;

    const contact: Person = scope.session.get(id) as Person;

    const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToContacts.push(contact);
    this.invoice.ShipToContactPerson = contact;
  }

  public shipToEndCustomerContactPersonCancelled(): void {
    this.addShipToEndCustomerContactPerson = false;
  }

  public shipToEndCustomerContactPersonAdded(id: string): void {
    const { scope } = this.allors;

    this.addShipToEndCustomerContactPerson = false;

    const contact: Person = scope.session.get(id) as Person;

    const organisationContactRelationship = scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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
    this.invoice.BillToContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public billToEndCustomerContactMechanismCancelled() {
    this.addBillToEndCustomerContactMechanism = false;
  }

  public billToEndCustomerContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addBillToEndCustomerContactMechanism = false;

    this.billToEndCustomerContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.BillToEndCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToAddressCancelled() {
    this.addShipToAddress = false;
  }

  public shipToAddressAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addShipToAddress = false;

    this.shipToAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.ShipToAddress = partyContactMechanism.ContactMechanism as PostalAddress;
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

  public send(): void {
    const { scope } = this.allors;

    const sendFn: () => void = () => {
      scope.invoke(this.invoice.Send)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully send.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                sendFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
    const { scope } = this.allors;

    const cancelFn: () => void = () => {
      scope.invoke(this.invoice.CancelInvoice)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                cancelFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
    const { scope } = this.allors;

    const writeOffFn: () => void = () => {
      scope.invoke(this.invoice.WriteOff)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully written off.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                writeOffFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            writeOffFn();
          }
        });
    } else {
      writeOffFn();
    }
  }

  public reopen(): void {
    const { scope } = this.allors;

    const reopenFn: () => void = () => {
      scope.invoke(this.invoice.Reopen)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully reopened.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            scope
              .save()
              .subscribe((saved: Saved) => {
                scope.session.reset();
                reopenFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
                });
          } else {
            reopenFn();
          }
        });
    } else {
      reopenFn();
    }
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    const { scope } = this.allors;

    scope
      .save()
      .subscribe((saved: Saved) => {
        this.router.navigate(['/accountsreceivable/invoice/' + this.invoice.id]);
      },
        (error: Error) => {
          this.errorService.handle(error);
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

  private updateShipToCustomer(party: Party): void {
    const { pull, tree, scope } = this.allors;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          PartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism({
          ContactMechanism: {
            PostalAddress_PostalBoundary: {
              Country: x
            }
          }
        })
      }),
      pull.Party({
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToCustomer !== this.previousShipToCustomer) {
          this.invoice.ShipToAddress = null;
          this.invoice.ShipToContactPerson = null;
          this.previousShipToCustomer = this.invoice.ShipToCustomer;
        }

        if (this.invoice.ShipToCustomer !== null && this.invoice.BillToCustomer === null) {
          this.invoice.BillToCustomer = this.invoice.ShipToCustomer;
          this.updateBillToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateBillToCustomer(party: Party) {

    const { pull, tree, scope } = this.allors;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism(
          {
            ContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            }
          }
        )
      }),
      pull.Party({
        object: party.id,
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.BillToContactMechanism = null;
          this.invoice.BillToContactPerson = null;
          this.previousBillToCustomer = this.invoice.BillToCustomer;
        }

        if (this.invoice.BillToCustomer !== null && this.invoice.ShipToCustomer === null) {
          this.invoice.ShipToCustomer = this.invoice.BillToCustomer;
          this.updateShipToCustomer(this.invoice.ShipToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateBillToEndCustomer(party: Party) {

    const { pull, tree, scope } = this.allors;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: {
          CurrentPartyContactMechanisms: x,
        },
        include: tree.PartyContactMechanism(
          {
            ContactMechanism: {
              PostalAddress_PostalBoundary: {
                Country: x
              }
            }
          }
        )
      }),
      pull.Party({
        fetch: {
          CurrentContacts: x
        }
      })
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BillToEndCustomer !== this.previousBillToEndCustomer) {
          this.invoice.BillToEndCustomerContactMechanism = null;
          this.invoice.BillToEndCustomerContactPerson = null;
          this.previousBillToEndCustomer = this.invoice.BillToEndCustomer;
        }

        if (this.invoice.BillToEndCustomer !== null && this.invoice.ShipToEndCustomer === null) {
          this.invoice.ShipToEndCustomer = this.invoice.BillToEndCustomer;
          this.updateShipToEndCustomer(this.invoice.ShipToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.billToEndCustomerContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateShipToEndCustomer(party: Party) {

    const { pull, tree, scope } = this.allors;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: x
        },
        include: tree.PartyContactMechanism({
          ContactMechanism: {
            PostalAddress_PostalBoundary: {
              Country: x
            }
          }
        })
      }),
      pull.Party({
        object: party,
        fetch: { CurrentContacts: x }
      }),
    ];

    scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.invoice.ShipToEndCustomerAddress = null;
          this.invoice.ShipToEndCustomerContactPerson = null;
          this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        }

        if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToEndCustomer === null) {
          this.invoice.BillToEndCustomer = this.invoice.ShipToEndCustomer;
          this.updateBillToEndCustomer(this.invoice.BillToEndCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      }, this.errorService.handler);
  }
}
