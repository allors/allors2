//------------------------------------------------------------------------------------------------- 
// <copyright file="Unit.cs" company="Allors bvba">
// Copyright 2002-2013 Allors bvba.
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
// <summary>Defines the IObjectType type.</summary>
//-------------------------------------------------------------------------------------------------

namespace Allors.Meta
{
    using System;

    public abstract partial class Unit : ObjectType, IUnit
    {
        private UnitTags unitTag;

        private Type clrType;

        internal Unit(MetaPopulation metaPopulation)
            : base(metaPopulation)
        {
            metaPopulation.OnUnitCreated(this);
        }

        public UnitTags UnitTag
        {
            get
            {
                return this.unitTag;
            }

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
        public bool IsBinary
        {
            get { return this.Id.Equals(UnitIds.BinaryId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a boolean.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a boolean; otherwise, <c>false</c>.
        /// </value>
        public bool IsBoolean
        {
            get { return this.Id.Equals(UnitIds.BooleanId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a date time.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is a date time; otherwise, <c>false</c>.
        /// </value>
        public bool IsDateTime
        {
            get { return this.Id.Equals(UnitIds.DateTimeId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a decimal.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is a decimal; otherwise, <c>false</c>.
        /// </value>
        public bool IsDecimal
        {
            get { return this.Id.Equals(UnitIds.DecimalId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a float.
        /// </summary>
        /// <value><c>true</c> if this instance is a float; otherwise, <c>false</c>.</value>
        public bool IsFloat
        {
            get { return this.Id.Equals(UnitIds.FloatId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is an integer.
        /// </summary>
        /// <value>
        ///  <c>true</c> if this instance is an integer; otherwise, <c>false</c>.
        /// </value>
        public bool IsInteger
        {
            get { return this.Id.Equals(UnitIds.IntegerId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a string.
        /// </summary>
        /// <value><c>true</c> if this instance is a string; otherwise, <c>false</c>.</value>
        public bool IsString
        {
            get { return this.Id.Equals(UnitIds.StringId); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is a unique.
        /// </summary>
        /// <value><c>true</c> if this instance is a unique; otherwise, <c>false</c>.</value>
        public bool IsUnique
        {
            get { return this.Id.Equals(UnitIds.UniqueId); }
        }

        public override Type ClrType
        {
            get
            {
                return this.clrType;
            }
        }

        internal void Bind()
        {
            switch (this.UnitTag)
            {
                case UnitTags.AllorsBinary:
                    this.clrType = typeof(byte[]);
                    break;

                case UnitTags.AllorsBoolean:
                    this.clrType = typeof(bool);
                    break;

                case UnitTags.AllorsDateTime:
                    this.clrType = typeof(DateTime);
                    break;

                case UnitTags.AllorsDecimal:
                    this.clrType = typeof(decimal);
                    break;

                case UnitTags.AllorsFloat:
                    this.clrType = typeof(double);
                    break;

                case UnitTags.AllorsInteger:
                    this.clrType = typeof(int);
                    break;

                case UnitTags.AllorsString:
                    this.clrType = typeof(string);
                    break;

                case UnitTags.AllorsUnique:
                    this.clrType = typeof(Guid);
                    break;
            }
        }
    }
}