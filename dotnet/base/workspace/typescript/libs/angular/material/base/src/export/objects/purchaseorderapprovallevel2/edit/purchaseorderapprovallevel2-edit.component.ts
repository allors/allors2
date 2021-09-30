import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest, Observable } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService, Invoked } from '@allors/angular/services/core';
import { PurchaseOrderApprovalLevel2 } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService, ObjectData } from '@allors/angular/material/services/core';
import { PrintService } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { TestScope, Action } from '@allors/angular/core';

@Component({
  templateUrl: './purchaseorderapprovallevel2-edit.component.html',
  providers: [ContextService],
})
export class PurchaseOrderApprovalLevel2EditComponent extends TestScope implements OnInit, OnDestroy {
  title: string;
  subTitle: string;

  readonly m: Meta;

  private subscription: Subscription;

  purchaseOrderApproval: PurchaseOrderApprovalLevel2;

  print: Action;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: ObjectData,
    public dialogRef: MatDialogRef<PurchaseOrderApprovalLevel2EditComponent>,
    public metaService: MetaService,
    public printService: PrintService,
    public refreshService: RefreshService,
    private saveService: SaveService
  ) {
    super();

    this.m = this.metaService.m;

    this.print = printService.print(this.m.PurchaseOrderApprovalLevel2.PurchaseOrder);
  }

  public ngOnInit(): void {
    const { pull, x } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$)
      .pipe(
        switchMap(() => {
          const pulls = [
            pull.PurchaseOrderApprovalLevel2({
              object: this.data.id,
              include: {
                PurchaseOrder: {
                  PrintDocument: x,
                },
              },
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls })).pipe(map((loaded) => loaded));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();
        this.purchaseOrderApproval = loaded.objects.PurchaseOrderApprovalLevel2 as PurchaseOrderApprovalLevel2;

        this.title = this.purchaseOrderApproval.Title;
      });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  approve(): void {
    this.saveAndInvoke(() => this.allors.context.invoke(this.purchaseOrderApproval.Approve));
  }

  reject(): void {
    this.saveAndInvoke(() => this.allors.context.invoke(this.purchaseOrderApproval.Reject));
  }

  saveAndInvoke(methodCall: () => Observable<Invoked>): void {
    const { pull } = this.metaService;

    this.allors.context
      .save()
      .pipe(
        switchMap(() => {
          return this.allors.context.load(pull.PurchaseOrderApprovalLevel2({ object: this.data.id }));
        }),
        switchMap(() => {
          this.allors.context.reset();
          return methodCall();
        })
      )
      .subscribe(() => {
        const data: IObject = {
          id: this.purchaseOrderApproval.id,
          objectType: this.purchaseOrderApproval.objectType,
        };

        this.dialogRef.close(data);
        this.refreshService.refresh();
      }, this.saveService.errorHandler);
  }
}
