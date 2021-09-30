import { IObject } from '../IObject';
import { IDerivationError } from './IDerivationError';

export interface IResult {
  errorMessage: string;

  versionErrors: IObject[];

  accessErrors: IObject[];

  missingErrors: IObject[];

  derivationErrors: IDerivationError[];
}
