using System.ComponentModel;
using Nuke.Common.Tooling;

[TypeConverter(typeof(TypeConverter<Environment>))]
public class Environment : Enumeration
{
    public static Environment Development = new Environment {Value = "development"};

    public static Environment Staging = new Environment {Value = "staging"};

    public static Environment Production = new Environment {Value = "production"};

    public bool IsDevelopment => this == Development;

    public bool IsStaging => this == Staging;

    public bool IsProduction => this == Production;

    public static implicit operator string(Environment configuration) => configuration.Value;
}
