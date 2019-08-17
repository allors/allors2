// <copyright file="Commands.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using System;
    using System.Threading.Tasks;

    public partial class Commands
    {
        public async Task PeopleNew()
        {
            var sheet = this.Sheets.CreatePeople();
            await sheet.Refresh();
        }

        private void OnError(Exception e) => e.Handle();
    }
}
