namespace Allors.Meta
{
    public partial class MetaPermission
    {
        internal override void BaseExtend()
        {
            this.OperandTypePointer.IsRequired = true;
            this.ConcreteClassPointer.IsRequired = true;
            this.OperationEnum.IsRequired = true;
        }
    }
}