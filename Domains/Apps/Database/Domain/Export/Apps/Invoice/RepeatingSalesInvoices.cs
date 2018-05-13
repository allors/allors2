// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RepeatingSalesInvoices.cs" company="Allors bvba">
//   Copyright 2002-2012 Allors bvba.
// Dual Licensed under
//   a) the General Public Licence v3 (GPL)
//   b) the Allors License
// The GPL License is included in the file gpl.txt.
// The Allors License is an addendum to your contract.
// Allors Applications is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Domain
{
    public partial class RepeatingSalesInvoices
    {
        public static void Daily(ISession session, ILogger logger)
        {
            var repeatingSalesInvoices = new RepeatingSalesInvoices(session).Extent();

            foreach (RepeatingSalesInvoice repeatingSalesInvoice in repeatingSalesInvoices)
            {
                if (repeatingSalesInvoice.NextExecutionDate.Date == session.Now().Date)
                {
                    repeatingSalesInvoice.Repeat();
                }
            }

            var derivation = new NonLogging.Derivation(session, new DerivationConfig());
            var validation = derivation.Derive();

            if (validation.HasErrors)
            {
                foreach (var error in validation.Errors)
                {
                    logger?.Error(error.Message);
                }

                session.Rollback();
            }
            else
            {
                session.Commit();
            }
        }
    }
}
