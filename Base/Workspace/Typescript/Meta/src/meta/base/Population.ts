import * as data from "./Data";
import { Population } from "../core/Population";

declare module "../core/Population" {
    interface Population {
        baseInit(dataPopulation: data.Population);
    }
}

Population.prototype.baseInit = function(this: Population, dataPopulation: data.Population) {
};