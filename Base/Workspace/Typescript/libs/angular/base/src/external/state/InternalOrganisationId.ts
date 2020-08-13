import { Injectable } from '@angular/core';
import { SessionState } from '@allors/angular/core';

@Injectable({
  providedIn: 'root',
})
export class InternalOrganisationId extends SessionState {
  constructor() {
    super('State$InternalOrganisationId');
  }
}
