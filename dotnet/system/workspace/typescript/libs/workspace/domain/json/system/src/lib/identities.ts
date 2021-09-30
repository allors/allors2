export class Identities {
  private _counter: number;

  constructor(initial = 0) {
    this._counter = initial;
  }

  nextId() {
    return --this._counter;
  }
}
