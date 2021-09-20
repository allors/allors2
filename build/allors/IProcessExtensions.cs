using System;
using Nuke.Common.Tooling;

public static class IProcessExtensions
{
    private const int SUCCESS = 0;

    public static void ThrowOnFailure(this IProcess @this)
    {
        if (!@this.WaitForExit())
        {
            throw new Exception($"{@this}");
        }

        if (@this.ExitCode != SUCCESS)
        {
            throw new Exception($"{@this.Output}");
        }
    }
}
