import { Component, EventEmitter, OnInit, Output, Self } from '@angular/core';

import { ErrorService, Saved, SessionService } from '../../../../../angular';
import { Enumeration, Locale, Person } from '../../../../../domain';
import { PullRequest, Sort, Equals } from '../../../../../framework';
import { MetaDomain } from '../../../../../meta';

@Component({
  // tslint:disable-next-line:component-selector
  selector: 'person-inline',
  templateUrl: './person-inline.component.html'
})
export class PersonInlineComponent implements OnInit {

  @Output()
  public saved: EventEmitter<string> = new EventEmitter<string>();

  @Output()
  public cancelled: EventEmitter<any> = new EventEmitter();

  public person: Person;

  public m: MetaDomain;

  public locales: Locale[];
  public genders: Enumeration[];
  public salutations: Enumeration[];

  constructor(
    private allors: SessionService,
    private errorService: ErrorService) {

    this.m = this.allors.m;
  }

  public ngOnInit(): void {

    const { pull, x } = this.allors;

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

    this.allors
      .load('Pull', new PullRequest({ pulls }))
      .subscribe((loaded) => {
        this.locales = loaded.collections.locales as Locale[];
        this.genders = loaded.collections.genders as Enumeration[];
        this.salutations = loaded.collections.salutations as Enumeration[];

        this.person = this.allors.session.create('Person') as Person;
      }, this.errorService.handler);
  }

  public cancel(): void {
    this.cancelled.emit();
  }

  public save(): void {

    this.allors
      .save()
      .subscribe((saved: Saved) => {
        this.saved.emit(this.person.id);
      },
        (error: Error) => {
          this.errorService.handle(error);
        });
  }

  // TODO:  change to latest inline implementation
}
