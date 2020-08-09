import { Workspace } from '@allors/domain/system';

import { extendAutomatedAgent } from './AutomatedAgent';
import './ContactMechanism';
import { extendEmailAddress } from './EmailAddress';
import './FixedAsset';
import './InventoryItem';
import { extendNonSerialisedInventoryItem } from './NonSerialisedInventoryItem';
import { extendOrganisation } from './Organisation';
import { extendPartCategory } from './PartCategory';
import './Party';
import { extendPerson } from './Person';
import { extendPostalAddress } from './PostalAddress';
import { extendProductCategory } from './ProductCategory';
import { extendPurchaseOrder } from './PurchaseOrder';
import { extendPurchaseOrderItem } from './PurchaseOrderItem';
import { extendSerialisedInventoryItem } from './SerialisedInventoryItem';
import { extendSerialisedItem } from './SerialisedItem';
import { extendSerialisedItemCharacteristicType } from './SerialisedItemCharacteristicType';
import { extendTelecommunicationsNumber } from './TelecommunicationsNumber';
import { extendUnifiedGood } from './UnifiedGood';
import './UnifiedProduct';
import './User';
import { extendWebAddress } from './WebAddress';
import { extendWorkEffortInventoryAssignment } from './WorkEffortInventoryAssignment';
import { extendWorkEffortPartyAssignment } from './WorkEffortPartyAssignment';
import { extendWorkEffortState } from './WorkEffortState';

import './WorkEffortState';

export function extend(workspace: Workspace) {
  extendAutomatedAgent(workspace);
  extendEmailAddress(workspace);
  extendNonSerialisedInventoryItem(workspace);
  extendOrganisation(workspace);
  extendPartCategory(workspace);
  extendPerson(workspace);
  extendPostalAddress(workspace);
  extendProductCategory(workspace);
  extendPurchaseOrder(workspace);
  extendPurchaseOrderItem(workspace);
  extendSerialisedInventoryItem(workspace);
  extendSerialisedItem(workspace);
  extendSerialisedItemCharacteristicType(workspace);
  extendTelecommunicationsNumber(workspace);
  extendUnifiedGood(workspace);
  extendWebAddress(workspace);
  extendWorkEffortInventoryAssignment(workspace);
  extendWorkEffortPartyAssignment(workspace);
  extendWorkEffortState(workspace);
}
