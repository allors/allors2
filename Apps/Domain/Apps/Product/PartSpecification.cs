// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PartSpecification.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// 
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// 
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// 
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public static partial class PartSpecificationExtensions
    {
        public static void AppsOnDerive(this PartSpecification partSpecification, ObjectOnDerive method)
        {
            if (partSpecification.ExistCurrentObjectState && !partSpecification.CurrentObjectState.Equals(partSpecification.PreviousObjectState))
            {
                var currentStatus = new PartSpecificationStatusBuilder(partSpecification.Strategy.Session).WithPartSpecificationObjectState(partSpecification.CurrentObjectState).Build();
                partSpecification.AddPartSpecificationStatus(currentStatus);
                partSpecification.CurrentPartSpecificationStatus = currentStatus;
            }

            if (partSpecification.ExistCurrentObjectState)
            {
                partSpecification.CurrentObjectState.Process(partSpecification);
            }
        }

        public static void AppsApprove(this PartSpecification partSpecification)
        {
            partSpecification.CurrentObjectState = new PartSpecificationObjectStates(partSpecification.Strategy.Session).Approved;
        }
    }
}
