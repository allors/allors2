import { Interface } from '@allors/workspace/meta/system';
import { InternalAssociationType } from './InternalAssociationType';
import { InternalComposite } from './InternalComposite';
import { InternalMethodType } from './InternalMethodType';
import { InternalRoleType } from './InternalRoleType';

export interface InternalInterface extends InternalComposite, Interface {
  deriveSub(): void;

  associationTypeGenerator(): IterableIterator<InternalAssociationType>;

  roleTypeGenerator(): IterableIterator<InternalRoleType>;

  methodTypeGenerator(): IterableIterator<InternalMethodType>;
}
