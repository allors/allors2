import { Component, OnDestroy, OnInit, Self, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { ContextService, MetaService, RefreshService } from '@allors/angular/services/core';
import { Enumeration, TelecommunicationsNumber } from '@allors/domain/generated';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { SaveService } from '@allors/angular/material/services/core';
import { InternalOrganisationId } from '@allors/angular/base';
import { IObject } from '@allors/domain/system';
import { Equals, Sort } from '@allors/data/system';
import { TestScope } from '@allors/angular/core';

@Component({
  templateUrl: './telecommunicationsnumber-edit.component.html',
  providers: [ContextService],
})
export class TelecommunicationsNumberEditComponent extends TestScope implements OnInit, OnDestroy {
  readonly m: Meta;

  contactMechanism: TelecommunicationsNumber;
  contactMechanismTypes: Enumeration[];
  title: string;

  private subscription: Subscription;

  constructor(
    @Self() public allors: ContextService,
    @Inject(MAT_DIALOG_DATA) public data: IObject,
    public dialogRef: MatDialogRef<TelecommunicationsNumberEditComponent>,
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
          const pulls = [
            pull.ContactMechanism({
              object: this.data.id,
            }),
            pull.ContactMechanismType({
              predicate: new Equals({ propertyType: m.ContactMechanismType.IsActive, value: true }),
              sort: new Sort(this.m.ContactMechanismType.Name),
            }),
          ];

          return this.allors.context.load(new PullRequest({ pulls }));
        })
      )
      .subscribe((loaded) => {
        this.allors.context.reset();

        this.contactMechanismTypes = loaded.collections.ContactMechanismTypes as Enumeration[];
        this.contactMechanism = loaded.objects.ContactMechanism as TelecommunicationsNumber;

        if (this.contactMechanism.CanWriteAreaCode) {
          this.title = 'Edit Phone Number';
        } else {
          this.title = 'View Phone Number';
        }
      });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe(() => {
      const data: IObject = {
        id: this.contactMechanism.id,
        objectType: this.contactMechanism.objectType,
      };

      this.dialogRef.close(data);
      this.refreshService.refresh();
    }, this.saveService.errorHandler);
  }
}
