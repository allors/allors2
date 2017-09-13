import { SerialisedInventoryItem } from "../generated/SerialisedInventoryItem.g";

declare module "../generated/SerialisedInventoryItem.g" {
  interface SerialisedInventoryItem {
    age;
    yearsToGo;
    goingConcern;
    marketValue;
    grossBookValue;
    expectedPosa;
  }
}

Object.defineProperty(SerialisedInventoryItem.prototype, "age", {
  get(this: SerialisedInventoryItem) {
    return new Date().getFullYear() - this.ManufacturingYear;
  },
});

Object.defineProperty(SerialisedInventoryItem.prototype, "yearsToGo", {
  get(this: SerialisedInventoryItem) {
    return this.LifeTime - this.age < 0 ? 0 : this.LifeTime - this.age;
  },
});

Object.defineProperty(SerialisedInventoryItem.prototype, "goingConcern", {
  get(this: SerialisedInventoryItem) {
    return Math.round((this.ReplacementValue * this.yearsToGo) / this.LifeTime);
  },
});

Object.defineProperty(SerialisedInventoryItem.prototype, "marketValue", {
  get(this: SerialisedInventoryItem) {
    return Math.round(this.ReplacementValue * Math.exp(-2.045 * this.age / this.LifeTime));
  },
});

Object.defineProperty(SerialisedInventoryItem.prototype, "grossBookValue", {
  get(this: SerialisedInventoryItem) {
    return this.PurchasePrice + this.RefurbishCost + this.TransportCost;
  },
});

Object.defineProperty(SerialisedInventoryItem.prototype, "expectedPosa", {
  get(this: SerialisedInventoryItem) {
    return this.ExpectedSalesPrice - this.grossBookValue;
  },
});
