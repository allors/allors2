namespace Allors.Meta
{
    public partial class StringTemplateClass
	{
        #region Allors
        [Id("720F0A89-CB6C-4B0C-8C8E-2CBA9A2FD6C9")]
        [AssociationId("F762A8B5-D3C8-41DF-8113-0F4467728CB0")]
        [RoleId("92920FED-5841-476B-BEE9-E9D97FC41049")]
        #endregion
        [Indexed]
        [Type(typeof(TemplatePurposeClass))]
        [Multiplicity(Multiplicity.ManyToOne)]
        public RelationType TemplatePurpose;

        internal override void AppsExtend()
	    {
	        this.TemplatePurpose.RoleType.IsRequired = true;
	    }
	}
}