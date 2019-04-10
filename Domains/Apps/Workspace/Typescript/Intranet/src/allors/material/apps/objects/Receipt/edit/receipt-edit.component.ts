import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatSnackBar, MAT_DIALOG_DATA, MatDialogRef } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService } from '../../../../../angular';
import { Receipt, SalesInvoice, PaymentApplication } from '../../../../../domain';
import { Equals, PullRequest, Sort, IObject, Not } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { StateService } from '../../../services/state';
import { switchMap, map } from 'rxjs/operators';
import { CreateData } from '../../../../../material/base/services/object';
import { SaveService } from 'src/allors/material';

@Component({
  templateUrl: './receipt-edit.component.html',
  providers: [ContextService]
})
export class ReceiptEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  receipt: Receipt;
  salesInvoice: SalesInvoice;

  title: string;

  private subscription: Subscription;
  paymentApplication: PaymentApplication;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: CreateData & IObject,
    public dialogRef: MatDialogRef<ReceiptEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    public stateService: StateService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([]) => {

          const isCreate = (this.data as IObject).id === undefined;

          const pulls = [
            pull.Receipt({
              object: this.data.id,
              include: {
                PaymentApplications: x
              }
            }),
          ];

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.SalesInvoice({
                object: this.data.associationId,
              })
            );
          }

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => ({ loaded, isCreate }))
            );
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.salesInvoice = loaded.objects.SalesInvoice as SalesInvoice;

        if (isCreate) {
          this.title = 'Add Receipt';
          this.paymentApplication = this.allors.context.create('PaymentApplication') as PaymentApplication;
          this.paymentApplication.Invoice = this.salesInvoice;

          this.receipt = this.allors.context.create('Receipt') as Receipt;
          this.receipt.AddPaymentApplication(this.paymentApplication);

        } else {
          this.receipt = loaded.objects.Receipt as Receipt;
          this.paymentApplication = this.receipt.PaymentApplications[0];

          if (this.receipt.CanWriteAmount) {
            this.title = 'Edit Receipt';
          } else {
            this.title = 'View Receipt';
          }
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.paymentApplication.AmountApplied = this.receipt.Amount;

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.receipt.id,
          objectType: this.receipt.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
