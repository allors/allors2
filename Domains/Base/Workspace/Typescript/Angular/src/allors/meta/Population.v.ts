import { Population } from './base/Population';
import { data } from './generated/meta.g';

declare module './base/Population' {
    interface Population {
        init();
    }
}

Population.prototype.init = function(this: Population) {
    this.baseInit(data);
};
