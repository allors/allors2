import { CreateNodeArgs, PluginOptions } from "gatsby";
import { createRemoteFileNode } from "gatsby-source-filesystem";

export class SourceMedia {

  url: string;

  constructor(private args: CreateNodeArgs, options: PluginOptions) {
    const { node  } = args;

    const fileName = node.fileName as string
    const fileComponent = fileName ? "/" + encodeURIComponent(fileName) : '';
    this.url = (options["url"] as string) + "media/" + node.uniqueId + fileComponent;
  }

  async onCreateNode() {

    const { node, cache, store, createNodeId, reporter, actions: { createNode } } = this.args;

    if (node.internal.type === "AllorsMedia") {
      try {
        const fileNode = await createRemoteFileNode({
          url: this.url,
          parentNodeId: node.id,
          store,
          cache,
          createNode,
          createNodeId,
          reporter
        });

        node.file___NODE = fileNode.id;
      } catch (e) {
        reporter.panicOnBuild("Could not create remote file node for " + this.url, e);
      }
    }
  }
}
