export class FactoryConfig {

  constructor(fields: Partial<FactoryConfig>) {
    Object.assign(this, fields);
  }

  items: FactoryConfigItem[];
}

export interface FactoryConfigItem {
  id: string;
  component: any;
}
