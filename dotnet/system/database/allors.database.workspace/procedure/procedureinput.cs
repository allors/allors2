namespace Allors.Database
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using Data;

    public class ProcedureInput : IProcedureInput
    {
        private readonly IObjectFactory objectFactory;
        private readonly Procedure procedure;

        public ProcedureInput(IObjectFactory objectFactory, Procedure procedure)
        {
            this.objectFactory = objectFactory;
            this.procedure = procedure;
        }


        public IDictionary<string, IObject[]> Collections => this.procedure.Collections;

        public IDictionary<string, IObject> Objects => this.procedure.Objects;

        public IDictionary<string, string> Values => this.procedure.Values;

        public T[] GetCollection<T>()
        {
            var objectType = this.objectFactory.GetObjectType<T>();
            var key = objectType.PluralName;
            return this.GetCollection<T>(key);
        }

        public T[] GetCollection<T>(string key) => this.Collections.TryGetValue(key, out var collection) ? collection?.Cast<T>().ToArray() : null;

        public T GetObject<T>()
            where T : class, IObject
        {
            var objectType = this.objectFactory.GetObjectType<T>();
            var key = objectType.SingularName;
            return this.GetObject<T>(key);
        }

        public T GetObject<T>(string key) where T : class, IObject => this.Objects.TryGetValue(key, out var @object) ? (T)@object : null;

        public string GetValue(string key) => this.Values[key];
        public byte[] GetValueAsBinary(string key) => Convert.FromBase64String(this.Values[key]);

        public bool GetValueAsBoolean(string key) => XmlConvert.ToBoolean(this.Values[key]);

        public double? GetValueAsFloat(string key) => XmlConvert.ToDouble(this.Values[key]);

        public int? GetValueAsInteger(string key) => XmlConvert.ToInt32(this.Values[key]);

        public DateTime? GetValueAsDateTime(string key) => XmlConvert.ToDateTime(this.Values[key], XmlDateTimeSerializationMode.Utc);

        public decimal? GetValueAsDecimal(string key) => XmlConvert.ToDecimal(this.Values[key]);

        public Guid? GetValueAsUnique(string key) => XmlConvert.ToGuid(this.Values[key]);
    }
}
