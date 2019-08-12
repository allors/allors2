import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

import { Subscription, combineLatest } from 'rxjs';

import { ContextService, MetaService, RefreshService, InternalOrganisationId, TestScope } from '../../../../../angular';
import { Enumeration, ElectronicAddress } from '../../../../../domain';
import { PullRequest, Sort, Equals, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap, map } from 'rxjs/operators';
import { SaveService } from '../../../../../../allors/material';

@Component({
  templateUrl: './emailaddress-edit.component.html',
  providers: [ContextService]
})
export class EmailAddressEditComponent extends TestScope implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: ElectronicAddress;
  contactMechanismTypes: Enumeration[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: IObject,
    public dialogRef: MatDialogRef<EmailAddressEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId
  ) {

    super();

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { m, pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(() => {

          const isCreate = this.data.id === undefined;

          const pulls = [
            pull.ContactMechanism({
              object: this.data.id,
            }),
          ];

          return this.allors.context
            .load('Pull', new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {

        this.allors.context.reset();
        this.contactMechanism = loaded.objects.ContactMechanism as ElectronicAddress;

        if (this.contactMechanism.CanWriteElectronicAddressString) {
          this.title = 'Edit Email Address';
        } else {
          this.title = 'View Email Address';
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {

    this.allors.context.save()
      .subscribe(() => {
        const data: IObject = {
          id: this.contactMechanism.id,
          objectType: this.contactMechanism.objectType,
        };

        this.dialogRef.close(data);
      },
        this.saveService.errorHandler
      );
  }
}
