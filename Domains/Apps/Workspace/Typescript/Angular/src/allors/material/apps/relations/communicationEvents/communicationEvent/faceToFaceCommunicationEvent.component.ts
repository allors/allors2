import { Observable, Subject, Subscription } from 'rxjs/Rx';
import { Component, OnInit, AfterViewInit, OnDestroy } from '@angular/core';
import { Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { MdSnackBar, MdSnackBarConfig } from '@angular/material';
import { TdMediaService } from '@covalent/core';

import { MetaDomain } from '../../../../../meta/index';
import { PullRequest, PushResponse, Fetch, Path, Query, Equals, Like, TreeNode, Sort, Page } from '../../../../../domain';
import { Party, PartyRelationship, CommunicationEvent, CommunicationEventPurpose, FaceToFaceCommunication } from '../../../../../domain';
import { AllorsService, ErrorService, Scope, Loaded, Saved, Filter } from '../../../../../angular';

@Component({
  templateUrl: './faceToFaceCommunicationEvent.component.html',
})
export class FaceToFaceCommunicationEventFormComponent implements OnInit, AfterViewInit, OnDestroy {

  private subscription: Subscription;
  private scope: Scope;

  flex: string = '100%';
  flex2: string = `calc(50%-25px)`;

  m: MetaDomain;

  communicationEvent: CommunicationEvent;
  parties: Party[];
  purposes: CommunicationEventPurpose[];
  partyRelationships: PartyRelationship[];

  partiesFilter: Filter;

  constructor(
    private allors: AllorsService,
    private errorService: ErrorService,
    private route: ActivatedRoute,
    public media: TdMediaService) {

    this.scope = new Scope(allors.database, allors.workspace);
    this.m = this.allors.meta;

    this.partiesFilter = new Filter(this.scope, this.m.Party, [this.m.Party.PartyName]);
  }

  ngOnInit(): void {
    this.subscription = this.route.url
      .mergeMap((url: any) => {

        const partyId: string = this.route.snapshot.paramMap.get('partyId');
        const id: string = this.route.snapshot.paramMap.get('id');

        const m: MetaDomain = this.allors.meta;

        const fetch: Fetch[] = [
          new Fetch({
            name: 'partyRelationships',
            id: partyId,
            path: new Path({ step: m.Party.CurrentPartyRelationships }),
          }),
          new Fetch({
            name: 'communicationEvent',
            id: id,
            include: [
              new TreeNode({ roleType: m.CommunicationEvent.FromParties }),
              new TreeNode({ roleType: m.CommunicationEvent.ToParties }),
              new TreeNode({ roleType: m.CommunicationEvent.EventPurposes }),
              new TreeNode({ roleType: m.CommunicationEvent.CurrentObjectState }),
            ],
          }),
        ];

        const query: Query[] = [
          new Query(
            {
              name: 'purposes',
              objectType: this.m.CommunicationEventPurpose,
            }),
          new Query(
            {
              name: 'parties',
              objectType: this.m.Party,
            }),
        ];

        this.scope.session.reset();

        return this.scope
          .load('Pull', new PullRequest({ fetch: fetch, query: query }));
      })
      .subscribe((loaded: Loaded) => {

        this.partyRelationships = loaded.collections.partyRelationships as PartyRelationship[];
        this.communicationEvent = loaded.objects.communicationEvent as CommunicationEvent;

        if (!this.communicationEvent) {
          this.communicationEvent = this.scope.session.create('FaceToFaceCommunication') as FaceToFaceCommunication;
          this.partyRelationships.forEach((v: PartyRelationship) => v.AddCommunicationEvent(this.communicationEvent));
        }

        this.purposes = loaded.collections.purposes as CommunicationEventPurpose[];
        this.parties = loaded.collections.parties as Party[];
      },
      (error: any) => {
        this.errorService.message(error);
        this.goBack();
      },
    );
  }

  ngAfterViewInit(): void {
    this.media.broadcast();
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  save(): void {

    this.scope
      .save()
      .subscribe((saved: Saved) => {
        this.goBack();
      },
      (error: Error) => {
        this.errorService.dialog(error);
      });
  }

  goBack(): void {
    window.history.back();
  }
}
