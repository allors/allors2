import { ResponseDerivationError } from './ResponseDerivationError';

export interface Response {
  /** error message */
  _e: string;

  /** version errors */
  _v: number[];

  /** access errors */
  _a: number[];

  /** missing errors */
  _m: number[];

  /** derivation errors */
  _d: ResponseDerivationError[];
}
