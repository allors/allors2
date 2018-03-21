namespace Allors.Domain
{
    using System;

    public static class AuditableExtension
    {
        public static void AppsOnDerive(this Auditable @this, ObjectOnDerive method)
        {
            var derivation = method.Derivation;
            var user = @this.Strategy.Session.GetUser();

            if (user != null)
            {
                var changeSet = derivation.ChangeSet;
                if (changeSet.Created.Contains(@this.Id))
                {
                    @this.CreationDate = DateTime.UtcNow;
                    @this.CreatedBy = user;
                }

                if (changeSet.Associations.Contains(@this.Id))
                {
                    @this.LastModifiedDate = DateTime.UtcNow;
                    @this.LastModifiedBy = user;
                }
            }
        }
    }
}
