import { domain } from '../domain';
import { SerialisedInventoryItem } from '../generated/SerialisedInventoryItem.g';

declare module '../generated/SerialisedInventoryItem.g' {
  interface SerialisedInventoryItem {
    age;
    yearsToGo;
    goingConcern;
    marketValue;
    grossBookValue;
    expectedPosa;
  }
}

domain.extend((workspace) => {

  const obj: SerialisedInventoryItem = workspace.prototypeByName['SerialisedInventoryItem'];

  Object.defineProperty(obj, 'age', {
    get(this: SerialisedInventoryItem) {
      if (obj.ManufacturingYear) {
        return new Date().getFullYear() - this.ManufacturingYear;
      }
      else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'yearsToGo', {
    get(this: SerialisedInventoryItem) {
      if (obj.ManufacturingYear) {
        return this.LifeTime - this.age < 0 ? 0 : this.LifeTime - this.age;
      }
      else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'goingConcern', {
    get(this: SerialisedInventoryItem) {
      return Math.round((this.ReplacementValue * this.yearsToGo) / this.LifeTime);
    },
  });

  Object.defineProperty(obj, 'marketValue', {
    get(this: SerialisedInventoryItem) {
      if (obj.ManufacturingYear) {
        return Math.round(this.ReplacementValue * Math.exp(-2.045 * this.age / this.LifeTime));
      }
      else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'grossBookValue', {
    get(this: SerialisedInventoryItem) {
      return Math.round(this.PurchasePrice + this.RefurbishCost + this.TransportCost);
    },
  });

  Object.defineProperty(obj, 'expectedPosa', {
    get(this: SerialisedInventoryItem) {
      return this.ExpectedSalesPrice - this.grossBookValue;
    },
  });

});
