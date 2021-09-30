import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs';

import { Like, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';

import { Organisation } from '@allors/domain/generated';
import { ContextService, MetaService, Loaded } from '@allors/angular/services/core';

@Component({
  templateUrl: './query.component.html',
  providers: [ContextService],
})
export class QueryComponent implements OnInit, OnDestroy {
  public organisations: Organisation[];

  public organisationCount: number;
  public skip = 5;
  public take = 5;

  private subscription: Subscription;

  constructor(@Self() private allors: ContextService, private metaService: MetaService, private title: Title) {}

  public ngOnInit() {
    this.title.setTitle('Query');
    this.query();
  }

  public query() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { m, pull, x } = this.metaService;

    const pulls = [
      pull.Organisation({
        predicate: new Like({
          roleType: m.Organisation.Name,
          value: 'Org%',
        }),
        include: { Owner: x },
        sort: new Sort(m.Organisation.Name),
        skip: this.skip || 0,
        take: this.take || 10,
      }),
    ];

    this.allors.context.reset();

    this.subscription = this.allors.context.load(new PullRequest({ pulls })).subscribe(
      (loaded: Loaded) => {
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.organisationCount = loaded.values.Organisations_count;
      },
      (error) => {
        alert(error);
      }
    );
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
