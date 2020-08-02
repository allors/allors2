export class AssertionError implements Error {
  name = 'AssertionError';
  constructor(public message: string) {}
}

export function assert(condition: any, msg?: string): asserts condition {
  if (!condition) {
    throw new AssertionError(msg ?? `Assertion failed: ${condition}`);
  }
}
