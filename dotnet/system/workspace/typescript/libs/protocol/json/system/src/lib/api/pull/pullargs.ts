import { UnitTypes } from '@allors/workspace/domain/system';

export interface PullArgs {
  /** Collections */
  c: { [name: string]: number[] };

  /** Objects */
  o: { [name: string]: number };

  /** Values */
  v: { [name: string]: UnitTypes };

  /** Pool */
  Pool: number[][];
}
