import { Population } from ".";
import { data } from "./generated/base.g";

declare module "./base/Population" {
    interface Population {
        init();
    }
}

Population.prototype.init = function(this: Population) {
    this.baseInit(data);
};
