import { Injectable } from '@angular/core';
import { SessionState } from './SessionState';

@Injectable({
  providedIn: 'root',
})
export class SingletonId extends SessionState {
  constructor() {
    super('State$SingletonId');
  }
}
