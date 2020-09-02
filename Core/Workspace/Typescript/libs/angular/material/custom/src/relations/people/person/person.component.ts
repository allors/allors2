import { AfterViewInit, Component, OnDestroy, OnInit, Self } from '@angular/core';
import { Title } from '@angular/platform-browser';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { switchMap } from 'rxjs/operators';

import { TestScope } from '@allors/angular/core';
import { Person } from '@allors/domain/generated';
import { Pull } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';
import { Meta } from '@allors/meta/generated';
import { ContextService, MetaService, Loaded } from '@allors/angular/services/core';

@Component({
  templateUrl: './person.component.html',
  providers: [ContextService],
})
export class PersonComponent extends TestScope implements OnInit, AfterViewInit, OnDestroy {
  public title: string;

  public m: Meta;
  public locales!: Locale[];
  public person!: Person;

  private subscription!: Subscription;

  constructor(
    @Self() private allors: ContextService,
    private metaService: MetaService,
    private titleService: Title,
    private route: ActivatedRoute,
  ) {
    super();

    this.title = 'Person';
    this.titleService.setTitle(this.title);

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {
    const { pull } = this.metaService;

    this.subscription = this.route.url
      .pipe(
        switchMap(() => {
          const x = {};
          const id = this.route.snapshot.paramMap.get('id');

          const pulls: Pull[] = [
            pull.Person({
              object: id ?? '',
              include: {
                Photo: x,
                Pictures: x,
              },
            }),
            pull.Locale(),
          ];
          this.allors.context.reset();
          return this.allors.context.load(new PullRequest({ pulls }));
        }),
      )
      .subscribe((loaded: Loaded) => {
        this.person = (loaded.objects.Person as Person) || (this.allors.context.create('Person') as Person);
        this.locales = loaded.collections.Locales as Locale[];
      });
  }

  public ngAfterViewInit(): void {}

  public ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  public save(): void {
    this.allors.context.save().subscribe(() => {
      this.goBack();
    });
  }

  public goBack(): void {
    window.history.back();
  }
}
