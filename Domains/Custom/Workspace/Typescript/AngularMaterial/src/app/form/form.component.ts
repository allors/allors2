import { Observable, Subject } from 'rxjs/Rx';
import { Component, OnInit } from '@angular/core';

import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';

import { Person } from '../../allors/domain';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
})
export class FormComponent implements OnInit {

  private scope: Scope;

  people: Person[];
  selected: Person;

 constructor(private allors: AllorsService) {
    this.scope = new Scope('People', allors.database, allors.workspace);
  }

  ngOnInit() {
    this.refresh().subscribe();
  }

  protected refresh(): Observable<any> {
    this.scope.session.reset();

    return this.scope
        .load({})
        .do(() => {
            this.people = this.scope.collections.people as Person[];
            this.people = this.people.filter(v => v.FirstName);
        })
        .catch((e) => {
          this.allors.onError(e);
          return Observable.empty();
        });
  }
}
