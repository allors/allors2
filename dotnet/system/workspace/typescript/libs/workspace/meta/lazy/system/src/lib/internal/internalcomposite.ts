import { Composite } from '@allors/workspace/meta/system';
import { Lookup } from '../utils/Lookup';
import { InternalAssociationType } from './InternalAssociationType';
import { InternalInterface } from './InternalInterface';
import { InternalObjectType } from './InternalObjectType';
import { InternalRoleType } from './InternalRoleType';

export interface InternalComposite extends InternalObjectType, Composite {
  derive(lookup: Lookup): void;
  deriveSuper(): void;
  deriveOperand(): void;
  supertypeGenerator(): IterableIterator<InternalInterface>;
  onNewAssociationType(associationType: InternalAssociationType): void;
  onNewRoleType(roleType: InternalRoleType): void;
}
