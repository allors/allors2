import { IObject } from "./IObject";
import { IStrategy } from "./IStrategy";

export interface IObjectFactory {
  create(strategy: IStrategy): IObject;
}
