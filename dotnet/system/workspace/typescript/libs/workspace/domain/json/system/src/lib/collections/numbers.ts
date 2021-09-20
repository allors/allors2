export type Numbers = Array<number> | undefined;

function _has(set: Array<number>, value: number): boolean {
  let j = 0;
  let length = set.length;
  while (j < length) {
    const i = (length + j - 1) >> 1;
    if (value > set[i]) {
      j = i + 1;
    } else if (value < set[i]) {
      length = i;
    } else {
      return true;
    }
  }

  return false;
}

function* _add(set: Array<number>, value: number) {
  let inserted = false;

  for (const current of set) {
    if (!inserted && value < current) {
      inserted = true;
      yield value;
    }
    yield current;
  }

  if (!inserted) {
    yield value;
  }
}

function* _remove(set: Array<number>, value: number) {
  for (const current of set) {
    if (value !== current) {
      yield value;
    }
  }
}

function assert(value: unknown): asserts value {
  if (value === undefined) {
    throw new Error('value must be defined');
  }
}

export function has(set: Numbers, value: number): boolean {
  assert(value);

  if (Number.isInteger(value)) {
    if (Array.isArray(set)) {
      return _has(set, value);
    }
  }

  return false;
}

export function Numbers(set?: number[]): Numbers {
  if (Array.isArray(set)) {
    return [...set].sort();
  }

  return undefined;
}

export function add(set: Numbers, value: number): Exclude<Numbers, undefined> {
  if (!Array.isArray(set)) {
    return [value];
  }

  if (!_has(set, value)) {
    return Array.from(_add(set, value));
  }

  return set;
}

export function remove(set: Numbers, value: number): Numbers {
  if (Array.isArray(set) && _has(set, value)) {
    if (set.length === 1) {
      return undefined;
    }

    return Array.from(_remove(set, value));
  }

  return set;
}

export function difference(self: Numbers, other: Numbers): Numbers {
  if (self === undefined || other === undefined) {
    return self;
  }

  const diff = self.filter((v) => !_has(other, v));
  return diff.length > 0 ? diff : undefined;
}

export function equals(self: Numbers, other: Numbers): boolean {
  if (self === undefined) {
    return other === undefined;
  }

  if (other === undefined) {
    return false;
  }

  if(self.length != other.length){
    return false;
  }

  for(let i=0; i<self.length; i++){
    if(self[i] !== other[i]){
      return false;
    }
  }

  return true;
}

export function properSubset(self: Numbers, other: Numbers): boolean {
  if (other === undefined) {
    return false;
  }

  if (self === undefined) {
    return true;
  }

  if (self.length >= other.length) {
    return false;
  }

  for (const element of self) {
    if (!_has(other, element)) {
      return false;
    }
  }

  return true;
}
