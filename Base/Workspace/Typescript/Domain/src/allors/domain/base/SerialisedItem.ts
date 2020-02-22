import { domain } from '../domain';
import { SerialisedItem } from '../generated/SerialisedItem.g';
import { Meta } from '../../meta/generated/domain.g';
import { UnifiedGood } from '..';
import { assert } from '../../framework';

declare module '../generated/SerialisedItem.g' {
  interface SerialisedItem {
    displayName: string;
    age: number;
    yearsToGo: number;
    goingConcern: number;
    marketValue: number;
    grossBookValue: number;
    expectedPosa: number;
  }
}

domain.extend((workspace) => {

  const m = workspace.metaPopulation as Meta;
  const cls = workspace.constructorByObjectType.get(m.SerialisedItem);
  assert(cls);

  Object.defineProperty(cls.prototype, 'displayName', {
    configurable: true,
    get(this: SerialisedItem): string {

      return this.ItemNumber + ' ' + this.Name + ' SN: ' + this.SerialNumber;
    },
  });

  Object.defineProperty(cls.prototype, 'age', {
    configurable: true,
    get(this: SerialisedItem): number {
      if (this.CanReadPurchasePrice && this.ManufacturingYear != null) {
        return new Date().getFullYear() - this.ManufacturingYear;
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'yearsToGo', {
    configurable: true,
    get(this: SerialisedItem): number {
      const good = this.PartWhereSerialisedItem as UnifiedGood | null;

      if (this.CanReadPurchasePrice && this.ManufacturingYear != null && good?.LifeTime != null) {
        return good.LifeTime - this.age < 0 ? 0 : good.LifeTime - this.age;
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'goingConcern', {
    configurable: true,
    get(this: SerialisedItem): number {
      const good = this.PartWhereSerialisedItem as UnifiedGood | null;

      if (this.CanReadPurchasePrice && good?.ReplacementValue != null && good.LifeTime != null) {
        return Math.round((parseFloat(good.ReplacementValue) * this.yearsToGo) / good.LifeTime);
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'marketValue', {
    configurable: true,
    get(this: SerialisedItem): number {
      const good = this.PartWhereSerialisedItem as UnifiedGood | null;

      if (this.CanReadPurchasePrice && this.ManufacturingYear != null && good?.ReplacementValue != null && good.LifeTime != null) {
        return Math.round(parseFloat(good.ReplacementValue) * Math.exp(-2.045 * this.age / good.LifeTime));
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'grossBookValue', {
    configurable: true,
    get(this: SerialisedItem): number {
      const transportCost = this.EstimatedTransportCost;
      const refurbishCost = this.EstimatedRefurbishCost;
      if (this.CanReadPurchasePrice && this.PurchasePrice != null) {
        return Math.round(parseFloat(this.PurchasePrice) + parseFloat(refurbishCost) + parseFloat(transportCost));
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(cls.prototype, 'expectedPosa', {
    configurable: true,
    get(this: SerialisedItem): number {
      if (this.CanReadPurchasePrice && this.ExpectedSalesPrice != null) {
        return parseFloat(this.ExpectedSalesPrice) - this.grossBookValue;
      } else {
        return 0;
      }
    },
  });

});
