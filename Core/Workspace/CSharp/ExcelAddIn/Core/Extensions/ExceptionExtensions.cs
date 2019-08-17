// <copyright file="ExceptionExtensions.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Excel
{
    using System;
    using System.Windows.Forms;

    using NLog;

    public static class ExceptionExtensions
    {
        public static void Handle(this Exception @this)
        {
            var logger = LogManager.GetCurrentClassLogger();
            logger.Error(@this);
            MessageBox.Show(@"System error occured. Please restart.");
        }
    }
}
