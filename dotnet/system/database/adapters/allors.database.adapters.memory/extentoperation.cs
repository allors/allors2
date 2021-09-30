// <copyright file="ExtentOperation.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Allors.Database.Adapters.Memory
{
    using System;
    using System.Collections.Generic;
    using Meta;

    internal sealed class ExtentOperation : Extent
    {
        private readonly Extent firstOperand;
        private readonly ExtentOperationType operationType;
        private readonly Extent secondOperand;

        public ExtentOperation(Transaction transaction, Extent firstOperand, Extent secondOperand, ExtentOperationType operationType)
            : base(transaction)
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

        public override ICompositePredicate Filter => null;

        public override IComposite ObjectType => this.firstOperand.ObjectType;

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
                        foreach (var strategy in secondOperandStrategies)
                        {
                            if (!this.Strategies.Contains(strategy))
                            {
                                this.Strategies.Add(strategy);
                            }
                        }

                        break;

                    case ExtentOperationType.Intersect:
                        this.Strategies = new List<Strategy>();
                        foreach (var strategy in firstOperandStrategies)
                        {
                            if (secondOperandStrategies.Contains(strategy))
                            {
                                this.Strategies.Add(strategy);
                            }
                        }

                        break;

                    case ExtentOperationType.Except:
                        this.Strategies = firstOperandStrategies;
                        foreach (var strategy in secondOperandStrategies)
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
