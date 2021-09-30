import { MetaData, Multiplicity, Origin } from '@allors/workspace/meta/system';

export class Lookup {
  o: Map<number, Origin>;
  m: Map<number, Multiplicity>;
  d: Set<number>;
  r: Set<number>;
  u: Set<number>;
  t: Map<number, string>;

  constructor(data: MetaData) {
    this.m = new Map();
    data.m?.forEach((v, i) => {
      const multiplicity = i == 0 ? Multiplicity.OneToOne : i == 1 ? Multiplicity.OneToMany : Multiplicity.ManyToMany;
      v.forEach((w) => this.m.set(w, multiplicity));
    });

    this.o = new Map();
    data.o?.forEach((v, i) => {
      const origin = i == 0 ? Origin.Workspace : Origin.Session;
      v.forEach((w) => this.o.set(w, origin));
    });

    this.d = new Set(data.d ?? []);
    this.r = new Set(data.r ?? []);
    this.u = new Set(data.u ?? []);

    this.t = new Map();
    if (data.t) {
      for (const mediaType in data.t) {
        data.t[mediaType].forEach((v) => this.t.set(v, mediaType));
      }
    }
  }
}
