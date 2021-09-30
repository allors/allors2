namespace Allors.Domain
{
    export function extend(type, extension) {
        Object.getOwnPropertyNames(extension).forEach(name => {
            var ownPropertyDescriptor = Object.getOwnPropertyDescriptor(extension, name);
            Object.defineProperty(type.prototype, name, ownPropertyDescriptor);
        });
    }
}