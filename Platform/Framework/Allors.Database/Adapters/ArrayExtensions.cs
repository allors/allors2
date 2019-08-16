// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrayExtensions.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
// 
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// 
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Adapters
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
            T[] dest = new T[source.Length - 1];
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
