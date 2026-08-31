using System.Diagnostics.CodeAnalysis;

namespace CaptchaN.Abstractions;

public sealed class FontsNotLoadException(string message) : InvalidOperationException(message)
{
    public string[] SearchDirectories { get; private init; } = [];

    [DoesNotReturn]
    public static void Throw()
    {
        throw new FontsNotLoadException("No fonts loaded.");
    }

    [DoesNotReturn]
    public static void Throw(string searchDir)
    {
        throw new FontsNotLoadException($"No fonts loaded: {searchDir}.") { SearchDirectories = [searchDir] };
    }

    [DoesNotReturn]
    public static void Throw(IEnumerable<string> searchDirs)
    {
        throw new FontsNotLoadException($"No fonts loaded: {string.Join("; ", searchDirs)}.") { SearchDirectories = [.. searchDirs] };
    }
}