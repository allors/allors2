// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtentOperation.cs" company="Allors bvba">
//   Copyright 2002-2017 Allors bvba.
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

namespace Allors.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Allors.Meta;

    internal sealed class ExtentOperation : Extent
    {
        private readonly Extent firstOperand;
        private readonly ExtentOperationType operationType;
        private readonly Extent secondOperand;

        public ExtentOperation(Session session, Extent firstOperand,  Extent secondOperand,  ExtentOperationType operationType)
            : base(session)
        {
            if (!firstOperand.ObjectType.Equals(secondOperand.ObjectType))
            {
                throw new ArgumentException("Both extents in a Union, Intersect or Except must be from the same type");
            }

            this.operationType = operationType;

            this.firstOperand = firstOperand;
            this.secondOperand = secondOperand;

            firstOperand.Parent = this;
            secondOperand.Parent = this;
        }

        public override ICompositePredicate Filter
        {
            get { return null; }
        }

        public override IComposite ObjectType
        {
            get { return this.firstOperand.ObjectType; }
        }

        protected override void Evaluate()
        {
            if (this.Strategies == null)
            {
                var firstOperandStrategies = new List<Strategy>(this.firstOperand.GetEvaluatedStrategies());
                var secondOperandStrategies = new List<Strategy>(this.secondOperand.GetEvaluatedStrategies());

                switch (this.operationType)
                {
                    case ExtentOperationType.Union:
                        this.Strategies = firstOperandStrategies;
                        foreach (Strategy strategy in secondOperandStrategies)
                        {
                            if (!this.Strategies.Contains(strategy))
                            {
                                this.Strategies.Add(strategy);
                            }
                        }

                        break;
                    case ExtentOperationType.Intersect:
                        this.Strategies = new List<Strategy>();
                        foreach (Strategy strategy in firstOperandStrategies)
                        {
                            if (secondOperandStrategies.Contains(strategy))
                            {
                                this.Strategies.Add(strategy);
                            }
                        }

                        break;
                    case ExtentOperationType.Except:
                        this.Strategies = firstOperandStrategies;
                        foreach (Strategy strategy in secondOperandStrategies)
                        {
                            if (this.Strategies.Contains(strategy))
                            {
                                this.Strategies.Remove(strategy);
                            }
                        }

                        break;

                    default:
                        throw new Exception("Unknown extent operation");
                }

                if (this.Sorter != null)
                {
                    this.Strategies.Sort(this.Sorter);
                }
            }
        }
    }
}