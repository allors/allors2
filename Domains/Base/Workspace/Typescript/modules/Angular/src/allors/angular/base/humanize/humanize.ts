export function humanize(input: string): string {
  let result = input && input
    .replace(/([a-z\d])([A-Z])/g, '$1 $2')
    .replace(/([A-Z]+)([A-Z][a-z\d]+)/g, '$1 $2');

  result = result && result.charAt(0).toUpperCase() + result.slice(1);

  return result;
}
