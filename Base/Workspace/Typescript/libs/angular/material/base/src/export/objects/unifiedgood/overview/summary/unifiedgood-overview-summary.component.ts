import { Component, Self, OnInit, OnDestroy, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { Subscription, combineLatest } from 'rxjs';
import { switchMap, map } from 'rxjs/operators';
import { isBefore, isAfter } from 'date-fns';

import { TestScope, MetaService, NavigationService, PanelService, MediaService, ContextService, RefreshService, Action, ActionTarget, Invoked } from '@allors/angular/core';
import { Organisation, Person, OrganisationContactRelationship, OrganisationContactKind, SupplierOffering, Part, RatingType, Ordinal, UnitOfMeasure, Currency, Settings, SupplierRelationship, WorkTask, SalesInvoice, FixedAsset, Printable, UnifiedGood } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { ObjectData, SaveService } from '@allors/angular/material/core';
import { FiltersService, FetcherService, InternalOrganisationId } from '@allors/angular/base';
import { Sort, ContainedIn, Extent, Equals } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { IObject } from '@allors/domain/system';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'unifiedgood-overview-summary',
  templateUrl: './unifiedgood-overview-summary.component.html',
  providers: [PanelService]
})
export class UnifiedGoodOverviewSummaryComponent {

  m: Meta;

  good: UnifiedGood;
  suppliers: string;

  constructor(
    @Self() public panel: PanelService,
    public metaService: MetaService,
    public navigation: NavigationService) {

    this.m = this.metaService.m;

    panel.name = 'summary';

    const pullName = `${panel.name}_${this.m.UnifiedGood.name}`;

    panel.onPull = (pulls) => {
      const { pull, x } = this.metaService;

      const id = this.panel.manager.id;

      pulls.push(
        pull.UnifiedGood({
          name: pullName,
          object: id,
          include: {
            ProductType: x,
            Brand: x,
            Model: x,
            SuppliedBy: x,
            ManufacturedBy: x
          }
        })
      );
    };

    panel.onPulled = (loaded) => {
      this.good = loaded.objects[pullName] as UnifiedGood;

      if (this.good.SuppliedBy.length > 0) {
        this.suppliers = this.good.SuppliedBy
          .map(v => v.displayName)
          .reduce((acc: string, cur: string) => acc + ', ' + cur);
      }
    };
  }
}
