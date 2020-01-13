import { Gatsby } from './allors/gatsby/core/Gatsby';

export async function sourceNodes(args, extra) {

  var gatsby = new Gatsby(args, extra);
  await gatsby.sourceNodes();
}
