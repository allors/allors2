// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValuesTest.cs" company="Allors bvba">
//   Copyright 2002-2010 Allors bvba.
// Dual Licensed under
//   a) the Lesser General Public Licence v3 (LGPL)
//   b) the Allors License
// The LGPL License is included in the file lgpl.txt.
// The Allors License is an addendum to your contract.
// Allors Platform is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// For more information visit http://www.allors.com/legal
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Allors.Database.Special.MySqlClient
{
    using System;
    using Domain;
    using NUnit.Framework;

    [TestFixture]
    public abstract class UnitTest : Special.UnitTest
    {
        protected DateTime MaximumDatetime
        {
            get { return new DateTime(9999, 12, 31, 11, 59, 00, 00); }
        }

        protected DateTime MinimumDatetime
        {
            get { return new DateTime(1753, 1, 1, 12, 0, 0, 0); }
        }

        protected override bool UseDoubleMaximum
        {
            get
            {
                return false;
            }
        }

        protected override bool UseDoubleMinimum
        {
            get
            {
                return false;
            }
        }

        [Test]
        public override void AllorsDate()
        {
            {
// year, day & month
                C1 values = C1.Create(this.Session);
                values.C1AllorsDate = new DateTime(1973, 03, 27, 1, 1, 1);
                values.I1AllorsDate = new DateTime(1973, 03, 27, 2, 2, 2);
                Assert.AreEqual(new DateTime(1973, 03, 27), values.C1AllorsDate);
                Assert.AreEqual(new DateTime(1973, 03, 27), values.I1AllorsDate);
            }

            {
                // Minimum
                C1 values = C1.Create(this.Session);
                values.C1AllorsDate = this.MinimumDatetime;
                values.I1AllorsDate = this.MinimumDatetime;
                Assert.AreEqual(this.MinimumDatetime.Date, values.C1AllorsDate);
                Assert.AreEqual(this.MinimumDatetime.Date, values.I1AllorsDate);
            }

            {
                // Maximum
                C1 values = C1.Create(this.Session);
                values.C1AllorsDate = this.MaximumDatetime;
                values.I1AllorsDate = this.MaximumDatetime;
                Assert.AreEqual(this.MaximumDatetime.Date, values.C1AllorsDate);
                Assert.AreEqual(this.MaximumDatetime.Date, values.I1AllorsDate);
            }

            {
                // initial empty
                C1 values = C1.Create(this.Session);

                DateTime value = DateTime.Now;
                DateTime valueOld = value;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDate);
                Assert.IsFalse(values.ExistI1AllorsDate);
                Assert.IsFalse(values.ExistS1AllorsDate);

                this.Session.Commit();

                value = DateTime.Now;
                valueOld = value;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDate);
                Assert.IsFalse(values.ExistI1AllorsDate);
                Assert.IsFalse(values.ExistS1AllorsDate);
            }

            {
                // reset empty
                C1 values = C1.Create(this.Session);
                values.C1AllorsDate = DateTime.Now;
                values.I1AllorsDate = DateTime.Now;
                values.S1AllorsDate = DateTime.Now;

                this.Session.Commit();

                Assert.IsTrue(values.ExistC1AllorsDate);
                Assert.IsTrue(values.ExistI1AllorsDate);
                Assert.IsTrue(values.ExistS1AllorsDate);

                values.RemoveC1AllorsDate();
                values.RemoveI1AllorsDate();
                values.RemoveS1AllorsDate();

                DateTime value = DateTime.Now;
                DateTime valueOld = value;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDate);
                Assert.IsFalse(values.ExistI1AllorsDate);
                Assert.IsFalse(values.ExistS1AllorsDate);

                this.Session.Commit();

                value = DateTime.Now;
                valueOld = value;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDate;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDate);
                Assert.IsFalse(values.ExistI1AllorsDate);
                Assert.IsFalse(values.ExistS1AllorsDate);
            }
        }
        
        [Test]
        public override void AllorsDateTime()
        {
            {
                // year, day & month
            C1 values = C1.Create(this.Session);
                values.C1AllorsDateTime = new DateTime(1973, 03, 27);
                values.I1AllorsDateTime = new DateTime(1973, 03, 27);
                Assert.AreEqual(new DateTime(1973, 03, 27), values.C1AllorsDateTime);
                Assert.AreEqual(new DateTime(1973, 03, 27), values.I1AllorsDateTime);
            }

            {
                // Minimum
                C1 values = C1.Create(this.Session);
                values.C1AllorsDateTime = this.MinimumDatetime;
                values.I1AllorsDateTime = this.MinimumDatetime;
                Assert.AreEqual(this.MinimumDatetime, values.C1AllorsDateTime);
                Assert.AreEqual(this.MinimumDatetime, values.I1AllorsDateTime);
            }

            {
                // Maximum
                C1 values = C1.Create(this.Session);
                values.C1AllorsDateTime = this.MaximumDatetime;
                values.I1AllorsDateTime = this.MaximumDatetime;
                Assert.AreEqual(this.MaximumDatetime, values.C1AllorsDateTime);
                Assert.AreEqual(this.MaximumDatetime, values.I1AllorsDateTime);
            }
            
            {
                // initial empty
                C1 values = C1.Create(this.Session);

                DateTime value = DateTime.Now;
                DateTime valueOld = value;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDateTime);
                Assert.IsFalse(values.ExistI1AllorsDateTime);
                Assert.IsFalse(values.ExistS1AllorsDateTime);

                this.Session.Commit();

                value = DateTime.Now;
                valueOld = value;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDateTime);
                Assert.IsFalse(values.ExistI1AllorsDateTime);
                Assert.IsFalse(values.ExistS1AllorsDateTime);
            }
            
            {
                // reset empty
                C1 values = C1.Create(this.Session);
                values.C1AllorsDateTime = DateTime.Now;
                values.I1AllorsDateTime = DateTime.Now;
                values.S1AllorsDateTime = DateTime.Now;

                this.Session.Commit();

                Assert.IsTrue(values.ExistC1AllorsDateTime);
                Assert.IsTrue(values.ExistI1AllorsDateTime);
                Assert.IsTrue(values.ExistS1AllorsDateTime);

                values.RemoveC1AllorsDateTime();
                values.RemoveI1AllorsDateTime();
                values.RemoveS1AllorsDateTime();

                DateTime value = DateTime.Now;
                DateTime valueOld = value;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDateTime);
                Assert.IsFalse(values.ExistI1AllorsDateTime);
                Assert.IsFalse(values.ExistS1AllorsDateTime);

                this.Session.Commit();

                value = DateTime.Now;
                valueOld = value;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDateTime;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(valueOld, value);

                Assert.IsFalse(values.ExistC1AllorsDateTime);
                Assert.IsFalse(values.ExistI1AllorsDateTime);
                Assert.IsFalse(values.ExistS1AllorsDateTime);
            }
        }
        
        [Test]
        public override void AllorsDecimal()
        {
            {
                // Positive
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 10.10m;
                values.I1AllorsDecimal = 10.10m;
                values.S1AllorsDecimal = 10.10m;

                this.Session.Commit();

                Assert.AreEqual(10.10m, values.C1AllorsDecimal);
                Assert.AreEqual(10.10m, values.I1AllorsDecimal);
                Assert.AreEqual(10.10m, values.S1AllorsDecimal);
            }

            {
                // Negative
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = -10.10m;
                values.I1AllorsDecimal = -10.10m;
                values.S1AllorsDecimal = -10.10m;

                this.Session.Commit();

                Assert.AreEqual(-10.10m, values.C1AllorsDecimal);
                Assert.AreEqual(-10.10m, values.I1AllorsDecimal);
                Assert.AreEqual(-10.10m, values.S1AllorsDecimal);
            }

            {
                // Zero
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 0m;
                values.I1AllorsDecimal = 0m;
                values.S1AllorsDecimal = 0m;

                this.Session.Commit();

                Assert.AreEqual(0m, values.C1AllorsDecimal);
                Assert.AreEqual(0m, values.I1AllorsDecimal);
                Assert.AreEqual(0m, values.S1AllorsDecimal);
            }

            {
                // initial empty
                C1 values = C1.Create(this.Session);

                decimal value = -1;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(-1, value);

                Assert.IsFalse(values.ExistC1AllorsDecimal);
                Assert.IsFalse(values.ExistI1AllorsDecimal);
                Assert.IsFalse(values.ExistS1AllorsDecimal);

                this.Session.Commit();

                value = -1;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(-1, value);

                Assert.IsFalse(values.ExistC1AllorsDecimal);
                Assert.IsFalse(values.ExistI1AllorsDecimal);
                Assert.IsFalse(values.ExistS1AllorsDecimal);
            }

            {
                // reset empty
                C1 values = C1.Create(this.Session);
                values.C1AllorsDecimal = 10.10m;
                values.I1AllorsDecimal = 10.10m;
                values.S1AllorsDecimal = 10.10m;

                this.Session.Commit();

                Assert.IsTrue(values.ExistC1AllorsDecimal);
                Assert.IsTrue(values.ExistI1AllorsDecimal);
                Assert.IsTrue(values.ExistS1AllorsDecimal);

                values.RemoveC1AllorsDecimal();
                values.RemoveI1AllorsDecimal();
                values.RemoveS1AllorsDecimal();

                decimal value = -1;
                bool exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(-1, value);

                Assert.IsFalse(values.ExistC1AllorsDecimal);
                Assert.IsFalse(values.ExistI1AllorsDecimal);
                Assert.IsFalse(values.ExistS1AllorsDecimal);

                this.Session.Commit();
                value = -1;
                exceptionThrown = false;
                try
                {
                    value = values.C1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.I1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                exceptionThrown = false;
                try
                {
                    value = values.S1AllorsDecimal;
                }
                catch
                {
                    exceptionThrown = true;
                }

                Assert.IsTrue(exceptionThrown);
                Assert.AreEqual(-1, value);

                Assert.IsFalse(values.ExistC1AllorsDecimal);
                Assert.IsFalse(values.ExistI1AllorsDecimal);
                Assert.IsFalse(values.ExistS1AllorsDecimal);
            }
        }
    }
}