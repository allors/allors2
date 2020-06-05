// <copyright file="FixedAssetAssignmentModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

using System.Linq;
using Allors.Meta;

namespace Allors.Domain.Print.WorkTaskModel
{
    public class FixedAssetAssignmentModel
    {
        public FixedAssetAssignmentModel(WorkEffortFixedAssetAssignment assignment)
        {
            var session = assignment.Strategy.Session;

            this.Name = assignment.FixedAsset?.Name;
            this.Comment = assignment.FixedAsset?.Comment?.Split('\n');
            
            if (assignment.FixedAsset is SerialisedItem serialisedItem)
            {
                this.CustomerReferenceNumber = serialisedItem.CustomerReferenceNumber;
                this.ItemNumber = serialisedItem.ItemNumber;
                this.SerialNumber = serialisedItem.SerialNumber;
                this.Brand = serialisedItem.PartWhereSerialisedItem?.Brand?.Name;
                this.Model = serialisedItem.PartWhereSerialisedItem?.Model?.Name;

                var hoursType = new SerialisedItemCharacteristicTypes(session).FindBy(M.SerialisedItemCharacteristicType.Name, "Operating Hours");
                var hoursCharacteristic = serialisedItem.SerialisedItemCharacteristics.FirstOrDefault(v => v.SerialisedItemCharacteristicType.Equals(hoursType));
                this.Hours = $"{hoursCharacteristic?.Value} {hoursType?.UnitOfMeasure?.Abbreviation}";
            }
        }

        public string Name { get; }

        public string CustomerReferenceNumber { get; }

        public string SerialNumber { get; }

        public string ItemNumber { get; }

        public string Brand { get; }

        public string Model { get; }

        public string Hours { get; }

        public string[] Comment { get; }
    }
}
