import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';

import { ContextService, MetaService } from '@allors/angular/services/core';
import { Person, Enumeration, Locale } from '@allors/domain/generated';
import { Meta } from '@allors/meta/generated';
import { Equals, Sort } from '@allors/data/system';
import { PullRequest } from '@allors/protocol/system';


@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-inline',
  templateUrl: './person-inline.component.html'
})
export class PersonInlineComponent implements OnInit, OnDestroy {

  @Output()
  public saved: EventEmitter<Person> = new EventEmitter<Person>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public person: Person;

  public m: Meta;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];

  constructor(
    private allors: ContextService,
    public metaService: MetaService) {

    this.m = this.metaService.m;
  }

  public ngOnInit(): void {

    const { pull } = this.metaService;

    const pulls = [
      pull.Locale({
        sort: new Sort(this.m.Locale.Name)
      }),
      pull.GenderType({
        predicate: new Equals({ propertyType: this.m.GenderType.IsActive, value: true }),
        sort: new Sort(this.m.GenderType.Name),
      }),
      pull.Salutation({
        predicate: new Equals({ propertyType: this.m.Salutation.IsActive, value: true }),
        sort: new Sort(this.m.Salutation.Name)
      })
    ];

    this.allors.context
      .load(new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.locales = loaded.collections.Locales as Locale[];
        this.genders = loaded.collections.GenderTypes as Enumeration[];
        this.salutations = loaded.collections.Salutations as Enumeration[];

        this.person = this.allors.context.create('Person') as Person;
      });
  }

  public ngOnDestroy(): void {
    if (!!this.person) {
      this.allors.context.delete(this.person);
    }
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {
      this.saved.emit(this.person);
      this.person = undefined;
  }
}
