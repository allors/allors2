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
      return new Date().getFullYear() - this.ManufacturingYear;
    },
  });

  Object.defineProperty(obj, 'yearsToGo', {
    get(this: SerialisedInventoryItem) {
      return this.LifeTime - this.age < 0 ? 0 : this.LifeTime - this.age;
    },
  });

  Object.defineProperty(obj, 'goingConcern', {
    get(this: SerialisedInventoryItem) {
      return Math.round((this.ReplacementValue * this.yearsToGo) / this.LifeTime);
    },
  });

  Object.defineProperty(obj, 'marketValue', {
    get(this: SerialisedInventoryItem) {
      return Math.round(this.ReplacementValue * Math.exp(-2.045 * this.age / this.LifeTime));
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
