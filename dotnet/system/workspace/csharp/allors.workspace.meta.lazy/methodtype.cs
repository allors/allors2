// <copyright file="MethodType.cs" company="Allors bvba">
// Copyright (c) Allors bvba. All rights reserved.
// Licensed under the LGPL license. See LICENSE file in the project root for full license information.
// </copyright>
// <summary>Defines the IObjectType type.</summary>

namespace Allors.Workspace.Meta
{
    public sealed class MethodType : IMethodTypeInternals
    {
        public MetaPopulation MetaPopulation { get; set; }

        private string Tag { get; set; }
        private ICompositeInternals ObjectType { get; set; }
        private string Name { get; set; }

        #region IMetaObject
        IMetaPopulation IMetaObject.MetaPopulation => this.MetaPopulation;

        string IMetaObject.Tag => this.Tag;

        Origin IMetaObject.Origin => Origin.Database;

        #endregion

        #region IOperandType
        string IOperandType.OperandTag => this.Tag;
        #endregion

        #region IMethodType
        IComposite IMethodType.ObjectType => this.ObjectType;
        #endregion

        public override string ToString() => this.Name;

        public MethodType Init(string tag, ICompositeInternals objectType, string name)
        {
            this.Tag = tag;
            this.ObjectType = objectType;
            this.Name = name;

            return this;
        }
    }
}
