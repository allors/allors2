import { AfterViewInit, ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
import { MatSnackBar } from '@angular/material';
import { ActivatedRoute, Router, UrlSegment } from '@angular/router';

import { BehaviorSubject, Observable, Subscription, combineLatest } from 'rxjs';

import { ErrorService, Invoked, Loaded, Saved, Scope, WorkspaceService, DataService, x } from '../../../../../angular';
import { Good, PurchaseInvoice, PurchaseInvoiceItem, PurchaseOrder, SalesInvoice } from '../../../../../domain';
import { Fetch, Path, PullRequest, Pull, TreeNode, Sort } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';
import { AllorsMaterialDialogService } from '../../../../base/services/dialog';
import { switchMap } from 'rxjs/operators';

@Component({
  templateUrl: './invoice-overview.component.html',
})
export class InvoiceOverviewComponent implements OnInit, OnDestroy {

  public m: MetaDomain;
  public title = 'Purchase Invoice Overview';
  public order: PurchaseOrder;
  public invoice: PurchaseInvoice;
  public goods: Good[] = [];

  private subscription: Subscription;
  private scope: Scope;
  private refresh$: BehaviorSubject<Date>;

  constructor(
    private workspaceService: WorkspaceService,
    private dataService: DataService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    private snackBar: MatSnackBar,
    private router: Router,
    private dialogService: AllorsMaterialDialogService) {

    this.refresh$ = new BehaviorSubject<Date>(undefined);

    this.scope = this.workspaceService.createScope();
    this.m = this.workspaceService.metaPopulation.metaDomain;
  }

  public refresh(): void {
    this.refresh$.next(new Date());
  }

  public save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.snackBar.open('items saved', 'close', { duration: 1000 });
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public ngOnInit(): void {

    const { m, pull } = this.dataService;

    this.subscription = combineLatest(this.route.url, this.refresh$)
      .pipe(
        switchMap(([urlSegments, date]) => {

          const id: string = this.route.snapshot.paramMap.get('id');

          const pulls = [
            pull.PurchaseInvoice({
              object: id,
              include: {
                PurchaseInvoiceItems: {
                  Product: x,
                  InvoiceItemType: x
                },
                BilledFrom: x,
                BilledFromContactPerson: x,
                BillToCustomer: x,
                BillToCustomerContactPerson: x,
                ShipToEndCustomer: x,
                ShipToEndCustomerContactPerson: x,
                PurchaseInvoiceState: x,
                CreatedBy: x,
                LastModifiedBy: x,
                PurchaseOrder: x,
                BillToCustomerContactMechanism: {
                    PostalAddress_Country : {
                    }
                },
                ShipToEndCustomerAddress : {
                  PostalBoundary: {
                    Country: x
                  }
                }
              },
            }),
            pull.PurchaseInvoice({
              object: id,
              fetch: {
                PurchaseOrder: x
              }
            }),
            pull.Good({
              sort: new Sort(m.Good.Name)
            })
          ];

          return this.scope
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.scope.session.reset();
        this.goods = loaded.collections.Goods as Good[];
        this.order = loaded.objects.Order as PurchaseOrder;
        this.invoice = loaded.objects.Invoice as PurchaseInvoice;
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        },
      );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public goBack(): void {
    window.history.back();
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

  public deleteInvoiceItem(invoiceItem: PurchaseInvoiceItem): void {
    this.dialogService
      .confirm({ message: 'Are you sure you want to delete this item?' })
      .subscribe((confirm: boolean) => {
        if (confirm) {
          this.scope.invoke(invoiceItem.Delete)
            .subscribe((invoked: Invoked) => {
              this.snackBar.open('Successfully deleted.', 'close', { duration: 5000 });
              this.refresh();
            },
              (error: Error) => {
                this.errorService.handle(error);
              });
        }
      });
  }

  public createInvoice(): void {
    this.scope.invoke(this.invoice.CreateSalesInvoice)
      .subscribe((invoked: Invoked) => {
        this.goBack();
        this.snackBar.open('Sales Invoice successfully created.', 'close', { duration: 5000 });
        this.gotoInvoice();
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  public gotoInvoice(): void {

    const { pull } = this.dataService;

    const pulls = [
      new Fetch({
      id: this.invoice.id,
      name: 'invoice',
      path: new Path({ step: this.m.PurchaseInvoice.SalesInvoiceWherePurchaseInvoice }),
    })];

    this.scope.load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        const invoice = loaded.objects.invoice as SalesInvoice;
        this.router.navigate(['/accountsreceivable/invoice/' + invoice.id]);
      },
        (error: any) => {
          this.errorService.handle(error);
          this.goBack();
        });
  }
}
