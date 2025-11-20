// <copyright file="IValidation.cs" company="Allors bv">
// Copyright (c) Allors bv. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Derivations
{
    public interface IIteration
    {
        IAccumulatedChangeSet ChangeSet { get; }

        object this[string name] { get; set; }

        ICycle Cycle { get; }

        IPreparation Preparation { get; }

        void Schedule(Object @object);

        void Mark(Object @object);

        void Mark(params Object[] objects);

        bool IsMarked(Object @object);

        /// <summary>
        /// The dependencies are derived before the dependent object.
        /// </summary>
        /// <param name="dependent">The dependent object.</param>
        /// <param name="dependencies">The dependencies.</param>
        void AddDependency(Object dependent, params Object[] dependencies);
    }
}
