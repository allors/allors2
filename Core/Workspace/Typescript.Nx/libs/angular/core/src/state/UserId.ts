import { Injectable } from '@angular/core';
import { SessionState } from './SessionState';

@Injectable({
  providedIn: 'root',
})
export class UserId extends SessionState {
  constructor() {
    super('State$UserId');
  }
}
