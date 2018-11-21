import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { Subscription } from 'rxjs';

import { Loaded, SessionService } from '../../allors/angular';
import { Organisation } from '../../allors/domain';
import { Like, PullRequest, Sort } from '../../allors/framework';

@Component({
  templateUrl: './query.component.html',
  providers: [SessionService]
})
export class QueryComponent implements OnInit, OnDestroy {

  public organisations: Organisation[];

  public organisationCount: number;
  public skip = 5;
  public take = 5;

  private subscription: Subscription;

  constructor(
    @Self() private allors: SessionService,
    private title: Title,
  ) {
  }

  public ngOnInit() {
    this.title.setTitle('Query');
    this.query();
  }

  public query() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { m, pull, x } = this.allors;

    const pulls = [
      pull.Organisation({
        predicate: new Like(
          {
            roleType: m.Organisation.Name,
            value: 'Org%',
          }),
        include: { Owner: x },
        sort: new Sort(m.Organisation.Name),
        skip: this.skip || 0,
        take: this.take || 10,
      })
    ];


    this.allors.session.reset();

    this.subscription = this.allors
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded: Loaded) => {
        this.organisations = loaded.collections.Organisations as Organisation[];
        this.organisationCount = loaded.values.Organisations_count;
      },
        (error) => {
          alert(error);
        });
  }

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }
}
