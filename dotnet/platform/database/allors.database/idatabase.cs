// <copyright file="IDatabase.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Xml;

    using Allors.Meta;
    using Serialization;

    /// <summary>
    /// A database is an online <see cref="IDatabase"/>.
    /// </summary>
    public interface IDatabase
    {
        /// <summary>
        /// Occurs when an object could not be loaded.
        /// </summary>
        [Obsolete]
        event ObjectNotLoadedEventHandler ObjectNotLoaded;

        /// <summary>
        /// Occurs when a relation could not be loaded.
        /// </summary>
        [Obsolete]
        event RelationNotLoadedEventHandler RelationNotLoaded;

        /// <summary>
        /// Gets a value indicating whether this database is shared with other databases with the same name.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this database is shared; otherwise, <c>false</c>.
        /// </value>
        bool IsShared { get; }

        /// <summary>
        ///  Gets.
        /// <ul>
        /// <li>the id of this database</li>
        /// <li>the id of the database from this workspace</li>
        /// </ul>
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Gets the object factory.
        /// </summary>
        /// <value>The object factory.</value>
        IObjectFactory ObjectFactory { get; }

        /// <summary>
        /// Gets the meta domain of this population.
        /// </summary>
        IMetaPopulation MetaPopulation { get; }

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Initializes the database. If this population is persistent then
        /// all existing objects will be deleted.
        /// </summary>
        void Init();

        /// <summary>
        /// Creates a new database Session.
        /// </summary>
        /// <returns>a newly created AllorsSession.</returns>
        ISession CreateSession();

        /// <summary>
        /// Loads the population from the <see cref="XmlReader"/>.
        /// </summary>
        /// <param name="reader">The reader.</param>
        [Obsolete]
        void Load(XmlReader reader);

        /// <summary>
        /// Saves the population to the <see cref="XmlWriter"/>.
        /// </summary>
        /// <param name="writer">The writer.</param>
        [Obsolete]
        void Save(XmlWriter writer);

        void Load(IPopulationData data);

        IPopulationData Save();
    }
}
