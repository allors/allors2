// <copyright file="ITransaction.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database
{
    using System;
    using System.Collections.Generic;

    using Meta;

    /// <summary>
    /// Extends the <see cref="ITransaction"/> with database capabilities.
    /// </summary>
    public interface ITransaction : IDisposable
    {
        /// <summary>
        /// Gets the database.
        /// </summary>
        IDatabase Database { get; }

        /// <summary>
        /// The services for this transaction.
        /// </summary>
        ITransactionServices Services { get; }

        /// <summary>
        /// Creates a change set of all changes up to this checkpoint,
        /// starting from either the beginning of the transaction or
        /// from a previous checkpoint.
        /// </summary>
        /// <returns>The change set.</returns>
        IChangeSet Checkpoint();

        /// <summary>
        /// Creates an Extent for the specified IObjectType.
        /// Only works for static domains.
        /// </summary>
        /// <typeparam name="T">The type for the extent.</typeparam>
        /// <returns>The extent.</returns>
        Extent<T> Extent<T>() where T : IObject;

        /// <summary>
        /// Creates an Extent for the specified <see cref="IObjectType"/>.
        /// </summary>
        /// <param name="objectType">The @class.</param>
        /// <returns>The extent.</returns>
        Extent Extent(IComposite objectType);

        /// <summary>
        /// Creates an Extent that is the exception of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The except extent.</returns>
        Extent Except(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Creates an Extent that is the Intersect of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The intersect extent.</returns>
        Extent Intersect(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Creates an Extent that is the Union of its two operands.
        /// </summary>
        /// <param name="firstOperand">The first operand.</param>
        /// <param name="secondOperand">The second operand.</param>
        /// <returns>The union extent.</returns>
        Extent Union(Extent firstOperand, Extent secondOperand);

        /// <summary>
        /// Commits all changes that where made during this transaction.
        /// Because transactions are rolling, a new transaction is automatically created.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rolls back all changes that where made during this transaction.
        /// Because transactions are rolling, a new transaction is automatically created.
        /// </summary>
        void Rollback();

        /// <summary>
        /// Creates an Allors Object.
        /// Only works for static domains.
        /// </summary>
        /// <typeparam name="T">The IObjectType.</typeparam>
        /// <returns>a new <see cref="IObject"/>.</returns>
        T Create<T>() where T : IObject;

        /// <summary>
        /// Creates an Allors Object.
        /// </summary>
        /// <param name="objectType">The IObjectType.</param>
        /// <returns>a new <see cref="IObject"/>.</returns>
        IObject Create(IClass objectType);

        /// <summary>
        /// Creates a specified amount of AllorsObjects.
        /// </summary>
        /// <param name="objectType">The IObjectType.</param>
        /// <param name="count">The count.</param>
        /// <returns>The created objects.</returns>
        IObject[] Create(IClass objectType, int count);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(IObject obj);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(string objectId);

        /// <summary>
        /// Instantiates an Allors Object.
        /// </summary>
        /// <param name="objectId">The object id.</param>
        /// <returns>The instantiated object.</returns>
        IObject Instantiate(long objectId);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objects">The objects.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(IEnumerable<IObject> objects);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objectIds">The object ids.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(IEnumerable<string> objectIds);

        /// <summary>
        /// Instantiates an array of Allors Objects.
        /// </summary>
        /// <param name="objectIds">The object ids.</param>
        /// <returns>The instantiated objects.</returns>
        IObject[] Instantiate(IEnumerable<long> objectIds);

        void Prefetch(PrefetchPolicy prefetchPolicy, params IObject[] objects);

        void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<string> objectIds);

        void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<long> objectIds);

        void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IStrategy> strategies);

        void Prefetch(PrefetchPolicy prefetchPolicy, IEnumerable<IObject> objects);

        /// <summary>
        /// Instantiate a strategy.
        /// This method is primarily used by <see cref="IWorkspace"/>s.
        /// </summary>
        /// <param name="objectId">
        /// The object id.
        /// </param>
        /// <returns>
        /// The <see cref="IStrategy"/>.
        /// </returns>
        IStrategy InstantiateStrategy(long objectId);
    }
}
