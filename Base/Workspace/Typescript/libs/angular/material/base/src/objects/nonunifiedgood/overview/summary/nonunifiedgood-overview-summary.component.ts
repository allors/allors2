import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood, SalesOrder, RepeatingSalesInvoice, Good, WorkEffort, PurchaseOrder, PurchaseInvoice, Shipment, NonUnifiedGood } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';
import { PrintService } from '../../../../services/actions';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'nonunifiedgood-overview-summary',
  templateUrl: './nonunifiedgood-overview-summary.component.html',
  providers: [PanelService]
})
export class NonUnifiedGoodOverviewSummaryComponent extends TestScope {

  m: Meta;

  good: NonUnifiedGood;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService
  ) {
    super();

    this.m = this.metaService.m;

    panel.name = 'summary';

    const pullName = `${panel.name}_${this.m.NonUnifiedGood.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.NonUnifiedGood({
          name: pullName,
          object: id,
          include: {
            ProductIdentifications: {
              ProductIdentificationType: x
            },
            Part: {
              Brand: x,
              Model: x
            }
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.good = loaded.objects[pullName] as NonUnifiedGood;
    };
  }
}
