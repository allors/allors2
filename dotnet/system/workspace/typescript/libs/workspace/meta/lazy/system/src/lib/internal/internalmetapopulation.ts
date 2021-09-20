import { MetaPopulation } from '@allors/workspace/meta/system';
import { InternalMetaObject } from './InternalMetaObject';
import { InternalObjectType } from './InternalObjectType';
import { InternalComposite } from './InternalComposite';

export interface InternalMetaPopulation extends MetaPopulation {
  onNew(metaObject: InternalMetaObject): void;
  onNewObjectType(objectType: InternalObjectType): void;
  onNewComposite(objectType: InternalComposite): void;
}
