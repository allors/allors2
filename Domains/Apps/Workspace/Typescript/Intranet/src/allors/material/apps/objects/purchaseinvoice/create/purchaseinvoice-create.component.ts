import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ErrorService, Invoked, Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { ContactMechanism, Currency, InternalOrganisation, Organisation, OrganisationContactRelationship, Party, PartyContactMechanism, Person, PostalAddress, PurchaseInvoice, PurchaseInvoiceType, PurchaseOrder, VatRate, VatRegime } from '../../../../../domain';
import { Equals, PullRequest, Sort } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { Fetcher } from '../../Fetcher';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { CreateData, EditData, ObjectData } from 'src/allors/material/base/services/object';
import { SalesTermEditComponent } from '../../salesterm/edit/salesterm-edit.module';

@Component({
  templateUrl: './purchaseinvoice-create.component.html',
  providers: [ContextService]
})
export class PurchaseInvoiceCreateComponent implements OnInit, OnDestroy {

  public m: Meta;

  public title = 'Edit Purchase Invoice';

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
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData,
    public dialogRef: MatDialogRef<SalesTermEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private errorService: ErrorService,
    private snackBar: MatSnackBar,
    private dialogService: AllorsMaterialDialogService,
    public stateService: StateService) {

    this.m = this.metaService.m;

    this.fetcher = new Fetcher(this.stateService, this.metaService.pull);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.stateService.internalOrganisationId$)
      .pipe(
        switchMap(([]) => {

          const create = (this.data as EditData).id === undefined;
          const { objectType, associationRoleType } = this.data;

          const pulls = [
            this.fetcher.internalOrganisation,
            pull.VatRate(),
            pull.VatRegime(),
            pull.Currency({
              sort: new Sort(m.Currency.Name),
            }),
            pull.PurchaseInvoiceType({
              predicate: new Equals({ propertyType: m.PurchaseInvoiceType.IsActive, value: true }),
              sort: new Sort(m.PurchaseInvoiceType.Name),
            })
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.vatRates = loaded.collections.VatRates as VatRate[];
        this.vatRegimes = loaded.collections.VatRegimes as VatRegime[];
        this.currencies = loaded.collections.Currencies as Currency[];
        this.purchaseInvoiceTypes = loaded.collections.PurchaseInvoiceTypes as PurchaseInvoiceType[];

        this.invoice = loaded.objects.PurchaseInvoice as PurchaseInvoice;
        this.order = loaded.objects.PurchaseOrder as PurchaseOrder;
        const internalOrganisation = loaded.objects.InternalOrganisation as Organisation;

        this.invoice = this.allors.context.create('PurchaseInvoice') as PurchaseInvoice;
        this.invoice.BilledTo = internalOrganisation;

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
      }, this.errorService.handler);
  }

  public billedFromContactPersonAdded(id: string): void {

    this.addBilledFromContactPerson = false;

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BilledFrom as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billedFromContacts.push(contact);
    this.invoice.BilledFromContactPerson = contact;
  }

  public billToContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.BillToCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.billToContacts.push(contact);
    this.invoice.BillToCustomerContactPerson = contact;
  }

  public shipToEndCustomerContactPersonAdded(id: string): void {

    const contact: Person = this.allors.context.get(id) as Person;

    const organisationContactRelationship = this.allors.context.create('OrganisationContactRelationship') as OrganisationContactRelationship;
    organisationContactRelationship.Organisation = this.invoice.ShipToEndCustomer as Organisation;
    organisationContactRelationship.Contact = contact;

    this.shipToEndCustomerContacts.push(contact);
    this.invoice.ShipToEndCustomerContactPerson = contact;
  }

  public billToContactMechanismAdded(partyContactMechanism: PartyContactMechanism): void {
    this.addBillToContactMechanism = false;

    this.billToContactMechanisms.push(partyContactMechanism.ContactMechanism);
    this.invoice.BillToCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.BillToCustomerContactMechanism = partyContactMechanism.ContactMechanism;
  }

  public shipToEndCustomerAddressAdded(partyContactMechanism: PartyContactMechanism): void {

    this.shipToEndCustomerAddresses.push(partyContactMechanism.ContactMechanism);
    this.invoice.ShipToEndCustomer.AddPartyContactMechanism(partyContactMechanism);
    this.invoice.ShipToEndCustomerAddress = partyContactMechanism.ContactMechanism as PostalAddress;
  }

  public cancel(): void {

    const cancelFn: () => void = () => {
      this.allors.context.invoke(this.invoice.CancelInvoice)
        .subscribe((invoked: Invoked) => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully cancelled.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.context.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors.context
              .save()
              .subscribe((saved: Saved) => {
                this.allors.context.reset();
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
      this.allors.context.invoke(this.invoice.Approve)
        .subscribe((invoked: Invoked) => {
          this.refreshService.refresh();
          this.snackBar.open('Successfully approved.', 'close', { duration: 5000 });
        },
          (error: Error) => {
            this.errorService.handle(error);
          });
    };

    if (this.allors.context.hasChanges) {
      this.dialogService
        .confirm({ message: 'Save changes?' })
        .subscribe((confirm: boolean) => {
          if (confirm) {
            this.allors.context
              .save()
              .subscribe((saved: Saved) => {
                this.allors.context.reset();
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
          this.allors.context.invoke(invoice.Finish)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully finished.', 'close', { duration: 5000 });
              this.refreshService.refresh();
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

    this.allors.context
      .save()
      .subscribe((saved: Saved) => {
        const data: ObjectData = {
          id: this.invoice.id,
          objectType: this.invoice.objectType,
        };

        this.dialogRef.close(data);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
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

    const { pull, x } = this.metaService;

    const pulls = [
      pull.Party({
        object: party.id,
        fetch: { CurrentContacts: x }
      })
    ];

    this.allors.context
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {

        if (this.invoice.BilledFrom !== this.previousBilledFrom) {
          this.invoice.BilledFromContactPerson = null;
          this.previousBilledFrom = this.invoice.BilledFrom;
        }

        this.billedFromContacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateBillToCustomer(party: Party) {

    const { pull, x } = this.metaService;

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

    this.allors.context
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

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.billToContactMechanisms = partyContactMechanisms.map((v: PartyContactMechanism) => v.ContactMechanism);
        this.billToContacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
  }

  private updateShipToEndCustomer(party: Party) {

    const { m, pull, x } = this.metaService;

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

    this.allors.context
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

        const partyContactMechanisms: PartyContactMechanism[] = loaded.collections.CurrentPartyContactMechanisms as PartyContactMechanism[];
        this.shipToEndCustomerAddresses = partyContactMechanisms.filter((v: PartyContactMechanism) => v.ContactMechanism.objectType.name === 'PostalAddress').map((v: PartyContactMechanism) => v.ContactMechanism);
        this.shipToEndCustomerContacts = loaded.collections.CurrentContacts as Person[];
      }, this.errorService.handler);
  }
}
