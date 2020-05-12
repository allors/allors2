using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Environment>))]
public class Environment : Enumeration
{
    public static Environment Development = new Environment
    {
        Value = "development"
    };

    public static Environment Staging = new Environment
    {
        Value = "staging"
    };

    public static Environment Production = new Environment
    {
        Value = "production"
    };

    public bool IsDevelopment => this == Environment.Development;

    public bool IsStaging => this == Environment.Staging;

    public bool IsProduction => this == Environment.Production;

    public static implicit operator string(Environment configuration) => configuration.Value;
}
