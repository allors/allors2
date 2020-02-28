namespace Allors.Excel
{
    using Allors.Workspace;
    using Allors.Workspace.Meta;
    using Dipu.Excel;

    public class RoleTypeBinding : IBinding
    {
        public ISessionObject Object { get; }

        public RoleType RoleType { get; }

        public RoleType RelationType { get; }

        public bool OneWayBinding { get; }

        public bool TwoWayBinding => !this.OneWayBinding;

        /// <summary>
        /// Gets a reference to a relation, based on a key. eg Lookup WheelDiameter by its inch value.
        /// </summary>
        public System.Func<object, dynamic> GetRelation { get; internal set; }

        /// <summary>
        /// Transforms the value in the cell to something else. eg true => Yes
        /// </summary>
        public System.Func<object, dynamic> Transform { get; internal set; }

        public RoleTypeBinding(ISessionObject @object, RoleType roleType, RoleType relationType = null, bool oneWayBinding = false)
        {
            this.Object = @object;
            this.RoleType = roleType;
            this.RelationType = relationType;
            this.OneWayBinding = oneWayBinding;
        }

        public void ToCell(ICell cell)
        {
            if(this.RelationType != null)
            {
                var relation = (dynamic) this.Object.Get(this.RoleType);

                if(this.Transform == null)
                {
                    cell.Value = relation.Get(this.RelationType);
                }
                else
                {
                    cell.Value = this.Transform(relation);
                }
            }
            else
            {
                if (this.Transform == null)
                {
                    cell.Value = this.Object.Get(this.RoleType);
                }
                else
                {
                    cell.Value = this.Transform(this.Object.Get(this.RoleType));
                }
            }
        }

        public void ToDomain(ICell cell)
        {
            if (this.TwoWayBinding)
            {
                if (this.GetRelation == null)
                {
                    this.Object.Set(this.RoleType, cell.Value);
                }
                else
                {
                    var relation = this.GetRelation(cell.Value);
                    this.Object.Set(this.RoleType, relation);
                }
            }
        }
    }
}
