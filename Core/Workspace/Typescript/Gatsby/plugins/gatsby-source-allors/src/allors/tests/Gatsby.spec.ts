
import { assert } from 'chai';
import 'mocha';
import { GatsbySourceAllors } from '../GatsbySourceAllors';

class FakeNodes {
  public nodes = [];

  public createNodeId = (id) => id;
  public createNode = (node) => { this.nodes.push(node); }

  public createContentDigest = (content) => content;
}

describe('gatsby-node',
  () => {
    describe('sourceNodes',
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

          var gatsby = new GatsbySourceAllors(args, extra);
          await gatsby.sourceNodes();

          assert.isNotEmpty(fakeNodes.nodes);
        });
      });
  });
