import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Title } from '@angular/platform-browser';
import { MdSnackBar } from '@angular/material';
import { TdLoadingService, TdDialogService, TdMediaService } from '@covalent/core';

import { Scope } from '../../allors/angular/base/Scope';
import { AllorsService } from '../allors.service';
import { Person } from '../../allors/domain';

@Component({
  templateUrl: './people.component.html',
})
export class PeopleComponent implements AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  people: Person[];
  filtered: Person[];

  constructor(private titleService: Title,
              private router: Router,
              private loadingService: TdLoadingService,
              private dialogService: TdDialogService,
              private snackBarService: MdSnackBar,
              public media: TdMediaService,
              allors: AllorsService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  goBack(route: string): void {
    this.router.navigate(['/']);
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
    this.titleService.setTitle( 'People' );
    this.subscription = this.refresh().subscribe();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  filter(displayName: string = ''): void {
    this.filtered = this.people.filter((person: Person) => {
      return person.FirstName && person.FirstName.toLowerCase().indexOf(displayName.toLowerCase()) > -1;
    });
  }

  delete(person: Person): void {
    this.dialogService
      .openConfirm({message: 'Are you sure you want to delete this person?'})
      .afterClosed().subscribe((confirm: boolean) => {
        if (confirm) {
          // TODO: Logical, physical or workflow delete
        }
      });
  }

  protected refresh(): Observable<any> {
    this.scope.session.reset();

    return this.scope
        .load('People', {})
        .do(() => {
            this.people = this.scope.collections.people as Person[];
            this.filter();
        });
  }
}
