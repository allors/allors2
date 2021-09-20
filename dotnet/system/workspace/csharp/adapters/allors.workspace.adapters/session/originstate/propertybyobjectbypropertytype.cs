// <copyright file="PropertyByObjectByPropertyType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the ObjectBase type.</summary>

namespace Allors.Workspace.Adapters
{
    using System.Collections.Generic;
    using Meta;
    using Ranges;

    public class PropertyByObjectByPropertyType
    {
        private readonly IRanges<Strategy> ranges;
        private readonly IDictionary<IPropertyType, IDictionary<Strategy, object>> propertyByObjectByPropertyType;
        private IDictionary<IPropertyType, IDictionary<Strategy, object>> changedPropertyByObjectByPropertyType;

        public PropertyByObjectByPropertyType(IRanges<Strategy> ranges)
        {
            this.ranges = ranges;
            this.propertyByObjectByPropertyType = new Dictionary<IPropertyType, IDictionary<Strategy, object>>();
            this.changedPropertyByObjectByPropertyType = new Dictionary<IPropertyType, IDictionary<Strategy, object>>();
        }

        public object GetUnit(Strategy @object, IPropertyType propertyType) => this.Get(@object, propertyType);

        public void SetUnit(Strategy @object, IPropertyType propertyType, object newValue) => this.Set(@object, propertyType, newValue);

        public Strategy GetComposite(Strategy @object, IPropertyType propertyType) => (Strategy)this.Get(@object, propertyType);

        public void SetComposite(Strategy @object, IPropertyType propertyType, Strategy newValue) => this.Set(@object, propertyType, newValue);

        public IRange<Strategy> GetComposites(Strategy @object, IPropertyType propertyType) => this.ranges.Ensure(this.Get(@object, propertyType));

        public void SetComposites(Strategy @object, IPropertyType propertyType, IRange<Strategy> newValue) => this.Set(@object, propertyType, newValue);

        public void SetComposites(Strategy @object, IPropertyType propertyType, object newValue)
        {
            if (!(this.propertyByObjectByPropertyType.TryGetValue(propertyType, out var valueByPropertyType) && valueByPropertyType.TryGetValue(@object, out var originalValue)))
            {
                originalValue = null;
            }

            this.changedPropertyByObjectByPropertyType.TryGetValue(propertyType, out var changedValueByPropertyType);

            if (propertyType.IsOne ? Equals(newValue, originalValue) : this.ranges.Ensure(newValue).Equals(originalValue))
            {
                changedValueByPropertyType?.Remove(@object);
            }
            else
            {
                if (changedValueByPropertyType == null)
                {
                    changedValueByPropertyType = new Dictionary<Strategy, object>();
                    this.changedPropertyByObjectByPropertyType.Add(propertyType, changedValueByPropertyType);
                }

                changedValueByPropertyType[@object] = newValue;
            }
        }

        public IDictionary<IPropertyType, IDictionary<Strategy, object>> Checkpoint()
        {
            try
            {
                var changesSet = this.changedPropertyByObjectByPropertyType;

                foreach (var kvp in changesSet)
                {
                    var propertyType = kvp.Key;
                    var changedPropertyByObject = kvp.Value;

                    this.propertyByObjectByPropertyType.TryGetValue(propertyType, out var propertyByObject);

                    foreach (var kvp2 in changedPropertyByObject)
                    {
                        var @object = kvp2.Key;
                        var changedProperty = kvp2.Value;

                        if (changedProperty == null)
                        {
                            propertyByObject?.Remove(@object);
                        }
                        else
                        {
                            if (propertyByObject == null)
                            {
                                propertyByObject = new Dictionary<Strategy, object>();
                                this.propertyByObjectByPropertyType.Add(propertyType, propertyByObject);
                            }

                            propertyByObject[@object] = changedProperty;
                        }
                    }

                    if (propertyByObject?.Count == 0)
                    {
                        this.propertyByObjectByPropertyType.Remove(propertyType);
                    }
                }

                return changesSet;
            }
            finally
            {
                this.changedPropertyByObjectByPropertyType = new Dictionary<IPropertyType, IDictionary<Strategy, object>>();
            }
        }

        private object Get(Strategy @object, IPropertyType propertyType)
        {
            if (this.changedPropertyByObjectByPropertyType.TryGetValue(propertyType, out var changedPropertyByObject) && changedPropertyByObject.TryGetValue(@object, out var changedValue))
            {
                return changedValue;
            }

            if (this.propertyByObjectByPropertyType.TryGetValue(propertyType, out var propertyByObject) && propertyByObject.TryGetValue(@object, out var value))
            {
                return value;
            }

            return null;
        }

        private void Set(Strategy @object, IPropertyType propertyType, object newValue)
        {
            if (!(this.propertyByObjectByPropertyType.TryGetValue(propertyType, out var valueByPropertyType) && valueByPropertyType.TryGetValue(@object, out var originalValue)))
            {
                originalValue = null;
            }

            this.changedPropertyByObjectByPropertyType.TryGetValue(propertyType, out var changedValueByPropertyType);

            if (propertyType.IsOne ? Equals(newValue, originalValue) : this.ranges.Ensure(newValue).Equals(originalValue))
            {
                changedValueByPropertyType?.Remove(@object);
            }
            else
            {
                if (changedValueByPropertyType == null)
                {
                    changedValueByPropertyType = new Dictionary<Strategy, object>();
                    this.changedPropertyByObjectByPropertyType.Add(propertyType, changedValueByPropertyType);
                }

                changedValueByPropertyType[@object] = newValue;
            }
        }
    }
}
