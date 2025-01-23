using Avalonia.Platform.Storage;

namespace Kotor.DevelopmentKit.Base.Common;

public static class FilePickerTypes
{
    public static readonly FilePickerFileType TwoDA = new FilePickerFileType("2DA File")
    {
        Patterns = ["*.2da"],
    };

    public static readonly FilePickerFileType ERF = new FilePickerFileType("ERF File")
    {
        Patterns = ["*.erf"],
    };
    public static readonly FilePickerFileType MOD = new FilePickerFileType("MOD File")
    {
        Patterns = ["*.mod"],
    };
    public static readonly FilePickerFileType RIM = new FilePickerFileType("RIM File")
    {
        Patterns = ["*.rim"],
    };
    public static readonly FilePickerFileType Encapsulator = new FilePickerFileType("Encapsulated Resource")
    {
        Patterns = ["*.erf", "*.mod", "*.rim", "*.key"],
    };
}
