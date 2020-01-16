import { GatsbySourceAllors } from './allors/GatsbySourceAllors';

export async function sourceNodes(args, extra) {

  var gatsby = new GatsbySourceAllors(args, extra);
  await gatsby.sourceNodes();
}
