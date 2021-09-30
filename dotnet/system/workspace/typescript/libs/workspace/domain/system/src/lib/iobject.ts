import { IStrategy } from "./IStrategy";

export interface IObject {
  id: number;

  strategy: IStrategy;
}
