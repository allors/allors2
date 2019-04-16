import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';

import { Subscription, combineLatest } from 'rxjs';

import {  ContextService, MetaService, RefreshService, InternalOrganisationId } from '../../../../../angular';
import { Enumeration, ElectronicAddress } from '../../../../../domain';
import { PullRequest, IObject } from '../../../../../framework';
import { Meta } from '../../../../../meta';
import { switchMap } from 'rxjs/operators';
import { SaveService } from 'src/allors/material';

@Component({
  templateUrl: './webaddress-edit.component.html',
  providers: [ContextService]
})
export class WebAddressEditComponent implements OnInit, OnDestroy {

  readonly m: Meta;

  contactMechanism: ElectronicAddress;
  contactMechanismTypes: Enumeration[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: IObject,
    public dialogRef: MatDialogRef<WebAddressEditComponent>,
    public metaService: MetaService,
    public refreshService: RefreshService,
    private saveService: SaveService,
    private internalOrganisationId: InternalOrganisationId) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

    this.subscription = combineLatest(this.refreshService.refresh$, this.internalOrganisationId.observable$)
      .pipe(
        switchMap(([]) => {


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

        this.contactMechanismTypes = loaded.collections.ContactMechanismTypes as Enumeration[];

        this.contactMechanism = loaded.objects.ContactMechanism as ElectronicAddress;

        if (this.contactMechanism.CanWriteElectronicAddressString) {
          this.title = 'Edit Web Address';
        } else {
          this.title = 'View Web Address';
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
