import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';
import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, OrganisationRole, Party, PartyContactMechanism, Person, PostalAddress, PurchaseInvoice, PurchaseInvoiceType, PurchaseOrder, VatRate, VatRegime } from '../../../../../domain';
import { Contains, Equals, Fetch, PullRequest, Pull, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain, PullFactory } from '../../../../../meta';
import { StateService } from '../../../services/StateService';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';

@Component({
  templateUrl: './invoice.component.html',
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
  public purchaseInvoiceTypes: PurchaseInvoiceType[];

  public billedFromContacts: Person[];
  public billToContactMechanisms: ContactMechanism[];
  public billToContacts: Person[];
  public shipToEndCustomerAddresses: ContactMechanism[];
  public shipToEndCustomerContacts: Person[];

  public addBilledFromContactPerson = false;
  public addBillToContactMechanism = false;
  public addBillToContactPerson = false;
  public addShipToEndCustomerAddress = false;
  public addShipToEndCustomerContactPerson = false;

  private previousBilledFrom: Party;
  private previousBillToCustomer: Party;
  private previousShipToEndCustomer: Party;

  private refresh$: BehaviorSubject<Date>;
  private subscription: Subscription;
  private fetcher: Fetcher;

  get showBillToOrganisations(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer.objectType.name === 'Organisation';
  }
  get showBillToPeople(): boolean {
    return !this.invoice.BillToCustomer || this.invoice.BillToCustomer.objectType.name === 'Person';
  }

  get showShipToEndCustomerOrganisations(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer.objectType.name === 'Organisation';
  }
  get showShipToEndCustomerPeople(): boolean {
    return !this.invoice.ShipToEndCustomer || this.invoice.ShipToEndCustomer.objectType.name === 'Person';
  }

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private router: Router,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;

    this.refresh$ = new BehaviorSubject<Date>(undefined);
    this.fetcher = new Fetcher(this.stateService, this.dataService.pull);
  }

  public ngOnInit(): void {

    this.subscription = combineLatest(this.route.url, this.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([urlSegments, date, internalOrganisationId]) => {

          const id: string = this.route.snapshot.paramMap.get('id');
          const m: MetaDomain = this.m;
          const pull = new PullFactory(this.workspaceService.metaPopulation);

          const pulls = [
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({
              sort: new Sort(m.Currency.Name),
            }),
            pull.PurchaseInvoiceType({
              predicate: new Equals(m.PurchaseInvoiceType.IsActive, true),
              sort: new Sort(m.PurchaseInvoiceType.Name),
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              switchMap((loaded) => {
                this.scope.session.reset();
                this.vatRates = loaded.collections.VatRates as VatRate[];
                this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
                this.currencies = loaded.collections.currencies as Currency[];
                this.purchaseInvoiceTypes = loaded.collections.purchaseInvoiceTypes as PurchaseInvoiceType[];

                const fetches = [
                  this.fetcher.internalOrganisation,
                  pull.PurchaseInvoice({
                    include: {
                      BilledFrom: x,
                      BilledFromContactPerson: x,
                      BillToCustomer: x,
                      BillToCustomerContactMechanism: x,
                      BillToCustomerContactPerson: x,
                      ShipToEndCustomer: x,
                      ShipToEndCustomerAddress: x,
                      ShipToEndCustomerContactPerson: x,
                      PurchaseInvoiceState: x,
                      PurchaseOrder: x,
                    }
                  }),
                  pull.PurchaseInvoice({
                    object: id,
                    fetch: {
                      PurchaseOrder: x,
                    }
                  })
                ];

                return this.scope.load('Pull', new PullRequest({ pulls: fetches }));
              })
            );
        })
      )
      .subscribe((loaded) => {
        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        const internalOrganisation = loaded.objects.InternalOrganisation as InternalOrganisation;

        if (!this.invoice) {
          this.invoice = this.scope.session.create('PurchaseInvoice') as PurchaseInvoice;
          this.invoice.BilledTo = internalOrganisation;
          this.title = 'Add Purchase Invoice';
        } else {
          this.title = 'Purchase Invoice from: ' + this.invoice.BilledFrom.PartyName;
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

        this.previousBilledFrom = this.invoice.BilledFrom;
        this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        this.previousBillToCustomer = this.invoice.BillToCustomer;
      },
        (error: Error) => {
          this.errorService.handle(error);
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

    const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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

    const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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

    const organisationContactRelationship = this.scope.session.create('OrganisationContactRelationship') as OrganisationContactRelationship;
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
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
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

  public approve(): void {
    const approveFn: () => void = () => {
      this.scope.invoke(this.invoice.Approve)
        .subscribe((invoked: Invoked) => {
          this.refresh();
          this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.scope.session.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.scope
              .save()
              .subscribe((saved: Saved) => {
                this.scope.session.reset();
                approveFn();
              },
                (error: Error) => {
                  this.errorService.handle(error);
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
      .confirm({ message: 'Are you sure you want to finish this invoice?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoice.Finish)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
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
        this.router.navigate(['/accountspayable/invoice/' + this.invoice.id]);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public goBack(): void {
    window.history.back();
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

  private updateBilledFrom(party: Party): void {

    const { pull } = this.dataService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: { CurrentContacts: x }
      })
    ];

    this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BilledFrom !== this.previousBilledFrom) {
          this.invoice.BilledFromContactPerson = null;
          this.previousBilledFrom = this.invoice.BilledFrom;
        }

        this.billedFromContacts = loaded.collections.currentContacts as Person[];
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  private updateBillToCustomer(party: Party) {

    const { pull } = this.dataService;

    const pulls = [
      pull.Party({
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: x
              }
            }
          }
        }
      }),
      pull.Party({
        object: party.id,
        fetch: {
          PartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x
                }
              }
            }
          }
        }
      }),
      pull.Party({
        object: party.id,
        fetch: { CurrentContacts: x }
      })
    ];

    this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BillToCustomer !== this.previousBillToCustomer) {
          this.invoice.BillToCustomerContactMechanism = null;
          this.invoice.BillToCustomerContactPerson = null;
          this.previousBillToCustomer = this.invoice.BillToCustomer;
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
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  private updateShipToEndCustomer(party: Party) {

    const { m, pull } = this.dataService;

    const pulls = [
      pull.Party({
        object: party,
        fetch: {
          CurrentPartyContactMechanisms: {
            include: {
              ContactMechanism: {
                PostalAddress_PostalBoundary: {
                  Country: x,
                }
              }
            }
          }
        }
      }),
      pull.Party({
        object: party,
        fetch: {
          CurrentContacts: x,
        }
      })
    ];

    this.scope
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.ShipToEndCustomer !== this.previousShipToEndCustomer) {
          this.invoice.ShipToEndCustomerAddress = null;
          this.invoice.ShipToEndCustomerContactPerson = null;
          this.previousShipToEndCustomer = this.invoice.ShipToEndCustomer;
        }

        if (this.invoice.ShipToEndCustomer !== null && this.invoice.BillToCustomer === null) {
          this.invoice.BillToCustomer = this.invoice.ShipToEndCustomer;
          this.updateBillToCustomer(this.invoice.BillToCustomer);
        }

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.partyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.currentContacts as Person[];
      },
        (error: Error) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }
}
