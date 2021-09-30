import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { Disbursement, PurchaseInvoice, PaymentApplication, Invoice } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/services/core';
import { IObject } from '@allors/domain/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './disbursement-edit.component.html',
  providers: [ContextService],
})
export class DisbursementEditComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;

  disbursement: Disbursement;
  invoice: Invoice;

  title: string;

  private subscription: Subscription;
  paymentApplication: PaymentApplication;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<DisbursementEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService
  ) {
    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { pull, x } = this.metaService;

    this.subscription = combineLatest([this.refreshService.refresh$])
      .pipe(
        switchMap(() => {
          const isCreate = this.data.id === undefined;

          const pulls = [
          ];
          
          if (!isCreate) {
            pulls.push(
              pull.Disbursement({
                object: this.data.id,
                include: {
                  PaymentApplications: x,
                },
              }),
            );
          }

          if (isCreate && this.data.associationId) {
            pulls.push(
              pull.Invoice({
                object: this.data.associationId,
              })
            );
          }

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => ({ loaded, isCreate })));
        })
      )
      .subscribe(({ loaded, isCreate }) => {
        this.allors.context.reset();

        this.invoice = loaded.objects.Invoice as Invoice;

        if (isCreate) {
          this.title = 'Add Disbursement';
          this.paymentApplication = this.allors.context.create('PaymentApplication') as PaymentApplication;
          this.paymentApplication.Invoice = this.invoice;

          this.disbursement = this.allors.context.create('Disbursement') as Disbursement;
          this.disbursement.AddPaymentApplication(this.paymentApplication);
        } else {
          this.disbursement = loaded.objects.Disbursement as Disbursement;
          this.paymentApplication = this.disbursement.PaymentApplications[0];

          if (this.disbursement.CanWriteAmount) {
            this.title = 'Edit Disbursement';
          } else {
            this.title = 'View Disbursement';
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
    this.paymentApplication.AmountApplied = this.disbursement.Amount;

    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.disbursement.id,
        objectType: this.disbursement.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
