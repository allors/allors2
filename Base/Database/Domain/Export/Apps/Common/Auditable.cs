namespace Allors.Domain
{
    using System;

    public static class AuditableExtension
    {
        public static void BaseOnDerive(this Auditable @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var user = @this.Strategy.Session.GetUser();

            if (user != null)
            {
                var changeSet = derivation.ChangeSet;
                if (changeSet.Created.Contains(@this.Id))
                {
                    @this.CreationDate = @this.Strategy.Session.Now();
                    @this.CreatedBy = user;
                }

                if (changeSet.Associations.Contains(@this.Id))
                {
                    @this.LastModifiedDate = @this.Strategy.Session.Now();
                    @this.LastModifiedBy = user;
                }
            }
        }
    }
}
