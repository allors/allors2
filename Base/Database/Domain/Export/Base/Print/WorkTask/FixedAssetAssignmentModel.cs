// <copyright file="FixedAssetAssignmentModel.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Domain.Print.WorkTaskModel
{
    public class FixedAssetAssignmentModel
    {
        public FixedAssetAssignmentModel(WorkEffortFixedAssetAssignment assignment)
        {
            this.Name = assignment.FixedAsset?.Name;
            this.Comment = assignment.FixedAsset?.Comment;

            if (assignment.FixedAsset is SerialisedItem serialisedItem)
            {
                this.CustomerReferenceNumber = serialisedItem.CustomerReferenceNumber;
                this.ItemNumber = serialisedItem.ItemNumber;
                this.SerialNumber = serialisedItem.SerialNumber;
                this.Brand = serialisedItem.PartWhereSerialisedItem?.Brand?.Name;
                this.Model = serialisedItem.PartWhereSerialisedItem?.Model?.Name;
            }
        }

        public string Name { get; }
        public string CustomerReferenceNumber { get; }
        public string SerialNumber { get; }
        public string ItemNumber { get; }
        public string Brand { get; }
        public string Model { get; }
        public string Comment { get; }
    }
}
