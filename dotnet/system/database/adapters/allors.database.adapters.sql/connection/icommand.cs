namespace Allors.Database.Adapters.Sql
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Meta;

    public interface ICommand : IDisposable
    {
        CommandType CommandType { get; set; }

        string CommandText { get; set; }

        void AddInParameter(string parameterName, object value);

        void ObjectParameter(long objectId);

        void AddTypeParameter(IClass @class);

        void AddCountParameter(int count);

        void AddUnitRoleParameter(IRoleType roleType, object unit);

        void AddCompositeRoleParameter(long objectId);

        void AddAssociationParameter(long objectId);

        void AddCompositesRoleTableParameter(IEnumerable<long> objectIds);

        void ObjectTableParameter(IEnumerable<long> objectIds);

        void UnitTableParameter(IRoleType roleType, IEnumerable<UnitRelation> relations);

        void AddCompositeRoleTableParameter(IEnumerable<CompositeRelation> relations);

        object ExecuteScalar();

        void ExecuteNonQuery();

        IReader ExecuteReader();

        object GetValue(IReader reader, string tag, int i);
    }
}
