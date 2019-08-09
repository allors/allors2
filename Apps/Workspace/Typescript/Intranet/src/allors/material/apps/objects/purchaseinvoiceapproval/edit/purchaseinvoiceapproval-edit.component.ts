import { PrintDocument } from '../../../../../domain/generated/PrintDocument.g';
import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest, Observable } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, Invoked, Action, TestScope } from '../../../../../angular';
import { PurchaseInvoiceApproval } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { ObjectData } from '../../../../core/services/object';
import { PrintService } from '../../../services/actions/print/print.service';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SaveService } from '../../../../../../allors/material';

@Component({
  templateUrl: './purchaseinvoiceapproval-edit.component.html',
  providers: [ContextService]
})
export class PurchaseInvoiceApprovalEditComponent extends TestScope implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  private subscription: Subscription;

  purchaseInvoiceApproval: PurchaseInvoiceApproval;

  print: Action;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseInvoiceApprovalEditComponent>,
    public metaService: MetaService,
    public printService: PrintService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;

    this.print = printService.print(this.m.PurchaseInvoiceApproval.PurchaseInvoice);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {

          const pulls = [
            pull.PurchaseInvoiceApproval({
              object: this.data.id,
              include: {
                PurchaseInvoice: {
                  PrintDocument: x
                }
              }
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }))
            .pipe(
              map((loaded) => (loaded))
            );
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        this.purchaseInvoiceApproval = loaded.objects.PurchaseInvoiceApproval as PurchaseInvoiceApproval;

        this.title = this.purchaseInvoiceApproval.Title;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  approve(): void {
    this.saveAndInvoke(this.allors.context.invoke(this.purchaseInvoiceApproval.Approve));
  }

  reject(): void {
    this.saveAndInvoke(this.allors.context.invoke(this.purchaseInvoiceApproval.Reject));
  }

  saveAndInvoke(methodCall: Observable<Invoked>): void {

    this.allors.context
      .save()
      .pipe(
        switchMap(() => methodCall)
      )
      .subscribe((invoked: Invoked) => {
        const data: IObject = {
          id: this.purchaseInvoiceApproval.id,
          objectType: this.purchaseInvoiceApproval.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

}
