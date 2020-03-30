import { BehaviorSubject, Observable } from 'rxjs';

export abstract class SessionState {

  public observable$: Observable<string | null>;

  private subject: BehaviorSubject<string | null>;

  constructor(private key: string) {
    const initialValue = sessionStorage.getItem(this.key);
    this.subject = new BehaviorSubject(initialValue);
    this.observable$ = this.subject.asObservable();
  }

  get value(): string | null {
    return this.subject.getValue();
  }

  set value(value: string | null) {
    if (value == null) {
      sessionStorage.removeItem(this.key);
    } else {
      sessionStorage.setItem(this.key, value);
    }

    this.subject.next(value);
  }
}
