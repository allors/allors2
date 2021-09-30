import { DerivationError } from './DerivationError';
import { ResponseType } from './ResponseType';

export interface Response {
  responseType: ResponseType;

  hasErrors: boolean;
  errorMessage?: string;
  versionErrors?: string[];
  accessErrors?: string[];
  missingErrors?: string[];
  derivationErrors?: DerivationError[];
}
