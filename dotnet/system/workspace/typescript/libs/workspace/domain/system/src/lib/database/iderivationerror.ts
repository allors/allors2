import { Role } from '../operands/Role';

export interface IDerivationError {
  message: string;

  roles: Role[];
}
