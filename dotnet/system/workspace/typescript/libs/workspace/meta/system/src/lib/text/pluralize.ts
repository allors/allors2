export function pluralize(singular: string) {
  if (singular.endsWith('y') && !singular.endsWith('ay') && !singular.endsWith('ey') && !singular.endsWith('iy') && !singular.endsWith('oy') && !singular.endsWith('uy')) {
    return singular.substring(0, singular.length - 1) + 'ies';
  }

  if (singular.endsWith('us')) {
    return singular + 'es';
  }

  if (singular.endsWith('ss')) {
    return singular + 'es';
  }

  if (singular.endsWith('x') || singular.endsWith('ch') || singular.endsWith('sh')) {
    return singular + 'es';
  }

  if (singular.endsWith('f') && singular.length > 1) {
    return singular.substring(0, singular.length - 1) + 'ves';
  }

  if (singular.endsWith('fe') && singular.length > 2) {
    return singular.substring(0, singular.length - 2) + 'ves';
  }

  return singular + 's';
}
