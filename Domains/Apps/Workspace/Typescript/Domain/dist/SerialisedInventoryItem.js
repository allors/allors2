"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
const SerialisedInventoryItem_g_1 = require("@allors/generated/dist/domain/SerialisedInventoryItem.g");
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "age", {
    get() {
        return new Date().getFullYear() - this.ManufacturingYear;
    },
});
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "yearsToGo", {
    get() {
        return this.LifeTime - this.age < 0 ? 0 : this.LifeTime - this.age;
    },
});
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "goingConcern", {
    get() {
        return Math.round((this.ReplacementValue * this.yearsToGo) / this.LifeTime);
    },
});
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "marketValue", {
    get() {
        return Math.round(this.ReplacementValue * Math.exp(-2.045 * this.age / this.LifeTime));
    },
});
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "grossBookValue", {
    get() {
        return this.PurchasePrice + this.RefurbishCost + this.TransportCost;
    },
});
Object.defineProperty(SerialisedInventoryItem_g_1.SerialisedInventoryItem.prototype, "expectedPosa", {
    get() {
        return this.ExpectedSalesPrice - this.grossBookValue;
    },
});
