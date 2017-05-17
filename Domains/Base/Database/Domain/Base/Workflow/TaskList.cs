// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TaskList.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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
    using System.Linq;
    
    public partial class TaskList
    {
        public override string ToString()
        {   
            return this.UserWhereTaskList.UserName + ": " + this.OpenTaskAssignments.Count + " open tasks.";
        }

        public void BaseOnDerive(ObjectOnDerive method)
        {
            if (!this.ExistUserWhereTaskList)
            {
                this.Delete();
            }
            else
            {
                this.TaskAssignments = this.UserWhereTaskList.TaskAssignmentsWhereUser;
                this.OpenTaskAssignments = this.TaskAssignments.Where(v => v.ExistTask && !v.Task.ExistDateClosed).ToArray();
                this.Count = this.OpenTaskAssignments.Count;
            }
        }
    }
}
