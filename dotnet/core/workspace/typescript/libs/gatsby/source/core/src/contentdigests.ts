import { NodePluginArgs } from 'gatsby';
import { Loaded } from '@allors/promise/core';

export class ContentDigests {
  byId: { [id: string]: string };

  constructor(private args: NodePluginArgs) {}

  onLoad(loaded: Loaded): void {
    const { createContentDigest } = this.args;

    this.byId = loaded.response.objects.reduce((acc, value) => {
      const id = value[0];
      const version = value[1];
      const accessControls = value.length > 2 ? value[2] : undefined;
      const deniedPermissions = value.length > 3 ? value[3] : undefined;

      let contentDigest = `allors-${id}-${version}`;
      if (accessControls) {
        contentDigest += `${accessControls}`;
      }

      if (deniedPermissions) {
        contentDigest += `${deniedPermissions}`;
      }

      acc[id] = createContentDigest(contentDigest);

      return acc;
    }, {} as { [key: string]: string });
  }
}
