import { Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';

import { assert } from '@allors/meta/system';
import { PullRequest } from '@allors/protocol/system';
import { Organisation } from '@allors/domain/generated';
import { ContextService, MetaService, Loaded } from '@allors/angular/services/core';

@Component({
  templateUrl: './fetch.component.html',
  providers: [ContextService],
})
export class FetchComponent implements OnInit, OnDestroy {
  public organisation: Organisation;
  public organisations: Organisation[];

  private subscription: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private title: Title,
    private route: ActivatedRoute
  ) {}

  public ngOnInit() {
    this.title.setTitle('Fetch');
    this.fetch();
  }

  public fetch() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }

    const { pull, x } = this.metaService;

    const id = this.route.snapshot.paramMap.get('id');
    assert(id);

    const pulls = [
      pull.Organisation({ object: id, include: { Owner: x } }),
      pull.Organisation({
        object: id,
        fetch: {
          Owner: {
            OrganisationsWhereOwner: {
              include: {
                Owner: x,
              },
            },
          },
        },
      }),
    ];

    this.allors.context.reset();

    this.subscription = this.allors.context
      .load(
        new PullRequest({
          pulls,
        })
      )
      .subscribe(
        (loaded: Loaded) => {
          this.organisation = loaded.objects.Organisation as Organisation;
          this.organisations = loaded.collections.Organisations as Organisation[];
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
