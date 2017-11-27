//------------------------------------------------------------------------------------------------- 
// <copyright file="SessionCommittingEventArgs.cs" company="Allors bvba">
// Copyright 2002-2017 Allors bvba.
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
// <summary>Defines the SessionCommittingEventArgs type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    /// <summary>
    /// The EventHandler for the committing event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The committing event arguments.</param>
    public delegate void SessionCommittingEventHandler(object sender, SessionCommittingEventArgs args);

    /// <summary>
    /// The committing event arguments.
    /// </summary>
    public class SessionCommittingEventArgs
    {
        /// <summary>
        /// The session
        /// </summary>
        private readonly ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionCommittingEventArgs"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SessionCommittingEventArgs(ISession session)
        {
            this.session = session;
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <value>The session.</value>
        public ISession Session
        {
            get { return this.session; }
        }
    }
}