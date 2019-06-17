import { PrintDocument } from '../../../../../domain/generated/PrintDocument.g';
import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { Subscription, combineLatest, Observable } from 'rxjs';

import { Saved, ContextService, MetaService, RefreshService, Invoked, Action, TestScope } from '../../../../../angular';
import { PurchaseOrderApprovalLevel1 } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { ObjectData } from '../../../../base/services/object';
import { PrintService } from '../../../services/actions/print/print.service';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { SaveService } from '../../../../../../allors/material';

@Component({
  templateUrl: './purchaseorderapprovallevel1-edit.component.html',
  providers: [ContextService]
})
export class PurchaseOrderApprovalLevel1EditComponent extends TestScope implements OnInit, OnDestroy {

  title: string;
  subTitle: string;

  readonly m: Meta;

  private subscription: Subscription;

  purchaseOrderApproval: PurchaseOrderApprovalLevel1;

  print: Action;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderApprovalLevel1EditComponent>,
    public metaService: MetaService,
    public printService: PrintService,
    public refreshService: RefreshService,
    private saveService: SaveService,
  ) {
    super();

    this.m = this.metaService.m;

    this.print = printService.print(this.m.PurchaseOrderApprovalLevel1.PurchaseOrder);
  }

  public ngOnInit(): void {

    const { m, pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(([, ]) => {

          const pulls = [
            pull.PurchaseOrderApprovalLevel1({
              object: this.data.id,
              include: {
                PurchaseOrder: {
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
        this.purchaseOrderApproval = loaded.objects.PurchaseOrderApprovalLevel1 as PurchaseOrderApprovalLevel1;

        this.title = this.purchaseOrderApproval.Title;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  approve(): void {
    this.saveAndInvoke(this.allors.context.invoke(this.purchaseOrderApproval.Approve));
  }

  reject(): void {
    this.saveAndInvoke(this.allors.context.invoke(this.purchaseOrderApproval.Reject));
  }

  saveAndInvoke(methodCall: Observable<Invoked> ): void {

    this.allors.context
      .save()
      .pipe(
        switchMap(() => methodCall)
      )
      .subscribe((invoked: Invoked) => {
        const data: IObject = {
          id: this.purchaseOrderApproval.id,
          objectType: this.purchaseOrderApproval.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }

}
