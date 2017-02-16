import { Population } from ".";

declare module "./core/Population" {
    interface Population {
        init();
    }
}

Population.prototype.init = function(this: Population) {
    this.coreInit();
};
