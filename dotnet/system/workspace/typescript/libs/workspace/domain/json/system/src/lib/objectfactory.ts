import { MetaPopulation } from '@allors/workspace/meta/system';
import { IObject, IObjectFactory, IStrategy } from '@allors/workspace/domain/system';

export class ObjectFactory implements IObjectFactory{
  constructor(public metaPopulation: MetaPopulation) {}

  create(strategy: IStrategy): IObject {
    throw new Error('Method not implemented.');
  }
}
