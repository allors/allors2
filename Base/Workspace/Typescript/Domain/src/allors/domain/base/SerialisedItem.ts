import { domain } from '../domain';
import { SerialisedItem } from '../generated/SerialisedItem.g';

declare module '../generated/SerialisedItem.g' {
  interface SerialisedItem {
    displayName;
    age;
    yearsToGo;
    goingConcern;
    marketValue;
    grossBookValue;
    expectedPosa;
  }
}

domain.extend((workspace) => {

  const obj: SerialisedItem = workspace.prototypeByName['SerialisedItem'];

  Object.defineProperty(obj, 'displayName', {
    configurable: true,
    get(this: SerialisedItem) {

      return this.ItemNumber + ' ' + this.Name + ' SN: ' + this.SerialNumber;
    },
  });

  Object.defineProperty(obj, 'age', {
    configurable: true,
    get(this: SerialisedItem) {
      if (this.ManufacturingYear) {
        return new Date().getFullYear() - this.ManufacturingYear;
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'yearsToGo', {
    configurable: true,
    get(this: SerialisedItem) {
      if (this.ManufacturingYear) {
        return this.LifeTime - this.age < 0 ? 0 : this.LifeTime - this.age;
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'goingConcern', {
    configurable: true,
    get(this: SerialisedItem) {
      return Math.round((this.ReplacementValue * this.yearsToGo) / this.LifeTime);
    },
  });

  Object.defineProperty(obj, 'marketValue', {
    configurable: true,
    get(this: SerialisedItem) {
      if (this.ManufacturingYear) {
        return Math.round(this.ReplacementValue * Math.exp(-2.045 * this.age / this.LifeTime));
      } else {
        return 0;
      }
    },
  });

  Object.defineProperty(obj, 'grossBookValue', {
    configurable: true,
    get(this: SerialisedItem) {
      return Math.round(this.PurchasePrice + this.RefurbishCost + this.TransportCost);
    },
  });

  Object.defineProperty(obj, 'expectedPosa', {
    configurable: true,
    get(this: SerialisedItem) {
      return this.ExpectedSalesPrice - this.grossBookValue;
    },
  });

});
