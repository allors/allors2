import { BehaviorSubject, Observable } from 'rxjs';

export abstract class SessionState {

  public observable$: Observable<string>;

  private subject: BehaviorSubject<string>;

  constructor(private key: string) {
    const initialValue = sessionStorage.getItem(key);
    this.subject = new BehaviorSubject(initialValue);
    this.observable$ = this.subject.asObservable();
  }

  get value(): string {
    return this.subject.getValue();
  }

  set value(value: string) {
    this.subject.next(value);
  }
}
