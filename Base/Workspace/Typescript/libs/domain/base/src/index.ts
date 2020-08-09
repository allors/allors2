declare module '@allors/domain/generated' {
  interface AutomatedAgent {
    displayName: string;
  }

  interface ContactMechanism {
    displayName: string;
  }

  interface EmailAddress {
    displayName: string;
  }

  interface FixedAsset {
    displayName: string;
  }

  interface InventoryItem {
    facilityName: string;
  }

  interface NonSerialisedInventoryItem {
    // facilityName: string;
    // quantityOnHand: number;
    // availableToPromise: number;
    // quantityCommittedOut: number;
  }

  interface Organisation {
    displayName: string;
    displayClassification: string;
    displayPhone: string;
    displayAddress: string;
    displayAddress2: string;
    displayAddress3: string;
  }

  interface PartCategory {
    displayName: string;
  }

  interface Party {
    displayName: string;
  }

  interface Person {
    displayName: string;
    displayEmail: string;
    displayPhone: string;
  }

  interface PostalAddress {
    displayName: string;
  }

  interface ProductCategory {
    displayName: string;
  }

  interface PurchaseOrder {
    displayName: string;
  }

  interface PurchaseOrderItem {
    displayName: string;
  }

  interface SerialisedInventoryItem {
    facilityName: string;
  }

  interface SerialisedItem {
    displayName: string;
    age: number;
    yearsToGo: number;
  }

  interface SerialisedItemCharacteristicType {
    displayName: string;
  }

  interface TelecommunicationsNumber {
    displayName: string;
  }

  interface UnifiedGood {
    categoryNames: string;
    motorized: boolean;
  }

  interface UnifiedProduct {
    categoryNames: string;
  }

  interface User {
    displayName: string;
  }

  interface WebAddress {
    displayName: string;
  }

  interface WorkEffortInventoryAssignment {
    totalSellingPrice: string;
  }

  interface WorkEffortPartyAssignment {
    displayName: string;
  }

  interface WorkEffortState {
    created: boolean;
    inProgress: boolean;
    cancelled: boolean;
    completed: boolean;
    finished: boolean;
  }
}

import { Workspace } from '@allors/domain/system';


import { extendAutomatedAgent } from './AutomatedAgent';
import { extendEmailAddress } from './EmailAddress';
import { extendNonSerialisedInventoryItem } from './NonSerialisedInventoryItem';
import { extendOrganisation } from './Organisation';
import { extendPartCategory } from './PartCategory';
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
import { extendWebAddress } from './WebAddress';
import { extendWorkEffortInventoryAssignment } from './WorkEffortInventoryAssignment';
import { extendWorkEffortPartyAssignment } from './WorkEffortPartyAssignment';
import { extendWorkEffortState } from './WorkEffortState';

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
