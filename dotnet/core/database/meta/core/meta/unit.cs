// <copyright file="Unit.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Meta
{
    using System;

    public sealed partial class Unit : ObjectType, IUnit
    {
        private UnitTags unitTag;

        private Type clrType;

        internal Unit(MetaPopulation metaPopulation, Guid id)
            : base(metaPopulation)
        {
            this.Id = id;
            metaPopulation.OnUnitCreated(this);
        }

        public UnitTags UnitTag
        {
            get => this.unitTag;

            set
            {
                this.MetaPopulation.AssertUnlocked();
                this.unitTag = value;
                this.MetaPopulation.Stale();
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a binary.
        /// </summary>
        /// <value><c>true</c> if this instance is a binary; otherwise, <c>false</c>.</value>
        public bool IsBinary => this.Id.Equals(UnitIds.Binary);

        /// <summary>
        /// Gets a value indicating whether this instance is a boolean.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean => this.Id.Equals(UnitIds.Boolean);

        /// <summary>
        /// Gets a value indicating whether this instance is a date time.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a date time; otherwise, <c>false</c>.
        /// </value>
        public bool IsDateTime => this.Id.Equals(UnitIds.DateTime);

        /// <summary>
        /// Gets a value indicating whether this instance is a decimal.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a decimal; otherwise, <c>false</c>.
        /// </value>
        public bool IsDecimal => this.Id.Equals(UnitIds.Decimal);

        /// <summary>
        /// Gets a value indicating whether this instance is a float.
        /// </summary>
        /// <value><c>true</c> if this instance is a float; otherwise, <c>false</c>.</value>
        public bool IsFloat => this.Id.Equals(UnitIds.Float);

        /// <summary>
        /// Gets a value indicating whether this instance is an integer.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is an integer; otherwise, <c>false</c>.
        /// </value>
        public bool IsInteger => this.Id.Equals(UnitIds.Integer);

        /// <summary>
        /// Gets a value indicating whether this instance is a string.
        /// </summary>
        /// <value><c>true</c> if this instance is a string; otherwise, <c>false</c>.</value>
        public bool IsString => this.Id.Equals(UnitIds.String);

        /// <summary>
        /// Gets a value indicating whether this instance is a unique.
        /// </summary>
        /// <value><c>true</c> if this instance is a unique; otherwise, <c>false</c>.</value>
        public bool IsUnique => this.Id.Equals(UnitIds.Unique);

        public override Type ClrType => this.clrType;

        internal void Bind()
        {
            switch (this.UnitTag)
            {
                case UnitTags.Binary:
                    this.clrType = typeof(byte[]);
                    break;

                case UnitTags.Boolean:
                    this.clrType = typeof(bool);
                    break;

                case UnitTags.DateTime:
                    this.clrType = typeof(DateTime);
                    break;

                case UnitTags.Decimal:
                    this.clrType = typeof(decimal);
                    break;

                case UnitTags.Float:
                    this.clrType = typeof(double);
                    break;

                case UnitTags.Integer:
                    this.clrType = typeof(int);
                    break;

                case UnitTags.String:
                    this.clrType = typeof(string);
                    break;

                case UnitTags.Unique:
                    this.clrType = typeof(Guid);
                    break;
            }
        }
    }
}
