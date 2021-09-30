const reservedWords = new Set(['id', 'children', 'parent', 'fields', 'internal']);

function escape(value: string) {
  if (reservedWords.has(value)) {
    return `_${value}`;
  }

  return value;
}

function camel(value: string) {
  return value.replace(/^\w/, (c) => c.toLowerCase());
}

export function createName(name: string) {
  return escape(camel(name));
}
