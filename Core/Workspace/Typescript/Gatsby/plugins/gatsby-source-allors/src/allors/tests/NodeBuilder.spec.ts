
import { assert } from 'chai';
import 'mocha';
import { NodeBuilder } from '../NodeBuilder';

class FakeNodes {
  public nodes = [];

  public createNodeId = (id) => id;
  public createNode = (node) => { this.nodes.push(node); }

  public createContentDigest = (content) => content;
}

describe('NodeBuilder',
  () => {
    describe('build',
      () => {
        it('should return the source nodes', async () => {

          const fakeNodes = new FakeNodes();

          const args = {
            actions: {
              createNode: fakeNodes.createNode,
            },
            createNodeId: fakeNodes.createNodeId,
            createContentDigest: fakeNodes.createContentDigest,
          } as any

          const extra = {
            plugins: undefined,
            url: "http://localhost:5000/",
            login: "TestAuthentication/Token",
            user: "administrator",
            password: undefined
          }

          var gatsby = new NodeBuilder(args, extra);
          await gatsby.build();

          var medias = fakeNodes.nodes.filter(v=>v.internal.type === "AllorsMedia");

          assert.isNotEmpty(fakeNodes.nodes);
        });
      });
  });
