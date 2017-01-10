//------------------------------------------------------------------------------------------------- 
// <copyright file="SessionRolledBackEventArgs.cs" company="Allors bvba">
// Copyright 2002-2016 Allors bvba.
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
// <summary>Defines the SessionRolledBackEventArgs type.</summary>
//-------------------------------------------------------------------------------------------------
namespace Allors
{
    /// <summary>
    /// The EventHandler for the rolled back event.
    /// </summary>
    /// <param name="sender">The sender.</param>
    /// <param name="args">The rolled back event arguments.</param>
    public delegate void SessionRolledBackEventHandler(object sender, SessionRolledBackEventArgs args);

    /// <summary>
    /// The rolled back event arguments.
    /// </summary>
    public class SessionRolledBackEventArgs
    {
        /// <summary>
        /// The session.
        /// </summary>
        private readonly ISession session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionRolledBackEventArgs"/> class.
        /// </summary>
        /// <param name="session">The session.</param>
        public SessionRolledBackEventArgs(ISession session)
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