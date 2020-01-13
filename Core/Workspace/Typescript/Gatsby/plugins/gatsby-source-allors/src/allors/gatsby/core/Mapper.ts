import { Loaded } from "../../promise";

import { NodePluginArgs } from "gatsby"

export class Mapper {

  constructor(private args: NodePluginArgs, private options) {
  }

  public map(loaded: Loaded) {

    const { actions: { createNode }, createNodeId, createContentDigest } = this.args;

    const workspace = loaded.session.workspace;
    const ids = loaded.response.objects.map(v => v[0]);

    ids.forEach((id) => {

      const obj = workspace.get(id);
      const type = obj.objectType;

      const node = {
        id: createNodeId(`allors-${obj.id}`),
        parent: null,
        children: [],
        internal: {
          type: `Allors${type.name}`,
          contentDigest: createContentDigest(`allors-${obj.id}-${obj.version}`),
        }
      }

      type.roleTypes.forEach((roleType => {
        const role = obj.roleByRoleTypeId.get(roleType.id);

        if (!!role) {
          if (roleType.objectType.isUnit) {
            node[roleType.name] = role;
          } else {
            const propertyName = `${roleType.name}___NODE`;

            if (roleType.isOne) {
              if (ids.indexOf(role) > -1) {
                node[propertyName] = createNodeId(`allors-${role}`);
              }
            } else {
              const roleIds = (role as string[]).filter(w => ids.indexOf(w) > -1);
              if (roleIds.length > 0) {
                node[propertyName] = roleIds.map((w) => createNodeId(`allors-${w}`));
              }
            }
          }
        }

      }))

      createNode(node);
    })
  }

}
