import { PluginOptions, NodePluginArgs, Actions, NodeInput } from 'gatsby';

import { workspace } from '../src';
import { NodeBuilder } from '../src/NodeBuilder';

import 'jest-extended';

class FakeNodes {
  nodes: NodeInput[] = [];

  private counter = 0;

  createNodeId = () => (this.counter++).toString();

  createNode = (node: NodeInput) => {
    this.nodes.push(node);
  };

  createContentDigest = (input: unknown) => input.toString();
}

describe('NodeBuilder', () => {
  describe('build', () => {
    it('should return the source nodes', async () => {
      const fakeNodes = new FakeNodes();

      const actions: Partial<Actions> = {
        createNode: fakeNodes.createNode,
      };

      const args: Partial<NodePluginArgs> = {
        actions: actions as Actions,
        createNodeId: fakeNodes.createNodeId,
        createContentDigest: fakeNodes.createContentDigest,
      };

      const extra: PluginOptions = {
        plugins: undefined,
        url: 'http://localhost:5000/',
        login: 'TestAuthentication/Token',
        user: 'koen@dipu.com',
        password: undefined,
      };

      const nodeBuilder = new NodeBuilder(workspace, args as NodePluginArgs, extra);
      await nodeBuilder.build();

      expect(fakeNodes.nodes).not.toBeEmpty();
    });
  });
});
