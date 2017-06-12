import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { Scope } from '../../../../allors/angular/base/Scope';
import { AllorsService } from '../../../allors.service';
import { CommunicationEvent, Organisation, Locale } from '../../../../allors/domain';

@Component({
  templateUrl: './organisation-overview.component.html',
})
export class OrganisationOverviewComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  private communicationEvents: CommunicationEvent[];

  private organisation: Organisation;
  id: string;

  constructor(private allors: AllorsService,
    private route: ActivatedRoute,
    public snackBar: MdSnackBar,
    public media: TdMediaService) {
    this.scope = new Scope(allors.database, allors.workspace);
  }

  ngOnInit(): void {

    this.subscription = this.route.url
      .mergeMap((url: any) => {

        this.id = this.route.snapshot.paramMap.get('id');
        this.scope.session.reset();

        return this.scope
          .load('Organisation', { id: this.id })
          .do(() => {
            this.organisation = this.scope.objects.organisation as Organisation;
            this.communicationEvents = this.scope.collections.communicationEvents as CommunicationEvent[];

          })
          .catch((e: any) => {
            this.snackBar.open(e.toString(), 'close', { duration: 5000 });
            this.goBack();
            return Observable.empty()
          });
      })
      .subscribe();
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy() {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  goBack(): void {
    window.history.back();
  }

  checkType(obj): string {
    return obj.objectType.name;
  };
}
