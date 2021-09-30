// <copyright file="ArrayExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters
{
    using System;

    public static class ArrayExtensions
    {
        public static T[] Add<T>(this T[] source, T item)
        {
            var index = Array.IndexOf(source, item);
            if (index < 0)
            {
                var destination = new T[source.Length + 1];
                source.CopyTo(destination, 0);
                destination[destination.Length - 1] = item;
                return destination;
            }

            return source;
        }

        public static T[] Remove<T>(this T[] source, T item)
        {
            var index = Array.IndexOf(source, item);
            if (index >= 0)
            {
                return source.RemoveAt(index);
            }

            return source;
        }

        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var dest = new T[source.Length - 1];
            if (index > 0)
            {
                Array.Copy(source, 0, dest, 0, index);
            }

            if (index < source.Length - 1)
            {
                Array.Copy(source, index + 1, dest, index, source.Length - index - 1);
            }

            return dest;
        }
    }
}
